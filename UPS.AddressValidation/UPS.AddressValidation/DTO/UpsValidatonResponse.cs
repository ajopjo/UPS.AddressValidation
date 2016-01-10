using System.Collections.Generic;

namespace UPS.AddressValidation.DTO
{
    public class UpsValidatonResponse
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public IEnumerable<Address> Addresses { get; set; }

        public AddressIndicator AddressIndicator { get; set; }
    }

    public enum AddressIndicator
    {

        /// <remarks/>
        AmbiguousAddressIndicator,

        /// <remarks/>
        NoCandidatesIndicator,

        /// <remarks/>
        ValidAddressIndicator
    }
}
