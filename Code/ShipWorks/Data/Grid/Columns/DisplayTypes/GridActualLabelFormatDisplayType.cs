using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    public class GridActualLabelFormatDisplayType : GridEnumDisplayType<ThermalLanguage>
    {
        public GridActualLabelFormatDisplayType(EnumSortMethod sortMethod) : 
            base(sortMethod)
        {

        }

        public override GridColumnSortProvider CreateDefaultSortProvider(EntityField2 field)
        {
            return new GridActualLabelFormatSortMethod();
        }

        protected override object GetEntityValue(EntityBase2 entity)
        {
            object value = base.GetEntityValue(entity);

            if (value == null)
            {
                ShipmentEntity shipment = entity as ShipmentEntity;
                if (shipment != null && shipment.Processed && !shipment.Voided)
                {
                    value = ThermalLanguage.None;
                }
            }

            return value;
        }
    }

    public class GridActualLabelFormatSortMethod : GridColumnEnumDescriptionSortProvider<ThermalLanguage> 
    {
        public GridActualLabelFormatSortMethod() : 
            base(ShipmentFields.ActualLabelFormat)
        {
        }
    }
}
