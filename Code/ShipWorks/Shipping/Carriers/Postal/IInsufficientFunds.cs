using System.Windows.Forms;
using Autofac;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Insufficient funds interface
    /// </summary>
    public interface IInsufficientFunds
    {
        /// <summary>
        /// Name of the provider associated with the exception
        /// </summary>
        string Provider { get; }

        /// <summary>
        /// Identifier of the account
        /// </summary>
        string AccountIdentifier { get; }

        /// <summary>
        /// Create a dialog that will allow a customer to purchase more postage
        /// </summary>
        Form CreatePostageDialog(ILifetimeScope lifetimeScope);
    }
}
