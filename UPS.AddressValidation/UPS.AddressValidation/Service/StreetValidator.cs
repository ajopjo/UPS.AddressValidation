using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.DTO;

namespace UPS.AddressValidation.Service
{
    internal class StreetValidator : IStreetValidator
    {
        private Repository.IStreetValidator _streetValidator;
        private Repository.ICityStateStreetValidator _cityValidator;


        public StreetValidator(Repository.IStreetValidator streetValidator, Repository.ICityStateStreetValidator cityValidator)
        {
            if (streetValidator == null)
            {
                throw new ArgumentNullException("streetValidator", "Initialize street validator dependency");
            }
            if (cityValidator == null)
            {
                throw new ArgumentNullException("cityValidator", "Initialize city validator dependency");
            }
            _streetValidator = streetValidator;
            _cityValidator = cityValidator;
        }
        public UpsValidatonResponse ValidateAddress(Address address)
        {
            var response = _streetValidator.ValidateAddress(address);

            if (response != null && response.AddressIndicator == AddressIndicator.ValidAddressIndicator && response.Addresses != null)
            {
                var validatedAddress = response.Addresses.FirstOrDefault();
                if (
                                   !string.Equals(validatedAddress.State, address.State,
                                       StringComparison.InvariantCultureIgnoreCase))
                {
                    response.Status = false;
                    response.Message = "State is invalid";
                    response.AddressIndicator = AddressIndicator.NoCandidatesIndicator;
                }
                else if (
                    !string.Equals(validatedAddress.Zip, address.Zip,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    response.Status = false;
                    response.Message = "State is invalid";
                    response.AddressIndicator = AddressIndicator.NoCandidatesIndicator;
                }
                else if (!string.Equals(validatedAddress.City, address.City, StringComparison.InvariantCultureIgnoreCase))
                {
                    //Resolve city 
                    var cityValidatorResponse = _cityValidator.ValidateCityStreetZip(address.City, address.State, address.Zip);

                    if (cityValidatorResponse.Status)
                    {

                        var resolvedCities = cityValidatorResponse.Addresses.Select(a => a.City).ToList<string>();
                        if (string.IsNullOrEmpty(
                           resolvedCities.FirstOrDefault(
                                rc => rc.ToString().Equals(address.City, StringComparison.InvariantCultureIgnoreCase))))
                        {
                            response.Status = false;
                            response.Message = "City is invalid";
                            response.AddressIndicator = AddressIndicator.NoCandidatesIndicator;
                        }
                    }
                    else
                    {
                        response.AddressIndicator = AddressIndicator.NoCandidatesIndicator;
                        response.Message = cityValidatorResponse.Message;
                        response.Status = false;
                    }
                }
            }

            return response;
        }

        public UpsValidatonResponse ValidateStreetUPSAPI(Address address)
        {
            return _streetValidator.ValidateAddress(address);
        }
    }
}
