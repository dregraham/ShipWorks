namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Interface for the OneBalanceAutoFundControlViewModel
    /// </summary>
    public interface IOneBalanceAutoFundControlViewModel
    {
        /// <summary>
        /// The amount to add to the account balance when auto funding
        /// </summary>
        decimal AutoFundAmount { get; set; }

        /// <summary>
        /// The message to be displayed if we couldn't get auto fund settings
        /// </summary>
        string AutoFundError { get; set; }

        /// <summary>
        /// A value indicating if auto funding is turned on
        /// </summary>
        bool IsAutoFund { get; set; }

        /// <summary>
        /// The account balance that triggers the auto fund
        /// </summary>
        decimal MinimumBalance { get; set; }

        /// <summary>
        /// Make an API call to stamps to save the auto fund settings
        /// </summary>
        void SaveSettings();
    }
}