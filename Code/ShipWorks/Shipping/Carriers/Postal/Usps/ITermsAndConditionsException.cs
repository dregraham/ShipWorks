using Autofac;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Represents a terms and conditions exception
    /// </summary>
    public interface ITermsAndConditionsException
    {
        /// <summary>
        /// Opens a dialog for the user to agree to terms and conditions
        /// </summary>
        void OpenTermsAndConditionsDlg(ILifetimeScope lifetimeScope);
    }
}