﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
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
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IFedExUtility fedExUtility;
        private readonly PropertyChangedHandler handler;
        private bool allowStealth;
        private bool allowNoPostage;
        private List<ThermalLanguage> labelFormats;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ctor
        /// </summary>
        public OrderLookupLabelOptionsControlViewModel(IViewModelOrchestrator orchestrator, IShipmentTypeManager shipmentTypeManager, IFedExUtility fedExUtility)
        {
            Orchestrator = orchestrator;
            Orchestrator.PropertyChanged += OrchestratorPropertyChanged;
            this.shipmentTypeManager = shipmentTypeManager;
            this.fedExUtility = fedExUtility;
            
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            
            OpenPrinterArticleCommand = new RelayCommand(OpenPrinterArticle);
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
        public List<ThermalLanguage> LabelFormats
        {
            get => labelFormats;
            set => handler.Set(nameof(LabelFormats), ref labelFormats, value);
        }

        /// <summary>
        /// The order lookup Orchestrator
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IViewModelOrchestrator Orchestrator { get; }
        
        /// <summary>
        /// Command to open the printer article
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand OpenPrinterArticleCommand { get; }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void OrchestratorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && Orchestrator.Order != null)
            {
                ShipmentEntity shipment = Orchestrator.ShipmentAdapter.Shipment;
                
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
                LabelFormats = EnumHelper.GetEnumList<ThermalLanguage>(x => ShouldIncludeLabelFormatInList(shipment, x))
                    .Select(x => x.Value).ToList();
                
                // Update the Orchestrator
                handler.RaisePropertyChanged(nameof(Orchestrator));
            }
        }

        /// <summary>
        /// Whether or not the given label format is allowed for the shipment
        /// </summary>
        private bool ShouldIncludeLabelFormatInList(ShipmentEntity shipment, ThermalLanguage labelFormat)
        {
            switch (shipment.ShipmentTypeCode)
            {
                case ShipmentTypeCode.Asendia:
                case ShipmentTypeCode.Amazon:
                case ShipmentTypeCode.DhlExpress:
                    if (labelFormat == ThermalLanguage.EPL)
                    {
                        return false;
                    }
                    break;
                case ShipmentTypeCode.FedEx:
                    if (labelFormat == ThermalLanguage.EPL &&
                        fedExUtility.IsFimsService((FedExServiceType) shipment.FedEx.Service))
                    {
                        return false;
                    }

                    if (labelFormat != ThermalLanguage.None &&
                        (shipment.FedEx.Packages?.Any(package => package.DangerousGoodsEnabled) ?? false))
                    {
                        return false;
                    }
                    break;
                case ShipmentTypeCode.iParcel:
                    if (labelFormat == ThermalLanguage.ZPL)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Open the printer help article
        /// </summary>
        private void OpenPrinterArticle()
        {
            Process.Start("http://support.shipworks.com/support/solutions/articles/140916-what-printer-should-i");
        }
    }
}
