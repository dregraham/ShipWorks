using System;
using Divelements.SandRibbon;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Service for managing profile popup menus
    /// </summary>
    public interface IProfilePopupService
    {
        /// <summary>
        /// Register the popup menu
        /// </summary>
        IDisposable BuildMenu(
            Button actualApplyProfileButton,
            Guid menuGuid,
            Func<ShipmentEntity> getCurrentShipment,
            Action<IShippingProfile> onSelection);
    }
}
