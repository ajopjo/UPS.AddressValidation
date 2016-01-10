
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.DTO;

namespace UPS.AddressValidation.Service

{
    public interface ICityStateZipValidator
    {
        UpsValidatonResponse ValidateCityZip(string city, string zip);

        UpsValidatonResponse ValidateCity(string city);

        UpsValidatonResponse ValidateZip(string zip);

        UpsValidatonResponse Validate(string city, string state);
    }
}
