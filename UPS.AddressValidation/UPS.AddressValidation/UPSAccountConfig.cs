using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.AddressValidation
{
    public class UpsAccountConfig
    {
        public string CountryCode { get; set; }
        public string AccessLicenseNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Uri StreetValidationUri { get; set; }
        public Uri CityStateZipValidationUri { get; set; }
    }
}
