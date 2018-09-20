using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.OrderLookup.Controls.LabelOptions
{
    /// <summary>
    /// View model for the OrderLookupLabelOptionsControlViewModel
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.LabelOptions)]
    public class OrderLookupLabelOptionsControlViewModel : INotifyPropertyChanged
    {
        private readonly IOrderLookupMessageBus messageBus;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IFedExUtility fedExUtility;
        private readonly PropertyChangedHandler handler;
        private DateTime shipDate;
        private bool stealth;
        private bool noPostage;
        private LabelFormatType requestedLabelFormat;
        private bool allowStealth;
        private bool allowNoPostage;
        private List<LabelFormatType> labelFormats;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ctor
        /// </summary>
        public OrderLookupLabelOptionsControlViewModel(IOrderLookupMessageBus messageBus, IShipmentTypeManager shipmentTypeManager, IFedExUtility fedExUtility)
        {
            this.messageBus = messageBus;
            this.shipmentTypeManager = shipmentTypeManager;
            this.fedExUtility = fedExUtility;
            this.messageBus.PropertyChanged += MessageBusPropertyChanged;

            handler = new PropertyChangedHandler(this, () => PropertyChanged); 
       }

        /// <summary>
        /// Ship date of the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DateTime ShipDate
        {
            get => shipDate;            
            set => handler.Set(nameof(ShipDate), ref shipDate, value);
        }

        /// <summary>
        /// Stealth option for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Stealth
        {
            get => stealth;
            set => handler.Set(nameof(Stealth), ref stealth, value);
        }

        /// <summary>
        /// Whether or not to hide postage on the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool NoPostage
        {
            get => noPostage;
            set => handler.Set(nameof(NoPostage), ref noPostage, value);
        }

        /// <summary>
        /// Requested label format for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public LabelFormatType RequestedLabelFormat
        {
            get => requestedLabelFormat;
            set => handler.Set(nameof(RequestedLabelFormat), ref requestedLabelFormat, value);
        }
        
        /// <summary>
        /// Requested label format for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AllowStealth
        {
            get => allowStealth;
            set => handler.Set(nameof(AllowStealth), ref allowStealth, value);
        }
        
        /// <summary>
        /// Requested label format for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AllowNoPostage
        {
            get => allowNoPostage;
            set => handler.Set(nameof(AllowNoPostage), ref allowNoPostage, value);
        }

        /// <summary>
        /// List of available label formats for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<LabelFormatType> LabelFormats
        {
            get => labelFormats;
            set => handler.Set(nameof(LabelFormats), ref labelFormats, value);
        }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && messageBus.Order != null)
            {
                ShipmentEntity shipment = messageBus.ShipmentAdapter.Shipment;
                
                // Set properties from the new shipment
                ShipDate = shipment.ShipDate;
                requestedLabelFormat = (LabelFormatType) shipment.RequestedLabelFormat;
                Stealth = shipment.Postal?.Usps?.HidePostage ?? false;
                NoPostage = shipment.Postal?.NoPostage ?? false;
                
                // Determine if stealth and no postage is allowed for the new shipment
                if (shipmentTypeManager.IsPostal(shipment.ShipmentTypeCode))
                {
                    AllowStealth = true;
                    AllowNoPostage = true;
                }
                else
                {
                    AllowStealth = false;
                    AllowNoPostage = false;
                }
                
                // Set the available label formats for the new shipment
                LabelFormats = EnumHelper.GetEnumList<LabelFormatType>(x => ShouldIncludeLabelFormatInList(shipment, x))
                    .Select(x => x.Value).ToList();            
            }
        }

        /// <summary>
        /// Whether or not the given label format is allowed for the shipment
        /// </summary>
        private bool ShouldIncludeLabelFormatInList(ShipmentEntity shipment, LabelFormatType labelFormat)
        {
            switch (shipment.ShipmentTypeCode)
            {
                case ShipmentTypeCode.Asendia:
                case ShipmentTypeCode.Amazon:
                case ShipmentTypeCode.DhlExpress:
                    if (labelFormat == LabelFormatType.EPL)
                    {
                        return false;
                    }
                    break;
                case ShipmentTypeCode.FedEx:
                    if (labelFormat == LabelFormatType.EPL &&
                        fedExUtility.IsFimsService((FedExServiceType) shipment.FedEx.Service))
                    {
                        return false;
                    }

                    if (labelFormat != LabelFormatType.Standard &&
                        (shipment.FedEx.Packages?.Any(package => package.DangerousGoodsEnabled) ?? false))
                    {
                        return false;
                    }
                    break;
                case ShipmentTypeCode.iParcel:
                    if (labelFormat == LabelFormatType.ZPL)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
