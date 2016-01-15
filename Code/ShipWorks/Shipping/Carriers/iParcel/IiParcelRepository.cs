using System.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// An interface for reading/writing i-parcel data.
    /// </summary>
    public interface IiParcelRepository
    {
        /// <summary>
        /// Saves the label(s) contained in the i-parcel response for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="iParcelResponse">The i-parcel response.</param>
        void SaveLabel(ShipmentEntity shipment, DataSet iParcelResponse);

        /// <summary>
        /// Saves the tracking info contained in the data set to the shipment entity.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="iParcelResponse">The i-parcel response.</param>
        void SaveTrackingInfoToEntity(ShipmentEntity shipment, DataSet iParcelResponse);

        /// <summary>
        /// Gets the shipping settings from the data source.
        /// </summary>
        /// <returns>The ShippingSettingsEntity.</returns>
        ShippingSettingsEntity GetShippingSettings();

        /// <summary>
        /// Gets the parcel account for the given shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An iParcelAccountEntity object.</returns>
        IParcelAccountEntity GetiParcelAccount(ShipmentEntity shipment);
    }
}
