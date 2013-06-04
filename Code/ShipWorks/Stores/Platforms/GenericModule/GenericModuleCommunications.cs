using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using System.Globalization;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Encapsulates the Communications settings in a GetModule response
    /// </summary>
    public class GenericModuleCommunications
    {
        /// <summary>
        /// Whether or not the Expect header should be included with requests
        /// </summary>
        public bool Expect100Continue
        {
            get;
            set;
        }


        /// <summary>
        /// Gets the expected encoding of responses from the module
        /// </summary>
        public GenericStoreResponseEncoding ResponseEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Reads the settings from the provided response xpath
        /// </summary>
        public void ReadModuleResponse(XPathNavigator xpath)
        {
            Expect100Continue = XPathUtility.EvaluateXsdBoolean(xpath, "//Communications/Http/@expect100Continue", true);

            // extract the character encoding used in the responses
            string encodingString = XPathUtility.Evaluate(xpath, "//Communications/ResponseEncoding", "UTF-8");
            if (string.Compare(encodingString, "Latin-1", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ResponseEncoding = GenericStoreResponseEncoding.Latin1;
            }
            else
            {
                ResponseEncoding = GenericStoreResponseEncoding.UTF8;
            }
        }

    }
}
