﻿using System.ComponentModel;
using System.Reflection;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    public class GenericReferenceViewModel : OrderLookupViewModelBase, IReferenceViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericReferenceViewModel(IOrderLookupShipmentModel shipmentModel,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, fieldLayoutProvider)
        {

        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.Undefined;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string Title { get; protected set; } = "Reference";

        /// <summary>

        /// <summary>
        /// Field layout repository
        /// </summary>
        public override IOrderLookupFieldLayoutProvider FieldLayoutProvider => ShipmentModel.FieldLayoutProvider;

        /// Update when the order changes
        /// </summary>
        protected override void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) && ShipmentModel.SelectedOrder != null)
            {
                Handler.RaisePropertyChanged(nameof(ShipmentModel));
            }
        }
    }
}
