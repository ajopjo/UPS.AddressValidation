using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.DTO;
using UPS.AddressValidation.Repository.Proxy;

namespace UPS.AddressValidation.Repository
{
    internal class StreetValidator : IStreetValidator
    {
        private  UpsAccountConfig _config;
        public StreetValidator(UpsAccountConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("_config", "Please initialize UPS configuration");
            }
            _config = config;
        }
        public UpsValidatonResponse ValidateAddress(Address address)
        {
            var client = CreateAddressValidatorClinet();
            var status = true;
            var message = "Valid Address";
            AddressIndicator addressIndicator = AddressIndicator.ValidAddressIndicator;
            IEnumerable<Address> addresses = null;

            try
            {
                var request = Create(address);
                var response = client.ProcessXAV(request.UPSSecurity, request.XAVRequest);

                if (response != null)
                {

                    switch (response.ItemElementName)
                    {
                        case ItemChoiceType.AmbiguousAddressIndicator:
                            status = false;
                            message = "Ambiguous Address found";
                            addresses = Parse(response.Candidate);
                            addressIndicator = AddressIndicator.AmbiguousAddressIndicator;
                            break;
                        case ItemChoiceType.ValidAddressIndicator:
                            addresses = Parse(response.Candidate);
                            break;
                        case ItemChoiceType.NoCandidatesIndicator:
                        default:
                            status = false;
                            message = "No Address Found";
                            addressIndicator = AddressIndicator.NoCandidatesIndicator;
                            break;
                    }
                }
            }
            catch (CommunicationException e)
            {

                client.Abort();
                return Parse(e);
            }
            catch (TimeoutException e)
            {
                client.Abort();
                return Parse(e);
            }
            catch (Exception ex)
            {
                client.Abort();
                return Parse(ex);
            }
            return new UpsValidatonResponse()
            {
                Status = status,
                Message = message,
                Addresses = addresses,
                AddressIndicator = addressIndicator
            };
        }

        private XAVPortTypeClient CreateAddressValidatorClinet()
        {
            ClientBase<XAVPortType>.CacheSetting = CacheSetting.AlwaysOn;
            return new XAVPortTypeClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport), new EndpointAddress(_config.StreetValidationUri));
        }

        private XAVRequest1 Create(Address address)
        {
            var request = new XAVRequest1()
            {
                UPSSecurity = new UPSSecurity()
                {
                    ServiceAccessToken = new UPSSecurityServiceAccessToken()
                    {
                        AccessLicenseNumber = _config.AccessLicenseNumber
                    },
                    UsernameToken = new UPSSecurityUsernameToken()
                    {
                        Username = _config.UserName,
                        Password = _config.Password
                    }
                },
                XAVRequest = new XAVRequest()
                {
                    Request = new RequestType()
                    {
                        RequestOption = new[] { "1" }
                    },
                    AddressKeyFormat = new AddressKeyFormatType()
                    {
                        AddressLine = address.AddressLine.ToArray(),
                        PoliticalDivision2 = address.City,
                        PoliticalDivision1 = address.State,
                        PostcodePrimaryLow = address.Zip,
                        PostcodeExtendedLow = address.ZipPlusFour,
                        CountryCode = _config.CountryCode
                    }
                }
            };

            return request;
        }

        private UpsValidatonResponse Parse(Exception e)
        {
            var response = new UpsValidatonResponse()
            {
                Status = false
            };
            var faultException = e as FaultException<Errors>;
            response.Message = faultException != null ? faultException.Detail.Nodes[0].InnerText : e.Message;
            return response;
        }

        private IEnumerable<Address> Parse(CandidateType[] candidateTypes)
        {
            return candidateTypes.Select(candidate => new Address()
            {
                AddressLine = candidate.AddressKeyFormat.AddressLine,
                City = candidate.AddressKeyFormat.PoliticalDivision2,
                State = candidate.AddressKeyFormat.PoliticalDivision1,
                Zip = candidate.AddressKeyFormat.PostcodePrimaryLow,
                ZipPlusFour = candidate.AddressKeyFormat.PostcodeExtendedLow,
                CountryCode = candidate.AddressKeyFormat.CountryCode

            }).ToList();
        }
    }
}
