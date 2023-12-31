﻿using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.LemonStand;

namespace ShipWorks.Stores.UI.Platforms.LemonStand
{
    public partial class LemonStandAccountSettingsControl : AccountSettingsControlBase
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="LemonStandAccountSettingsControl"/> class.
        /// </summary>
        public LemonStandAccountSettingsControl() : this(LogManager.GetLogger(typeof(LemonStandAccountSettingsControl)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LemonStandAccountSettingsControl"/> class.
        /// </summary>
        private LemonStandAccountSettingsControl(ILog log)
        {
            this.log = log;

            InitializeComponent();
        }

        /// <summary>
        ///     Load store settings from the entity to the GUI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            LemonStandStoreEntity lemonStandStore = store as LemonStandStoreEntity;
            if (lemonStandStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to LemonStand store account settings.");
            }
            storeURLTextBox.Text = lemonStandStore.StoreURL;
            accessTokenTextBox.Text = lemonStandStore.Token;
        }

        /// <summary>
        ///     Saves the user selected settings back to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            LemonStandStoreEntity lemonStandStore = store as LemonStandStoreEntity;
            if (lemonStandStore == null)
            {
                throw new ArgumentException("A non GenericStore store was passed to LemonStand store account settings.");
            }

            lemonStandStore.StoreURL = storeURLTextBox.Text;
            lemonStandStore.Token = accessTokenTextBox.Text;

            // see if we need to test the settings because they changed in some way
            if (ConnectionVerificationNeeded(lemonStandStore))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    LemonStandWebClient client = new LemonStandWebClient(lemonStandStore);
                    //Check to see if we have access to LemonStand with the new creds
                    //Ask for some order statuses
                    try
                    {
                        client.GetOrderStatuses();

                        LemonStandStatusCodeProvider statusProvider = new LemonStandStatusCodeProvider(lemonStandStore);
                        statusProvider.UpdateFromOnlineStore();

                    }
                    catch (Exception ex)
                    {
                        log.Error("Error validating access token", ex);

                        string message = ex.Message.Equals("The remote server returned an error: (401) Unauthorized.",
                            StringComparison.InvariantCultureIgnoreCase) ?
                            "Invalid access token" : "Invalid store URL";
                        MessageHelper.ShowError(this, message);

                        return false;
                    }

                    return true;
                }
                catch (LemonStandException ex)
                {
                    ShowConnectionException(ex);

                    return false;
                }
            }

            // Nothing changed
            return true;
        }

        /// <summary>
        ///     Hook to allow derivatives add custom error handling for connectivity testing failures.
        ///     Return true to indicate the error has been handled.
        /// </summary>
        protected virtual void ShowConnectionException(LemonStandException ex)
        {
            MessageHelper.ShowError(this, ex.Message);
        }

        /// <summary>
        ///     For determining if the connection needs to be tested
        /// </summary>
        protected virtual bool ConnectionVerificationNeeded(LemonStandStoreEntity store)
        {
            return (store.Fields[(int) LemonStandStoreFieldIndex.Token].IsChanged ||
                    store.Fields[(int) LemonStandStoreFieldIndex.StoreURL].IsChanged);
        }
    }
}