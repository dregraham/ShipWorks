using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using Interapptive.Shared.ComponentRegistration;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Service Control for DhlExpress
    /// </summary>
    [KeyedComponent(typeof(ServiceControlBase), ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhlExpressServiceControl" /> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public DhlExpressServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.DhlExpress, rateControl)
        {
            InitializeComponent();

            this.saturdayDelivery.CheckStateChanged += this.OnRateCriteriaChanged;
            this.dutyPaid.CheckStateChanged += this.OnRateCriteriaChanged;
            this.packageControl.RateCriteriaChanged += this.OnRateCriteriaChanged;
            this.nonMachinable.CheckStateChanged += this.OnRateCriteriaChanged;
        }

        /// <summary>
        /// Initialize the combo boxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            originControl.Initialize(ShipmentTypeCode.DhlExpress);
            packageControl.Initialize();

            LoadAccounts();
        }

        /// <summary>
        /// Load the list of DhlExpress accounts
        /// </summary>
        public override void LoadAccounts()
        {
            DhlExpressAccount.DisplayMember = "Key";
            DhlExpressAccount.ValueMember = "Value";

            if (DhlExpressAccountManager.Accounts.Count > 0)
            {
                DhlExpressAccount.DataSource = DhlExpressAccountManager.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.DhlExpressAccountID)).ToList();
                DhlExpressAccount.Enabled = true;
            }
            else
            {
                DhlExpressAccount.DataSource = new List<KeyValuePair<string, long>>
                {
                    new KeyValuePair<string, long>("(No accounts)", 0)
                };

                DhlExpressAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Load the data for the list of shipments into the control
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            // The base will disable if editing is not enabled, but due to the packaging selection, we need to customize how it works
            sectionShipment.ContentPanel.Enabled = true;

            // Manually disable all shipment panel controls, except the packaging control.  They still need to be able to switch packages
            // even after processing
            foreach (Control control in sectionShipment.ContentPanel.Controls)
            {
                if (control != packageControl)
                {
                    control.Enabled = enableEditing;
                }
            }

            // Load the origin
            originControl.LoadShipments(shipments);

            DhlExpressAccount.SelectedIndexChanged -= OnChangeDhlExpressAccount;

            // Load the shipment details
            LoadShipmentDetails();

            DhlExpressAccount.SelectedIndexChanged += OnChangeDhlExpressAccount;

            // Load the package information
            packageControl.LoadShipments(LoadedShipments, enableEditing);

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Should the specified label format be included in the list of available formats
        /// </summary>
        protected override bool ShouldIncludeLabelFormatInList(ThermalLanguage format)
        {
            return format != ThermalLanguage.EPL;
        }

        /// <summary>
        /// Loads the shipment details.
        /// </summary>
        private void LoadShipmentDetails()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                Dictionary<int, string> availableServices = lifetimeScope.ResolveKeyed<IShipmentServicesBuilder>(ShipmentTypeCode.DhlExpress)
                        .BuildServiceTypeDictionary(LoadedShipments);

                service.BindToEnumAndPreserveSelection<DhlExpressServiceType>(x => availableServices.ContainsKey((int) x));
            }

            // Determine if all shipments will have the same destination service types
            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    DhlExpressAccount.ApplyMultiValue(shipment.DhlExpress.DhlExpressAccountID);
                    service.ApplyMultiValue((DhlExpressServiceType) shipment.DhlExpress.Service);

                    dutyPaid.ApplyMultiCheck(shipment.DhlExpress.DeliveredDutyPaid);
                    saturdayDelivery.ApplyMultiCheck(shipment.DhlExpress.SaturdayDelivery);
                    nonMachinable.ApplyMultiCheck(shipment.DhlExpress.NonMachinable);
                    shipDate.ApplyMultiDate(shipment.ShipDate);
                }
            }
        }

        /// <summary>
        /// The selected DhlExpress account has changed
        /// </summary>
        private void OnChangeDhlExpressAccount(object sender, EventArgs e)
        {
            if (DhlExpressAccount.SelectedValue != null)
            {
                long accountID = (long) DhlExpressAccount.SelectedValue;
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.DhlExpress.DhlExpressAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
            }
        }

        /// <summary>
        /// Save the current content of the service control to the given shipments.
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();
            originControl.SaveToEntities();

            // Save the packages
            packageControl.SaveToEntities();

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                shipment.ContentWeight = shipment.DhlExpress.Packages.Sum(p => p.Weight);
                DhlExpressAccount.ReadMultiValue(v => shipment.DhlExpress.DhlExpressAccountID = (long) v);

                service.ReadMultiValue(v =>
                {
                    if (v != null)
                    {
                        shipment.DhlExpress.Service = (int) v;
                    }
                });

                dutyPaid.ReadMultiCheck(c => shipment.DhlExpress.DeliveredDutyPaid = c);
                saturdayDelivery.ReadMultiCheck(c => shipment.DhlExpress.SaturdayDelivery = c);
                nonMachinable.ReadMultiCheck(c => shipment.DhlExpress.NonMachinable = c);
                shipDate.ReadMultiDate(v => shipment.ShipDate = v);
            }

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Refresh the weight box with the latest weight information from the loaded shipments
        /// </summary>
        public override void RefreshContentWeight()
        {
            // We need to save the package stuff so we know what weights we are dealing with
            packageControl.SaveToEntities();
            bool changes = false;

            // Go through each shipment
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                changes |= DhlExpressShipmentType.RedistributeContentWeight(shipment);
            }

            // If there were changes we have to reload the package ui
            if (changes)
            {
                packageControl.LoadShipments(LoadedShipments, base.EnableEditing);
            }
        }

        /// <summary>
        /// Handle rate selection from the grid
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;

            DhlExpressServiceType servicetype = EnumHelper.GetEnumByApiValue<DhlExpressServiceType>((string) e.Rate.OriginalTag);

            service.SelectedValue = servicetype;
            if (service.SelectedIndex == -1 && oldIndex != -1)
            {
                service.SelectedIndex = oldIndex;
            }
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (DhlExpressAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                DhlExpressAccountEntity account = DhlExpressAccount.SelectedIndex >= 0 ? DhlExpressAccountManager.GetAccount((long) DhlExpressAccount.SelectedValue) : null;
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
            OnRateCriteriaChanged(sender, e);
        }

        /// <summary>
        /// One of the values that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Called when [package control resize].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnPackageControlSizeChanged(object sender, EventArgs e)
        {
            sectionShipment.Height = (sectionShipment.Height - sectionShipment.ContentPanel.Height) + packageControl.Bottom + 4;
        }

        /// <summary>
        /// Called when the selected service is changed.
        /// </summary>
        private void OnServiceChanged(object sender, EventArgs e)
        {
            SyncSelectedRate();
        }

        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public override void SyncSelectedRate()
        {
            if (!service.MultiValued && service.SelectedValue != null)
            {
                // Update the selected rate in the rate control to coincide with the service change
                DhlExpressServiceType serviceType = (DhlExpressServiceType) service.SelectedValue;
                RateResult matchingRate = RateControl.RateGroup.Rates.FirstOrDefault(r =>
                {
                    if (r.Tag == null || r.ShipmentType != ShipmentTypeCode.DhlExpress)
                    {
                        return false;
                    }

                    return EnumHelper.GetEnumByApiValue<DhlExpressServiceType>((string) r.OriginalTag) == serviceType;
                });

                RateControl.SelectRate(matchingRate);
            }
            else
            {
                RateControl.ClearSelection();
            }
        }

        /// <summary>
        /// Flush any in-progress changes before saving
        /// </summary>
        /// <remarks>This should cause weight controls to finish, etc.</remarks>
        public override void FlushChanges()
        {
            base.FlushChanges();

            packageControl?.FlushChanges();
        }
    }
}
