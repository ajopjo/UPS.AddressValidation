
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.DTO;

namespace UPS.AddressValidation.Repository
{
    internal interface ICityStateStreetValidator
    {
        UpsValidatonResponse ValidateCityStreetZip(string city, string stateCode, string zipCode);
    }
}
