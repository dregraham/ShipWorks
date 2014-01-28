using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.EquaShip.Enums;
using ShipWorks.UI.Controls;
using ShipWorks.Stores;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Control for configuring EquaShip
    /// </summary>
    public partial class EquaShipServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EquaShipServiceControl() 
            : base(ShipmentTypeCode.EquaShip)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // initialize
            dimensionsControl.Initialize();
            originControl.Initialize(Shipping.ShipmentTypeCode.EquaShip);

            // load accounts
            LoadEquashipAccounts();

            // setup binding for service
            service.DisplayMember = "Key";
            service.ValueMember = "Value";

            // confirmation
            confirmation.DisplayMember = "Key";
            confirmation.ValueMember = "Value";

            // setup binding for packaging
            packagingType.DisplayMember = "Key";
            packagingType.ValueMember = "Value";
            EnumHelper.BindComboBox<EquaShipPackageType>(packagingType);
        }

        /// <summary>
        /// Loads all of the equaship shipping accounts
        /// </summary>
        private void LoadEquashipAccounts()
        {
            if (EquaShipAccountManager.Accounts.Count > 0)
            {
                equashipAccount.DataSource = EquaShipAccountManager.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.EquaShipAccountID)).ToList();
                equashipAccount.Enabled = true;
            }
            else
            {
                equashipAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                equashipAccount.Enabled = false;
            }
            
            equashipAccount.DisplayMember = "Key";
            equashipAccount.ValueMember = "Value";
        }

        /// <summary>
        /// The Destination address was changed
        /// </summary>
        private void OnRecipientDestinationChanged(object sender, EventArgs e)
        {
            SaveToShipments();
            LoadShipmentDetails();
        }

        /// <summary>
        /// Load the shipment data into the ui
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();

            // load the base
            base.RecipientDestinationChanged -= new EventHandler(OnRecipientDestinationChanged);
            base.LoadShipments(shipments, enableEditing, enableShippingAddress);
            base.RecipientDestinationChanged += new EventHandler(OnRecipientDestinationChanged);

            // load the origin
            originControl.LoadShipments(shipments);

            // stop listening for weight changes
            dimensionsControl.ShipmentWeightBox = null;

            // shipment details
            equashipAccount.SelectedIndexChanged -= new EventHandler(OnChangeEquashipAccount);
            LoadShipmentDetails();
            equashipAccount.SelectedIndexChanged += new EventHandler(OnChangeEquashipAccount);

            // Start the dimensiosn control listening to weight changes
            dimensionsControl.ShipmentWeightBox = weight;

            UpdateInsuranceDisplay();

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Save the UI values back to the database
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();

            base.SaveToShipments();

            // save the origin
            originControl.SaveToEntities();

            // package dimensions
            dimensionsControl.SaveToEntities();

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                equashipAccount.ReadMultiValue(v => shipment.EquaShip.EquaShipAccountID = (long)v);
                service.ReadMultiValue(v => shipment.EquaShip.Service = (int)v);
                confirmation.ReadMultiValue(v => shipment.EquaShip.Confirmation = (int)v);
                shipDate.ReadMultiDate(d => shipment.ShipDate = d.Date.AddHours(12));
                packagingType.ReadMultiValue(v => shipment.EquaShip.PackageType = (int)v);
                weight.ReadMultiWeight(w => shipment.ContentWeight = w);
                emailNotification.ReadMultiCheck(b => shipment.EquaShip.EmailNotification = b);
                saturdayDelivery.ReadMultiCheck(b => shipment.EquaShip.SaturdayDelivery = b);
                customerReference.ReadMultiText(t => shipment.EquaShip.ReferenceNumber = t);
                customerDescription.ReadMultiText(t => shipment.EquaShip.Description = t);
                customerShippingNotes.ReadMultiText(t => shipment.EquaShip.ShippingNotes = t);
            }

            insuranceControl.SaveToInsuranceChoices();

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Loads all the shipment details
        /// </summary>
        private void LoadShipmentDetails()
        {
            bool allDomestic = true;
            bool allInternational = true;
            bool allHaveConfirmation = true;
            bool allLackConfirmation = true;

            EquaShipServiceType? serviceType = null;
            bool allServicesSame = true;

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                // Need to check with the store  to see if anything about the shipment was overridden in case
                // it may have effected the shipping services available (i.e. the eBay GSP program)
                ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

                if (overriddenShipment.ShipCountryCode == "US")
                {
                    //anyDomestic = true;
                    allInternational = false;
                }
                else
                {
                    //anyInternational = true;
                    allDomestic = false;
                }

                EquaShipServiceType thisService = (EquaShipServiceType)shipment.EquaShip.Service;
                if (serviceType == null)
                {
                    serviceType = thisService;
                }
                else
                {
                    if (serviceType != thisService)
                    {
                        allServicesSame = false;
                    }
                }

                if (EquaShipUtility.IsConfirmationAvailable((EquaShipServiceType)shipment.EquaShip.Service, (EquaShipPackageType)shipment.EquaShip.PackageType))
                {
                    allLackConfirmation = false;
                }
                else
                {
                    allHaveConfirmation = false;
                }
            }

            // if they weren't all the same, clear the value
            if (!allServicesSame)
            {
                serviceType = null;
            }

            // unhook events
            service.SelectedIndexChanged -= new EventHandler(OnChangeService);

            if (allDomestic || allInternational)
            {
                string countryCode = allDomestic ? "US" : "RU";

                service.DataSource = EquaShipUtility.GetValidServiceTypes(countryCode)
                    .Select(type => new KeyValuePair<string, EquaShipServiceType>(EnumHelper.GetDescription(type), type)).ToList();
            }
            else
            {
                service.DataSource = new KeyValuePair<string, EquaShipServiceType>[0];
            }

            // If they all have confirmation, load the confirmation types
            if (allHaveConfirmation)
            {
                UpdateConfirmationTypes(true);

            }
            // if none of them do, load hte None type
            else if (allLackConfirmation)
            {
                UpdateConfirmationTypes(false);
            }
            // otherwise load nothing
            else
            {
                confirmation.DataSource = new KeyValuePair<string, EquaShipConfirmationType>[0];
                confirmation.Enabled = true;
            }

            List<DimensionsAdapter> dimensions = new List<DimensionsAdapter>();
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    equashipAccount.ApplyMultiValue(shipment.EquaShip.EquaShipAccountID);
                    service.ApplyMultiValue((EquaShipServiceType)shipment.EquaShip.Service);
                    confirmation.ApplyMultiValue((EquaShipConfirmationType)shipment.EquaShip.Confirmation);
                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    packagingType.ApplyMultiValue((EquaShipPackageType)shipment.EquaShip.PackageType);
                    weight.ApplyMultiWeight(shipment.ContentWeight);
                    saturdayDelivery.ApplyMultiCheck(shipment.EquaShip.SaturdayDelivery);
                    emailNotification.ApplyMultiCheck(shipment.EquaShip.EmailNotification);
                    customerReference.ApplyMultiText(shipment.EquaShip.ReferenceNumber);
                    customerDescription.ApplyMultiText(shipment.EquaShip.Description);
                    customerShippingNotes.ApplyMultiText(shipment.EquaShip.ShippingNotes);

                    dimensions.Add(new DimensionsAdapter(shipment.EquaShip));
                }
            }

            // make sure confirmation looks multivalued if it's a mix
            if (!allHaveConfirmation && !allLackConfirmation)
            {
                confirmation.MultiValued = true;
            }

            // Load the dimensions
            dimensionsControl.LoadDimensions(dimensions);

            // reattach events
            service.SelectedIndexChanged += new EventHandler(OnChangeService);
        }

        /// <summary>
        /// Updates the Confirmation dropdown
        /// </summary>
        private void UpdateConfirmationTypes(bool available)
        {
            bool previousMulti = confirmation.MultiValued;
            object previousValue = confirmation.SelectedValue;

            List<KeyValuePair<string, EquaShipConfirmationType>> confirmationTypes = new List<KeyValuePair<string, EquaShipConfirmationType>>();

            if (available)
            {
                confirmationTypes.Add(new KeyValuePair<string, EquaShipConfirmationType>("None", EquaShipConfirmationType.None));
                confirmationTypes.Add(new KeyValuePair<string, EquaShipConfirmationType>(EnumHelper.GetDescription(EquaShipConfirmationType.Delivery), EquaShipConfirmationType.Delivery));
                confirmationTypes.Add(new KeyValuePair<string, EquaShipConfirmationType>(EnumHelper.GetDescription(EquaShipConfirmationType.Signature), EquaShipConfirmationType.Signature));
            }
            else
            {
                confirmationTypes.Add(new KeyValuePair<string, EquaShipConfirmationType>("(Not Available)", EquaShipConfirmationType.None));
            }

            confirmation.DataSource = confirmationTypes;

            // Disable it if its "None"
            confirmation.Enabled = available;

            // Set back the previous value
            if (previousMulti)
            {
                confirmation.MultiValued = true;
            }
            else if (previousValue != null)
            {
                confirmation.SelectedValue = previousValue;

                if (confirmation.SelectedIndex == -1)
                {
                    confirmation.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// The user changed the selected shipper
        /// </summary>
        void OnChangeEquashipAccount(object sender, EventArgs e)
        {
            if (equashipAccount.SelectedValue != null)
            {
                long accountID = (long)equashipAccount.SelectedValue;

                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.EquaShip.EquaShipAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
            }
        }

        /// <summary>
        /// Shipping service changed
        /// </summary>
        public void OnChangeService(object sender, EventArgs e)
        {
            // change package selection maybe
        }

        public override void RefreshContentWeight()
        {
            // Stop the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = null;

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    weight.ApplyMultiWeight(shipment.ContentWeight);
                }
            }

            // Start the dimensiosn control listening to weight changes
            dimensionsControl.ShipmentWeightBox = weight;
        }

        /// <summary>
        /// A rate has been selected
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;

            EquaShipServiceType servicetype = (EquaShipServiceType)e.Rate.Tag;

            service.SelectedValue = servicetype;
            if (service.SelectedIndex == -1 && oldIndex != -1)
            {
                service.SelectedIndex = oldIndex;
            }
        }

        /// <summary>
        /// Update the insurance rate display
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            insuranceControl.LoadInsuranceChoices(LoadedShipments.Select(shipment => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance));
        }

        /// <summary>
        /// Selected packaging type changed
        /// </summary>
        private void OnChangePackaging(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Ship Date was changed
        /// </summary>
        private void OnChangeShipDate(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (equashipAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                EquaShipAccountEntity account = equashipAccount.SelectedIndex >= 0 ? EquaShipAccountManager.GetAccount((long)equashipAccount.SelectedValue) : null;
                if (account != null)
                {
                    text += account.Description;
                }
                else
                {
                    text += "(None)";
                }
            }

            sectionFrom.ExtraText = text + ", " + originControl.OriginDescription;

            RaiseRateCriteriaChanged();
        }
    }
}
