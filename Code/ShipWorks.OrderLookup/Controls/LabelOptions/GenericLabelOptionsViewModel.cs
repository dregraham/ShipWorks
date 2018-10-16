using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.LabelOptions
{
    /// <summary>
    /// View model for the OrderLookupLabelOptionsViewModel
    /// </summary>
    public class GenericLabelOptionsViewModel: ILabelOptionsViewModel, IDataErrorInfo
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        protected readonly PropertyChangedHandler handler;
        private DateTime? shipDate;
        private Dictionary<int, string> labelFormats;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericLabelOptionsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            this.shipmentTypeManager = shipmentTypeManager;            

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            InitializeForChangedShipment(ShipmentModel.ShipmentAdapter.Shipment);
        }

        /// <summary>
        /// Is the section expanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Expanded { get; set; } = false;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title => "Label Options";

        /// <summary>
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

        /// <summary>
        /// Shipment ship date
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Required(ErrorMessage = @"Ship date is required")]
        [DateCompare(DateCompareType.Today, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Ship date must be today or in the future.")]
        public DateTime? ShipDate
        {
            get { return shipDate; }
            set
            {
                handler.Set(nameof(ShipDate), ref shipDate, value);

                if (ShipmentModel?.ShipmentAdapter?.ShipDate != null &&
                    ShipmentModel.ShipmentAdapter.ShipDate != value)
                {
                    ShipmentModel.ShipmentAdapter.ShipDate = value.Value;
                }
            }
        }

        /// <summary>
        /// List of available label formats for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> LabelFormats
        {
            get => labelFormats;
            set => handler.Set(nameof(LabelFormats), ref labelFormats, value);
        }

        /// <summary>
        /// The order lookup ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Do nothing
        /// </summary>
        public string Error => null;

        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                // If the shipment is null or processed, don't validate anything.
                if (ShipmentModel?.ShipmentAdapter?.Shipment == null || ShipmentModel.ShipmentAdapter.Shipment.Processed)
                {
                    return string.Empty;
                }

                return InputValidation<GenericLabelOptionsViewModel>.Validate(this, columnName);
            }
        }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShipmentFields.RequestedLabelFormat))
            {
                ThermalLanguage labelFormat = (ThermalLanguage) ShipmentModel.ShipmentAdapter.Shipment.RequestedLabelFormat;
                shipmentTypeManager.Get(ShipmentModel.ShipmentAdapter.Shipment).SaveRequestedLabelFormat(labelFormat, ShipmentModel.ShipmentAdapter.Shipment);
            }

            if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) && ShipmentModel.SelectedOrder != null)
            {
                ShipmentEntity shipment = ShipmentModel.ShipmentAdapter.Shipment;

                // Set the available label formats for the new shipment
                InitializeForChangedShipment(shipment);

                // Update the ShipmentModel
                handler.RaisePropertyChanged(nameof(ShipmentModel));
            }
        }

        /// <summary>
        /// Initialize the properties for a new or changed shipment
        /// </summary>
        /// <param name="shipment"></param>
        private void InitializeForChangedShipment(ShipmentEntity shipment)
        {
            ShipDate = ShipmentModel.ShipmentAdapter.ShipDate;
            LabelFormats = EnumHelper.GetEnumList<ThermalLanguage>(x => ShouldIncludeLabelFormatInList(shipment, x))
                .Select(x => x.Value).ToDictionary(s => (int) s, s => EnumHelper.GetDescription(s));
        }

        /// <summary>
        /// Whether or not the given label format is allowed for the shipment
        /// </summary>
        protected virtual bool ShouldIncludeLabelFormatInList(ShipmentEntity shipment, ThermalLanguage labelFormat) => true;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() =>
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;

    }
}
