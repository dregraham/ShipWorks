using System.Reactive;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Interface for AmazonTermsOrchestrator
    /// </summary>
    public interface IAmazonTermsOrchestrator
    {
        /// <summary>
        /// Do any work needed for Amazon terms acceptance
        /// </summary>
        Task<Unit> Handle();

        /// <summary>
        /// Have the terms been accepted
        /// </summary>
        bool TermsAccepted { get; set; }
    }
}
