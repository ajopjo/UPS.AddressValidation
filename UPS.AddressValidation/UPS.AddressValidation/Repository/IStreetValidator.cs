using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.DTO;

namespace UPS.AddressValidation.Repository
{
    internal interface IStreetValidator
    {
        UpsValidatonResponse ValidateAddress(Address address);      
    }
}
