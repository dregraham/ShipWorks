using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ThreeDCartOptionSet
    {
        public int OptionSetID { get; set; }

        public string OptionSetName { get; set; }

        public int OptionSorting { get; set; }

        public bool OptionRequired { get; set; }

        public string OptionType { get; set; }

        public string OptionURL { get; set; }

        public string OptionAdditionalInformation { get; set; }

        public int OptionSizeLimit { get; set; }

        public IList<ThreeDCartOption> OptionList { get; set; }
    }
}