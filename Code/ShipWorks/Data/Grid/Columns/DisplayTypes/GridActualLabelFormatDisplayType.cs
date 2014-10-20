using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Display the ActualLabelFormat in a grid
    /// </summary>
    /// <remarks>
    /// We can't just use the normal enum display because when a shipment is processed, null means Standard.
    /// When a shipment isn't yet processed, null just means 'not set'.
    /// </remarks>
    public class GridActualLabelFormatDisplayType : GridEnumDisplayType<ThermalLanguage>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridActualLabelFormatDisplayType() : 
            base(EnumSortMethod.Description)
        {

        }

        /// <summary>
        /// Create the sort provider
        /// </summary>
        public override GridColumnSortProvider CreateDefaultSortProvider(EntityField2 field)
        {
            return new GridActualLabelFormatSortProvider();
        }

        /// <summary>
        /// Get the value of the field from the entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return base.GetEntityValue(entity) ?? GetNullThermalLanguageValue(entity as ShipmentEntity);
        }

        /// <summary>
        /// Get whether a null thermal language should actually be Standard
        /// </summary>
        private static object GetNullThermalLanguageValue(ShipmentEntity shipment)
        {
            return shipment != null && shipment.Processed && !shipment.Voided ?
                (ThermalLanguage?) ThermalLanguage.None :
                null;
        }
    }
}
