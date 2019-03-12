﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon.SFP
{
    /// <summary>
    /// AmazonSettingsControl
    /// </summary>
    public partial class AmazonSFPSettingsControl : SettingsControlBase
    {
        private readonly IAmazonSFPServiceTypeRepository serviceTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPSettingsControl(IAmazonSFPServiceTypeRepository serviceTypeRepository)
        {
            InitializeComponent();
            base.Initialize(ShipmentTypeCode.AmazonSFP);
            this.serviceTypeRepository = serviceTypeRepository;
        }

        /// <summary>
        /// Carrier supports services
        /// </summary>
        protected override bool SupportsServices => true;

        /// <summary>
        /// Load the amazon settings
        /// </summary>
        public override void LoadSettings()
        {
            AmazonSFPShipmentType shipmentType = (AmazonSFPShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode);

            InitializeServicePicker(shipmentType);
        }

        /// <summary>
        /// Initialize the service picker
        /// </summary>
        private void InitializeServicePicker(AmazonSFPShipmentType shipmentType)
        {
            servicePicker.DisplayMember = "Key";
            servicePicker.ValueMember = "Value";

            List<int> excludedServices = shipmentType.GetExcludedServiceTypes().ToList();

            List<AmazonServiceTypeEntity> amazonServices = serviceTypeRepository.Get().ToList();

            foreach (AmazonServiceTypeEntity service in amazonServices)
            {
                servicePicker.Items
                    .Add(new KeyValuePair<string, int>(service.Description, service.AmazonServiceTypeID), !excludedServices.Contains(service.AmazonServiceTypeID));
            }
        }

        /// <summary>
        /// Returns a list of excluded service types based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            // Checked list box only exposes checked items or all items. Since we want unchecked, we'll just take the difference
            // between all services and checked services.
            List<int> includedServices = servicePicker.CheckedItems.Cast<KeyValuePair<string, int>>().Select(s => s.Value).ToList();

            return servicePicker.Items.Cast<KeyValuePair<string, int>>()
                .Select(serviceType => serviceType.Value)
                .Where(serviceTypeID => !includedServices.Contains(serviceTypeID));
        }
    }
}