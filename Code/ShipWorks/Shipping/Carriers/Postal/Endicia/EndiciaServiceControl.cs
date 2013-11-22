﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Service control for endicia
    /// </summary>
    public partial class EndiciaServiceControl : PostalServiceControlBase
    {
        readonly EndiciaReseller endiciaReseller = EndiciaReseller.None;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaServiceControl()
            : this (ShipmentTypeCode.Endicia, EndiciaReseller.None)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected EndiciaServiceControl(ShipmentTypeCode shipmentTypeCode, EndiciaReseller endiciaReseller)
            : base(shipmentTypeCode)
        {
            this.endiciaReseller = endiciaReseller;

            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode);

            EnumHelper.BindComboBox<PostalSortType>(sortType);
            EnumHelper.BindComboBox<PostalEntryFacility>(entryFacility);

            LoadEndiciaAccounts();
        }

        /// <summary>
        /// Load the list of endicia accounts
        /// </summary>
        private void LoadEndiciaAccounts()
        {
            endiciaAccount.DisplayMember = "Key";
            endiciaAccount.ValueMember = "Value";

            var accounts = EndiciaAccountManager.GetAccounts(endiciaReseller, false);

            if (accounts.Count > 0)
            {
                endiciaAccount.DataSource = accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.EndiciaAccountID)).ToList();
                endiciaAccount.Enabled = true;
            }
            else
            {
                endiciaAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                endiciaAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Load the shipment data into the ui
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();

            bool anyRequireEntryFacility = false;

            // Load the base
            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            // Load the origin
            originControl.LoadShipments(shipments);

            endiciaAccount.SelectedIndexChanged -= new EventHandler(OnChangeEndiciaAccount);
            service.SelectedIndexChanged -= new EventHandler(OnChangeServiceType);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    PostalServiceType serviceType = (PostalServiceType) shipment.Postal.Service;

                    if (PostalUtility.IsEntryFacilityRequired(serviceType))
                    {
                        anyRequireEntryFacility = true;
                    }

                    endiciaAccount.ApplyMultiValue(shipment.Postal.Endicia.EndiciaAccountID);

                    hidePostage.ApplyMultiCheck(shipment.Postal.Endicia.StealthPostage);
                    noPostage.ApplyMultiCheck(shipment.Postal.Endicia.NoPostage);

                    rubberStamp1.ApplyMultiText(shipment.Postal.Endicia.RubberStamp1);
                    rubberStamp2.ApplyMultiText(shipment.Postal.Endicia.RubberStamp2);
                    rubberStamp3.ApplyMultiText(shipment.Postal.Endicia.RubberStamp3);

                    referenceID.ApplyMultiText(shipment.Postal.Endicia.ReferenceID);

                    sortType.ApplyMultiValue((PostalSortType) shipment.Postal.SortType);
                    entryFacility.ApplyMultiValue((PostalEntryFacility) shipment.Postal.EntryFacility);
                }
            }

            sectionEntryFacility.Visible = anyRequireEntryFacility;

            endiciaAccount.SelectedIndexChanged += new EventHandler(OnChangeEndiciaAccount);
            service.SelectedIndexChanged += new EventHandler(OnChangeServiceType);

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();

            base.SaveToShipments();

            // Save the origin
            originControl.SaveToEntities();

            // Save the 
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                endiciaAccount.ReadMultiValue(v => shipment.Postal.Endicia.EndiciaAccountID = (long) v);

                hidePostage.ReadMultiCheck(c => shipment.Postal.Endicia.StealthPostage = c);
                noPostage.ReadMultiCheck(c => shipment.Postal.Endicia.NoPostage = c);

                rubberStamp1.ReadMultiText(t => shipment.Postal.Endicia.RubberStamp1 = t);
                rubberStamp2.ReadMultiText(t => shipment.Postal.Endicia.RubberStamp2 = t);
                rubberStamp3.ReadMultiText(t => shipment.Postal.Endicia.RubberStamp3 = t);

                referenceID.ReadMultiText(t => shipment.Postal.Endicia.ReferenceID = t);

                sortType.ReadMultiValue(v => shipment.Postal.SortType = (int) v);
                entryFacility.ReadMultiValue(v => shipment.Postal.EntryFacility = (int) v);
            }

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// The selected endicia account has changed
        /// </summary>
        void OnChangeEndiciaAccount(object sender, EventArgs e)
        {
            if (endiciaAccount.SelectedValue != null)
            {
                long accountID = (long) endiciaAccount.SelectedValue;

                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.Postal.Endicia.EndiciaAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
            }
        }

        /// <summary>
        /// Service type is changing
        /// </summary>
        void OnChangeServiceType(object sender, EventArgs e)
        {
            if (!service.MultiValued && service.SelectedValue != null && service.SelectedIndex >= 0)
            {
                PostalServiceType serviceType = (PostalServiceType) service.SelectedValue;

                sectionEntryFacility.Visible = PostalUtility.IsEntryFacilityRequired(serviceType);
            }
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (endiciaAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                EndiciaAccountEntity account = endiciaAccount.SelectedIndex >= 0 ? EndiciaAccountManager.GetAccount((long) endiciaAccount.SelectedValue) : null;
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
