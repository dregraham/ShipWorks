using System;
using Divelements.SandRibbon;
using ShipWorks.Data.Model.EntityInterfaces;

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
            Func<ShipmentTypeCode?> getCurrentShipmentType,
            Action<IShippingProfileEntity> onSelection);
    }
}
