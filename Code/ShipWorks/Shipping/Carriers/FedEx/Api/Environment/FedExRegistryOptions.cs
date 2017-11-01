using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Environment
{
    /// <summary>
    /// Registry options for FedEx
    /// </summary>
    public static class FedExRegistryOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use test server] based on a registry setting.
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("FedExTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("FedExTestServer", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [use list rates] based on a registry setting. Indicates if LIST rates are in
        /// effect, instead of the standard ACCOUNT rates
        /// </summary>
        public static bool UseListRates
        {
            get { return InterapptiveOnly.Registry.GetValue("FedExListRates", false); }
            set { InterapptiveOnly.Registry.SetValue("FedExListRates", value); }
        }
    }
}
