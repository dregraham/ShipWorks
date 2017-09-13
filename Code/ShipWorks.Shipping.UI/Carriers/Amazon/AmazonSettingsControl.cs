using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// AmazonSettingsControl
    /// </summary>
    public partial class AmazonSettingsControl : SettingsControlBase
    {
        private readonly IAmazonServiceTypeRepository serviceTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSettingsControl(IAmazonServiceTypeRepository serviceTypeRepository)
        {
            InitializeComponent();
            base.Initialize(ShipmentTypeCode.Amazon);
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
            AmazonShipmentType shipmentType = (AmazonShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode);

            InitializeServicePicker(shipmentType);
        }

        /// <summary>
        /// Initialize the service picker
        /// </summary>
        private void InitializeServicePicker(AmazonShipmentType shipmentType)
        {
            servicePicker.DisplayMember = "Description";
            servicePicker.ValueMember = "AmazonServiceTypeID";

            List<int> excludedServices = shipmentType.GetExcludedServiceTypes().ToList();

            List<AmazonServiceTypeEntity> amazonServices = serviceTypeRepository.Get().ToList();

            foreach (AmazonServiceTypeEntity service in amazonServices)
            {
                servicePicker.Items.Add(service, !excludedServices.Contains(service.AmazonServiceTypeID));
            }
        }

        /// <summary>
        /// Returns a list of excluded service types based on the servicePicker control
        /// </summary>
        public override IEnumerable<int> GetExcludedServices()
        {
            // Checked list box only exposes checked items or all items. Since we want unchecked, we'll just take the difference
            // between all services and checked services.
            List<int> includedServices = servicePicker.CheckedItems.Cast<AmazonServiceTypeEntity>().Select(s => s.AmazonServiceTypeID).ToList();

            return servicePicker.Items.Cast<AmazonServiceTypeEntity>()
                .Select(serviceType => serviceType.AmazonServiceTypeID)
                .Where(serviceTypeID => !includedServices.Contains(serviceTypeID));
        }
    }
}