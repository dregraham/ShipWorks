using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipEngine.DTOs.Registration
{
    /// <summary>
    /// Creates a UpsRegistrationRequest
    /// </summary>
    public interface IUpsRegistrationRequestFactory
    {
        /// <summary>
        /// Creates a UpsRegistrationRequest
        /// </summary>
        UpsRegistrationRequest Create(PersonAdapter person, string deviceIdentity);
    }
}