using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.DTO;
using System.IO;
using UPS.AddressValidation.DTO.CSZ.Response;
using UPS.AddressValidation.DTO.CSZ.Request;
using System.Net;
using System.Xml.Serialization;
using System.Xml;

namespace UPS.AddressValidation.Repository
{
    internal class CityStateStreetValidator : ICityStateStreetValidator
    {

        private readonly UpsAccountConfig _config;

        public CityStateStreetValidator(UpsAccountConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("_config", "Please initialize UPS configuration");
            }
            _config = config;
        }        

        public UpsValidatonResponse ValidateCityStreetZip(string city, string stateCode, string zipCode)
        {
            var message = string.Empty;
            var success = true;
            var addresses = new List<Address>();
            try
            {
                var xmlRequest = CreateXmlRequest(city, stateCode, zipCode);

                var encoder = new ASCIIEncoding();
                var encodedRequest = encoder.GetBytes(xmlRequest);

                var upsRequest = CreateUpsRequest(encodedRequest.Length);

                using (var sendStream = upsRequest.GetRequestStream())
                {
                    sendStream.Write(encodedRequest, 0, encodedRequest.Length);
                }

                using (var upsResponse = upsRequest.GetResponse())
                {

                    using (var sr = new StreamReader(upsResponse.GetResponseStream()))
                    {
                        var responseXml = sr.ReadToEnd();

                        if (!string.IsNullOrEmpty(responseXml))
                        {
                            var addressValidationResponse = ToObject<AddressValidationResponse>(responseXml);

                            if (addressValidationResponse.Response.ResponseStatusCode.Equals("0",
                                StringComparison.InvariantCultureIgnoreCase))
                            {
                                success = false;
                                message = addressValidationResponse.Response.Error.FirstOrDefault().ErrorDescription;
                            }
                            else
                            {
                                addresses.AddRange(addressValidationResponse.AddressValidationResult.Select(address => new Address()
                                {
                                    City = address.Address.City,
                                    Zip = address.PostalCodeHighEnd,
                                    ZipPlusFour = address.PostalCodeLowEnd,
                                    Quality = address.Quality,
                                    Rank = address.Rank
                                }));
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                success = false;
            }

            return new UpsValidatonResponse()
            {
                Message = message,
                Status = success,
                Addresses = addresses
            };
        }

        private string CreateXmlRequest(string city, string state, string zipCode)
        {
            var account = new AccessRequest()
            {
                AccessLicenseNumber = _config.AccessLicenseNumber,
                Password = _config.Password,
                UserId = _config.UserName
            };

            var addressValidationRequest = new AddressValidationRequest()
            {
                Request = new RequestType()
                {
                    RequestAction = "AV"
                },
                Address = new DTO.CSZ.Request.AddressType()
                {
                    PostalCode = zipCode,
                    CountryCode = _config.CountryCode,
                    StateProvinceCode = state,
                    City = city
                }
            };

            return string.Format("{0}{1}", ToXml(account), ToXml(addressValidationRequest));
        }

        private HttpWebRequest CreateUpsRequest(long requestLength)
        {

            var upsRequest = (HttpWebRequest)WebRequest.Create(_config.CityStateZipValidationUri);
            upsRequest.Method = "POST";
            upsRequest.KeepAlive = false;
            upsRequest.UserAgent = "RB.Shipping";
            upsRequest.ContentType = "application/x-www-form-urlencoded";
            upsRequest.ContentLength = requestLength;
            return upsRequest;
        }

        //can be moved to extension class
        private static string ToXml<T>(T obj)
        {
            var stringwriter = new StringWriter();
            var serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(stringwriter, obj);
            return stringwriter.ToString();
        }

        private static T ToObject<T>(string xmlString)
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream = new StringReader(xmlString);
            var reader = new XmlTextReader(stream);
            return (T)serializer.Deserialize(reader);
        }
    }
}
