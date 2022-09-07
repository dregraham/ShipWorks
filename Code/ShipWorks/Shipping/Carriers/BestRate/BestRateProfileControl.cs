using System;
using System.Linq;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data.Model.Custom;
using System.Windows.Forms;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// BestRate profile control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.BestRate)]
    public partial class BestRateProfileControl : ShippingProfileControlBase
    {
        private readonly ICarrierAccountRetrieverFactory accountRetrieverFactory;
        private readonly IBestRateExcludedAccountRepository excludedAccountRepository;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private BestRateProfileEntity bestRateProfile;
        private HashSet<long> allowedCarrierAccounts;

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateProfileControl(ICarrierAccountRetrieverFactory accountRetrieverFactory, IBestRateExcludedAccountRepository excludedAccountRepository, IShipmentTypeManager shipmentTypeManager)
        {
            InitializeComponent();
            this.accountRetrieverFactory = accountRetrieverFactory;
            this.excludedAccountRepository = excludedAccountRepository;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            base.LoadProfile(profile);
            LoadOrigins();
            dimensionsControl.Initialize();

            bestRateProfile = profile.BestRate;
            allowedCarrierAccounts = bestRateProfile.AllowedCarrierAccounts;

            CreateCarrierControls();

            PackageProfileEntity packageProfile = profile.Packages.Single();

            //TODO: Implement insurance wording correctly in story SHIP-156: Specifying insurance/declared value with best rate
            //if (ShippingSettings.Fetch().OnTracInsuranceProvider == (int)InsuranceProvider.Carrier)
            //{
            //    insuranceControl.UseInsuranceBoxLabel = "OnTrac Declared Value";
            //    insuranceControl.InsuredValueLabel = "Declared value:";
            //}

            EnumHelper.BindComboBox<ServiceLevelType>(transitTime);

            //From
            AddValueMapping(profile, ShippingProfileFields.OriginID, originState, origin, labelOrigin);

            //Shipment
            AddValueMapping(bestRateProfile, BestRateProfileFields.ServiceLevel, transitTimeState, transitTime, labelTransitTime);
            AddValueMapping(packageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(packageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            //Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);
        }

        /// <summary>
        /// Save to entity.
        /// </summary>
        public override void SaveToEntity()
        {
            bestRateProfile.AllowedCarrierAccounts = allowedCarrierAccounts;
            base.SaveToEntity();

            if (dimensionsControl.Enabled)
            {
                dimensionsControl.SaveToEntities();
            }
        }

        /// <summary>
        /// Load all the origins
        /// </summary>
        private void LoadOrigins()
        {
            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType(ShipmentTypeCode.BestRate).GetOrigins();

            origin.DisplayMember = "Key";
            origin.ValueMember = "Value";
            origin.DataSource = origins;
        }

        /// <summary>
        /// Create the carrier account checkboxes for the Allowed Carriers
        /// </summary>
        private void CreateCarrierControls()
        {
            var shipmentTypes = shipmentTypeManager.ShipmentTypeCodes.Except(shipmentTypeManager.BestRateExcludedShipmentTypes());
            var excludedAccounts = excludedAccountRepository.GetAll();
            var currentControlY = 10;
            foreach (var shipmentType in shipmentTypes)
            {
                IEnumerable<ICarrierAccount> carrierAccounts = accountRetrieverFactory.Create(shipmentType).AccountsReadOnly;
                if (carrierAccounts.Any())
                {
                    var header = new Label
                    {
                        Text = EnumHelper.GetDescription(shipmentType),
                        Location = new System.Drawing.Point(10, currentControlY),
                        Font = new System.Drawing.Font(this.Font, System.Drawing.FontStyle.Bold),
                        Width = 200,
                        Height = 20,
                    };
                    currentControlY += 20;
                    this.tabPageCarriers.Controls.Add(header);

                    var possibleAccounts = carrierAccounts.Where(a => !excludedAccounts.Contains(a.AccountId)).ToArray();

                    for(var i = 0; i < possibleAccounts.Length; i++)
                    {
                        var account = possibleAccounts[i];
                        var checkBox = new ValueCheckBox<ICarrierAccount>
                        {
                            Text = account.AccountDescription,
                            Value = account,
                            Checked = bestRateProfile.AllowedCarrierAccounts.Contains(account.AccountId),
                            Location = new System.Drawing.Point(20, currentControlY),
                            Width = 430,
                            Height = 20
                        };

                        checkBox.CheckStateChanged += AccountCheckBox_CheckedChanged;
                        if(i < possibleAccounts.Length - 1)
                        {
                            currentControlY += 20;
                        }
                        else
                        {
                            currentControlY += 40;
                        }

                        this.tabPageCarriers.Controls.Add(checkBox);
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for a carrier checkboxes checked changed event
        /// </summary>
        private void AccountCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var senderCheckbox = (ValueCheckBox<ICarrierAccount>) sender;
            if (senderCheckbox.Checked)
            {
                allowedCarrierAccounts.Add(senderCheckbox.Value.AccountId);
            }
            else
            {
                allowedCarrierAccounts.Remove(senderCheckbox.Value.AccountId);
            }
        }
    }
}
