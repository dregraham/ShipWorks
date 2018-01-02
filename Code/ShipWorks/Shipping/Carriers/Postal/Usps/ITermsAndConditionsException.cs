using Autofac;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Represents a terms and conditions exception
    /// </summary>
    public interface ITermsAndConditionsException
    {
        /// <summary>
        /// Associated TermsAndConditions
        /// </summary>
        IUspsTermsAndConditions TermsAndConditions { get; }
    }
}