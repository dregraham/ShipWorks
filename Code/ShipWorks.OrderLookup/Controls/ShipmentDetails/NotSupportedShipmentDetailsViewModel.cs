using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Not supported shipment details viewmodel
    /// </summary>
    [Component]
    [WpfView(typeof(NotSupportedShipmentControl))]
    class NotSupportedShipmentDetailsViewModel : IDetailsViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public NotSupportedShipmentDetailsViewModel(IOrderLookupShipmentModel shipmentModel, 
            ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider)
        {
            ShipmentModel = shipmentModel;
            Providers = carrierShipmentAdapterOptionsProvider.GetProviders(shipmentModel.ShipmentAdapter, shipmentModel.OriginalShipmentTypeCode);
        }

        /// <summary>
        /// Is the section expanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Expanded { get; set; } = true;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title => $"Shipping with {EnumHelper.GetDescription(ShipmentModel.ShipmentAdapter.ShipmentTypeCode)} is not supported in Order Lookup mode.";
        
        /// <summary>
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

        /// <summary>
        /// The shipment model
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; private set; }

        /// <summary>
        /// Shipment type code
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode ShipmentTypeCode
        {
            get => ShipmentModel.ShipmentAdapter?.ShipmentTypeCode ?? ShipmentTypeCode.None;
            set
            {
                if (value != ShipmentModel.ShipmentAdapter.ShipmentTypeCode)
                {
                    ShipmentModel.ChangeShipmentType(value);
                }
            }
        }

        /// <summary>
        /// Collection of Providers
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<ShipmentTypeCode, string> Providers
        {
            get;
            private set;
        }

        /// <summary>
        /// Dispose - Nothing to dispose
        /// </summary>
        public void Dispose() { }
    }
}
