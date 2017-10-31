using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.FedEx;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Control to manage shipments LTL freight properties.
    /// </summary>
    public partial class FedExLtlFreightControl : UserControl
    {
        private readonly ImmutableList<FedExLtlSpecialServicesControlContainer> specialServicesControls;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLtlFreightControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<FedExFreightShipmentRoleType>(role);
            EnumHelper.BindComboBox<FedExFreightCollectTermsType>(collectTerms);
            EnumHelper.BindComboBox<FedExFreightClassType>(freightClass);

            specialServicesControls = ImmutableList.Create(
                new FedExLtlSpecialServicesControlContainer(callBeforeDelivery, FedExFreightSpecialServicesType.CallBeforeDelivery),
                new FedExLtlSpecialServicesControlContainer(freezableProtection, FedExFreightSpecialServicesType.FreezableProtection),
                new FedExLtlSpecialServicesControlContainer(limitedAccessPickup, FedExFreightSpecialServicesType.LimitedAccessPickup),
                new FedExLtlSpecialServicesControlContainer(limitedAccessDelivery, FedExFreightSpecialServicesType.LimitedAccessDelivery),
                new FedExLtlSpecialServicesControlContainer(poison, FedExFreightSpecialServicesType.Poison),
                new FedExLtlSpecialServicesControlContainer(food, FedExFreightSpecialServicesType.Food),
                new FedExLtlSpecialServicesControlContainer(doNotStackPallets, FedExFreightSpecialServicesType.DoNotStackPallets),
                new FedExLtlSpecialServicesControlContainer(doNotBreakDownPallets, FedExFreightSpecialServicesType.DoNotBreakDownPallets),
                new FedExLtlSpecialServicesControlContainer(topLoad, FedExFreightSpecialServicesType.TopLoad),
                new FedExLtlSpecialServicesControlContainer(extremeLength, FedExFreightSpecialServicesType.ExtremeLength),
                new FedExLtlSpecialServicesControlContainer(liftgateAtDelivery, FedExFreightSpecialServicesType.LiftgateAtDelivery),
                new FedExLtlSpecialServicesControlContainer(liftgateAtPickup, FedExFreightSpecialServicesType.LiftgateAtPickup),
                new FedExLtlSpecialServicesControlContainer(insideDelivery, FedExFreightSpecialServicesType.InsideDelivery),
                new FedExLtlSpecialServicesControlContainer(insidePickup, FedExFreightSpecialServicesType.InsidePickup),
                new FedExLtlSpecialServicesControlContainer(freightGuarantee, FedExFreightSpecialServicesType.FreightGuarantee));
        }

        /// <summary>
        /// Load all the shipment details
        /// </summary>
        public void LoadShipmentDetails(IEnumerable<ShipmentEntity> shipments)
        {
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    FedExShipmentEntity fedEx = shipment.FedEx;

                    totalHandlingUnits.ApplyMultiText(fedEx.FreightTotalHandlinUnits.ToString());
                    role.ApplyMultiValue(fedEx.FreightRole);
                    collectTerms.ApplyMultiValue(fedEx.FreightCollectTerms);
                    freightClass.ApplyMultiValue(fedEx.FreightClass);

                    FedExFreightSpecialServicesType currentSpecialServicesType = (FedExFreightSpecialServicesType)fedEx.FreightSpecialServices;
                    foreach (var item in specialServicesControls)
                    {
                        item.Control.ApplyMultiCheck(currentSpecialServicesType.HasFlag(item.SpecialServicesType));
                    }
                }
            }
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public void SaveToShipments(IEnumerable<ShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                FedExShipmentEntity fedEx = shipment.FedEx;

                totalHandlingUnits.ReadMultiText(t => { int count; if (int.TryParse(t, out count)) shipment.FedEx.FreightTotalHandlinUnits = count; });
                role.ReadMultiValue(c => shipment.FedEx.FreightRole = (FedExFreightShipmentRoleType) c);
                collectTerms.ReadMultiValue(c => shipment.FedEx.FreightCollectTerms = (FedExFreightCollectTermsType) c);
                freightClass.ReadMultiValue(c => shipment.FedEx.FreightClass = (FedExFreightClassType) c);

                fedEx.FreightSpecialServices = SaveSpecialServices((int) fedEx.FreightSpecialServices, specialServicesControls);
            }
        }

        /// <summary>
        /// Save special services for the given control map
        /// </summary>
        private int SaveSpecialServices(int currentSpecialServicesValue, IEnumerable<FedExLtlSpecialServicesControlContainer> map) =>
            map.Aggregate(currentSpecialServicesValue, ReadSpecialServiceFromControl);


        /// <summary>
        /// Read the special services value from the control
        /// </summary>
        private int ReadSpecialServiceFromControl(int currentSpecialServicesValue, FedExLtlSpecialServicesControlContainer item)
        {
            item.Control.ReadMultiCheck(c => currentSpecialServicesValue = ApplySpecialServicesType(c, currentSpecialServicesValue, item.SpecialServicesType));
            return currentSpecialServicesValue;
        }

        /// <summary>
        /// Turn the given specialServicesTypes on or off depending on the enabled state given
        /// </summary>
        private int ApplySpecialServicesType(bool enabled, int previous, FedExFreightSpecialServicesType specialServicesTypes) =>
            enabled ?
                previous | (int)specialServicesTypes :
                previous & ~(int)specialServicesTypes;
    }

    /// <summary>
    /// Special services control type map
    /// </summary>
    internal struct FedExLtlSpecialServicesControlContainer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLtlSpecialServicesControlContainer(CheckBox control, FedExFreightSpecialServicesType specialServiceType)
        {
            Control = control;
            SpecialServicesType = specialServiceType;
        }

        /// <summary>
        /// Check box
        /// </summary>
        public CheckBox Control { get; }

        /// <summary>
        /// Special Services  type
        /// </summary>
        public FedExFreightSpecialServicesType SpecialServicesType { get; }
    }
}
