using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.DTO;

namespace UPS.AddressValidation.Service
{
    public interface IStreetValidator
    {
        /// <summary>
        /// This API calls Street validation and city validation to resolve complete address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        UpsValidatonResponse ValidateAddress(Address address);

        UpsValidatonResponse ValidateStreetUPSAPI(Address address);
    }
}
