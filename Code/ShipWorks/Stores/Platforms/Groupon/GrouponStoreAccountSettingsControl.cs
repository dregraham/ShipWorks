using System;
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
    public partial class GrouponStoreAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponStoreAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the control with settings from the given store type. This is to allow
        /// the control to pull any store specific settings from the store type (i.e. the URL
        /// for the help link).
        /// </summary>
        /// <param name="storeType">Type of the store.</param>
        //public void Initialize(GrouponStoreType storeType)
        //{
        //    //helpLink.Url = storeType.AccountSettingsHelpUrl;
        //}

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

            urlTextBox.Text = grouponStore.StoreUrl;
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
            grouponStore.StoreUrl = urlTextBox.Text;
            grouponStore.Token = tokenTextBox.Text;
            grouponStore.SupplierID = supplierIDTextbox.Text;

            return true;
        }

        /// <summary>
        /// Hook to allow derivatives add custom error handling for connectivity testing failures.
        /// Return true to indicate the error has been handled.
        /// </summary>
        //protected virtual void ShowConnectionException(GenericStoreException ex)
        //{
        //    MessageHelper.ShowError(this, ex.Message);
        //}

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        //protected virtual bool ConnectionVerificationNeeded(GenericModuleStoreEntity genericStore)
        //{
        //    return (genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModuleUsername].IsChanged ||
        //            genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModulePassword].IsChanged ||
        //            genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModuleUrl].IsChanged ||
        //            genericStore.Fields[(int)GenericModuleStoreFieldIndex.ModuleOnlineStoreCode].IsChanged);
        //}
    }
}
