using System;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Summary description for Express1Helper.
    /// </summary>
    static class Express1EndiciaUtility
    {
        // Express1
        public const string Express1ProductionUrl = "https://service.express1.com/Services/EwsLabelService.svc";
        public const string Express1DevelopmentUrl = "https://www.express1dev.com/Services/EwsLabelService.svc";
        
        /// <summary>
        /// Gets the API Key, "partnerid" in Endicia parlance
        /// </summary>
        public static string ApiKey
        {
            get
            {
                if (UseTestServer)
                {
                    return "1A4E8B5F-99D4-422F-8782-7EBA12DDE312";
                }
                else
                {
                    return "2BFD18C9-EB52-4C64-867C-0E5328D264C8";
                }
            }
        }

        /// <summary>
        /// Determines if the Live Server should be used
        /// </summary>
        public static bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("Express1EndiciaTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("Express1EndiciaTestServer", value);
            }
        }
    }
}
