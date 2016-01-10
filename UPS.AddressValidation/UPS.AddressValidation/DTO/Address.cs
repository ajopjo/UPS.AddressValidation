using System.Collections.Generic;

namespace UPS.AddressValidation.DTO
{
    public class Address
    {
        /// <summary>
        /// Address Line 1
        /// </summary>
        public IEnumerable<string> AddressLine { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Zip
        /// </summary>

        public string Zip { get; set; }

        /// <summary>
        /// Zip Plus
        /// </summary>
        public string ZipPlusFour { get; set; }

        /// <summary>
        /// Country Code
        /// </summary>
        public string CountryCode { get; internal set; }

        /// <summary>
        /// Quality of address
        /// </summary>
        public string Quality { get; internal set; }

        /// <summary>
        /// Rank of address
        /// </summary>
        public string Rank { get; internal set; }
    }
}
