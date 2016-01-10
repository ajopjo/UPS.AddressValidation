using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.DTO;

namespace UPS.AddressValidation.Service
{
    internal class CityStateZipValidator : ICityStateZipValidator
    {
        private Repository.ICityStateStreetValidator _cityValidator;

        public CityStateZipValidator(Repository.ICityStateStreetValidator cityValidator)
        {

            if (cityValidator == null)
            {
                throw new ArgumentNullException("cityValidator", "Initialize city validator dependency");
            }
            _cityValidator = cityValidator;
        }

        public UpsValidatonResponse Validate(string city, string state)
        {
            if (string.IsNullOrEmpty(city))
            {
                throw new ArgumentNullException("city", "Required field missing");
            }
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException("state", "Required field missing");
            }
            return _cityValidator.ValidateCityStreetZip(city, state, string.Empty);
        }

        public UpsValidatonResponse ValidateCityZip(string city, string zip)
        {
            if (string.IsNullOrEmpty(city))
            {
                throw new ArgumentNullException("city", "Required field missing");
            }
            if (string.IsNullOrEmpty(zip))
            {
                throw new ArgumentNullException("zip", "Required field missing");
            }
            return _cityValidator.ValidateCityStreetZip(city, string.Empty, zip);
        }

        public UpsValidatonResponse ValidateCity(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                throw new ArgumentNullException("city", "Required field missing");
            }
            return _cityValidator.ValidateCityStreetZip(city, string.Empty, string.Empty);
        }

        public UpsValidatonResponse ValidateZip(string zip)
        {
            if (string.IsNullOrEmpty(zip))
            {
                throw new ArgumentNullException("zip", "Required field missing");
            }
            return _cityValidator.ValidateCityStreetZip(string.Empty, string.Empty, zip);
        }
    }
}
