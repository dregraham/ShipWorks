﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.UI;
using ShipWorks.Data.Model;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Account settings for GenericStore
    /// </summary>
    public partial class GrouponAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponAccountSettingsControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Load store settings from the entity to the GUI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            GrouponStoreEntity grouponStore = store as GrouponStoreEntity;
            if (grouponStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to GrouponStore account settings.");
            }


            tokenTextBox.Text = grouponStore.Token;
            supplierIDTextbox.Text = grouponStore.SupplierID;
        }
        
        /// <summary>
        /// Saves the user selected settings back to the store entity;
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            GrouponStoreEntity grouponStore = store as GrouponStoreEntity;
            if (grouponStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to GrouponStore account settings.");
            }

            grouponStore.Token = tokenTextBox.Text;
            grouponStore.SupplierID = supplierIDTextbox.Text;

            // see if we need to test the settings because they changed in some way
            if (ConnectionVerificationNeeded(grouponStore))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    GrouponWebClient client = new GrouponWebClient(grouponStore);
                    client.GetOrders(1);

                    return true;
                }
                catch (GrouponException ex)
                {
                    ShowConnectionException(ex);

                    return false;
                }
            }
            else
            {
                // Nothing changed
                return true;
            }
        }

        /// <summary>
        /// Hook to allow derivatives add custom error handling for connectivity testing failures.
        /// Return true to indicate the error has been handled.
        /// </summary>
        protected virtual void ShowConnectionException(GrouponException ex)
        {
            MessageHelper.ShowError(this, ex.Message);
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        protected virtual bool ConnectionVerificationNeeded(GrouponStoreEntity store)
        {
            return (store.Fields[(int)GrouponStoreFieldIndex.Token].IsChanged ||
                    store.Fields[(int)GrouponStoreFieldIndex.SupplierID].IsChanged);
        }
    }
}
