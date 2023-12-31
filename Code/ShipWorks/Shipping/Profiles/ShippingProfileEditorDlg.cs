﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Window for editing a shipping profile
    /// </summary>
    [Component(RegistrationType.Self)]
    public partial class ShippingProfileEditorDlg : Form
    {
        private readonly IProfileControlFactory profileControlFactory;
        private IEditableShippingProfile profile;
        private readonly IShippingProfileService shippingProfileService;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileEditorDlg(
            IEditableShippingProfile profile,
            IShippingProfileService shippingProfileService,
            IProfileControlFactory profileControlFactory,
            IShipmentTypeManager shipmentTypeManager,
            IMessenger messenger)
        {
            InitializeComponent();
            this.profile = profile;
            this.shippingProfileService = shippingProfileService;
            this.profileControlFactory = profileControlFactory;
            this.shipmentTypeManager = shipmentTypeManager;
            this.messenger = messenger;

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            messenger.Send(new DisableSingleScanInputFilterMessage(this));
            profileName.Text = profile.ShippingProfileEntity.Name;
            barcode.Text = profile.Barcode;

            LoadShortcuts();
            LoadProviders();

            profileName.Enabled = !profile.ShippingProfileEntity.ShipmentTypePrimary;
            provider.Enabled = profile.ShippingProfileEntity.IsNew;

            if (profile.ShippingProfileEntity.ShipmentType != null)
            {
                provider.SelectedValue = profile.ShippingProfileEntity.ShipmentType;
            }
            else
            {
                LoadProfileEditor();
            }
        }

        /// <summary>
        /// Change the shipment type that the profile applies to
        /// </summary>
        private void LoadProfileEditor()
        {
            // If there was a previous control loaded, have it save itself
            ShippingProfileControlBase oldControl = panelSettings.Controls.Count > 0 ? panelSettings.Controls[0] as ShippingProfileControlBase : null;
            if (oldControl != null)
            {
                oldControl.SaveToEntity();
            }

            ShippingProfileControlBase newControl;

            ShipmentTypeCode? selectedProvider = (ShipmentTypeCode?) provider.SelectedValue;
            // Create the new profile control
            if (selectedProvider.HasValue)
            {
                newControl = profileControlFactory.Create(selectedProvider.Value);
            }
            else
            {
                newControl = profileControlFactory.Create();
            }

            if (newControl != null)
            {
                newControl.Width = panelSettings.Width;
                newControl.Dock = DockStyle.Fill;
                newControl.BackColor = Color.Transparent;

                // Load the profile data into the control
                newControl.LoadProfile(profile.ShippingProfileEntity);
            }

            // If there is a new control, add it now
            if (newControl != null)
            {
                panelSettings.Controls.Add(newControl);
            }

            // If there was an old control, remove it now
            if (oldControl != null)
            {
                oldControl.Dispose();
            }
        }

        /// <summary>
        /// OKing the close of the window
        /// </summary>
        private void OnOk(object sender, EventArgs e)
        {
            profile.ShippingProfileEntity.Name = profileName.Text.Trim();
            profile.ChangeShortcut(keyboardShortcut.SelectedValue as KeyboardShortcutData, barcode.Text);

            // Have the profile control save itself
            ShippingProfileControlBase profileControl = panelSettings.Controls.Count > 0
                ? panelSettings.Controls[0] as ShippingProfileControlBase
                : null;
            profileControl?.SaveToEntity();

            Result result = shippingProfileService.Save(profile);

            if (result.Failure)
            {
                MessageHelper.ShowError(this, result.Message);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                // Notify the profile control that it's being canceled
                ShippingProfileControlBase profileControl = panelSettings.Controls.Count > 0 ? panelSettings.Controls[0] as ShippingProfileControlBase : null;
                if (profileControl != null)
                {
                    profileControl.CancelChanges();

                    profile.ShippingProfileEntity.ResetDirtyFieldsToDbValues();
                    profile.Shortcut.ResetDirtyFieldsToDbValues();
                }
            }

            messenger.Send(new EnableSingleScanInputFilterMessage(this));
        }

        /// <summary>
        /// Load list of available shortcuts to populated the shortcut combo box
        /// </summary>
        private void LoadShortcuts()
        {
            IEnumerable<KeyboardShortcutData> availableHotkeys = shippingProfileService.GetAvailableHotkeys(profile);

            List<KeyValuePair<string, KeyboardShortcutData>> dataSource = new List<KeyValuePair<string, KeyboardShortcutData>>();
            dataSource.Add(new KeyValuePair<string, KeyboardShortcutData>("None", null));
            foreach (KeyboardShortcutData hotkey in availableHotkeys.OrderBy(k => k.ShortcutText))
            {
                dataSource.Add(new KeyValuePair<string, KeyboardShortcutData>(hotkey.ShortcutText, hotkey));
            }

            keyboardShortcut.DisplayMember = "Key";
            keyboardShortcut.ValueMember = "Value";
            keyboardShortcut.DataSource = dataSource;

            KeyboardShortcutData profilesKeyboardShortcut = profile.KeyboardShortcut;
            if (profilesKeyboardShortcut.ActionKey != null && profilesKeyboardShortcut.Modifiers != null)
            {
                KeyboardShortcutData selectedValue = availableHotkeys.First(a => a.Modifiers == profilesKeyboardShortcut.Modifiers && a.ActionKey == profilesKeyboardShortcut.ActionKey);
                keyboardShortcut.SelectedValue = selectedValue;
            }
        }

        /// <summary>
        /// Load configured providers into the provider combobox
        /// </summary>
        private void LoadProviders()
        {
            this.provider.SelectedValueChanged -= OnChangeProvider;
            ShipmentTypeCode? profileShipmentType = profile.ShippingProfileEntity.ShipmentType;

            IEnumerable<ShipmentTypeCode> configuredShipmentTypes = shipmentTypeManager.ConfiguredShipmentTypeCodes;
            if (profileShipmentType != null)
            {
                configuredShipmentTypes = configuredShipmentTypes.Union(new[] { profileShipmentType.Value });
            }

            List<KeyValuePair<string, ShipmentTypeCode?>> dataSource = new List<KeyValuePair<string, ShipmentTypeCode?>>();
            dataSource.Add(new KeyValuePair<string, ShipmentTypeCode?>("No Change", null));
            foreach (ShipmentTypeCode shipmentType in configuredShipmentTypes)
            {
                dataSource.Add(new KeyValuePair<string, ShipmentTypeCode?>(EnumHelper.GetDescription(shipmentType), shipmentType));
            }

            provider.DisplayMember = "Key";
            provider.ValueMember = "Value";
            provider.DataSource = dataSource;

            this.provider.SelectedValueChanged += OnChangeProvider;
        }

        /// <summary>
        /// When provider changes, load the appropriate control
        /// </summary>
        private void OnChangeProvider(object sender, EventArgs e)
        {
            profile.ChangeProvider((ShipmentTypeCode?) provider.SelectedValue);

            LoadProfileEditor();
        }

        /// <summary>
        /// Disable accept button as scans can send an enter key
        /// </summary>
        private void OnEnterBarcode(object sender, EventArgs e)
        {
            AcceptButton = null;
        }

        /// <summary>
        /// Enable accept button as we now want the enter key to work
        /// </summary>
        private void OnLeaveBarcode(object sender, EventArgs e)
        {
            AcceptButton = ok;
        }
    }
}
