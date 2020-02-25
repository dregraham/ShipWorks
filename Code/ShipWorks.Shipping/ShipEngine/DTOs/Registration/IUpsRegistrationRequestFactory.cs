using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipEngine.DTOs.Registration
{
    public interface IUpsRegistrationRequestFactory
    {
        Task<GenericResult<string>> Create(PersonAdapter person);
    }
}