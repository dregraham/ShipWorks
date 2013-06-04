using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Interface for shipping profile controls
    /// </summary>
    public interface IShippingProfileControl
    {
        /// <summary>
        /// State is whether the control is enabled or not
        /// </summary>
        bool State { get; set; }

        /// <summary>
        /// Saves the control data to the entity
        /// </summary>
        void SaveToEntity(EntityBase2 entity);

        /// <summary>
        /// Loads entity data to the control
        /// </summary>
        void LoadFromEntity(EntityBase2 entity);
    }
}
