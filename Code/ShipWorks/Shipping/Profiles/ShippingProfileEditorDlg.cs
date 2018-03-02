using System;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.IO.KeyboardShortcuts;
using Interapptive.Shared.ComponentRegistration;
using System.Collections.Generic;
using ShipWorks.Shipping.Settings;
using System.Linq;
using ShipWorks.Core.Messaging;
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
        private readonly ShippingProfile profile;
        private readonly IShippingProfileService shippingProfileService;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileEditorDlg(
            ShippingProfile profile, 
            IShippingProfileService shippingProfileService,
            IProfileControlFactory profileControlFactory,
			IShippingSettings shippingSettings,
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
            barcode.Text = profile.Shortcut.Barcode;

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

            ShippingProfileControlBase newControl = null;

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
            profile.Shortcut.Hotkey = (Hotkey?) keyboardShortcut.SelectedValue;
            profile.Shortcut.Barcode = barcode.Text.Trim();
            
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
                }
            }
            
            messenger.Send(new EnableSingleScanInputFilterMessage(this));
        }

        /// <summary>
        /// Load list of available shortcuts to populated the shortcut combo box
        /// </summary>
        private void LoadShortcuts()
        {
            IEnumerable<Hotkey> availableHotkeys = shippingProfileService.GetAvailableHotkeys(profile);
            
            List<KeyValuePair<string, Hotkey?>> dataSource = new List<KeyValuePair<string, Hotkey?>>();
            dataSource.Add(new KeyValuePair<string, Hotkey?>("None", null));
            foreach (Hotkey hotkey in availableHotkeys.OrderBy(k => k))
            {                
                dataSource.Add(new KeyValuePair<string, Hotkey?>(EnumHelper.GetDescription(hotkey), hotkey));
            }

            keyboardShortcut.DisplayMember = "Key";
            keyboardShortcut.ValueMember = "Value";
            keyboardShortcut.DataSource = dataSource;

            if (profile?.Shortcut?.Hotkey != null)
            {
                keyboardShortcut.SelectedValue = profile.Shortcut.Hotkey;
            }
        }

        /// <summary>
        /// Load configured providers into the provider combobox
        /// </summary>
        private void LoadProviders()
        {
            this.provider.SelectedValueChanged -= OnChangeProvider;
            ShipmentTypeCode? profileShipmentType = profile.ShippingProfileEntity.ShipmentType;

            IEnumerable<ShipmentTypeCode> configuredShipmentTypes = shipmentTypeManager.ShipmentTypeCodes;
            if (profileShipmentType != null)
            {
                configuredShipmentTypes = configuredShipmentTypes.Union(new[] { profileShipmentType.Value });
            }

            List<KeyValuePair<string, ShipmentTypeCode?>> dataSource = new List<KeyValuePair<string, ShipmentTypeCode?>>();
            dataSource.Add(new KeyValuePair<string, ShipmentTypeCode?>("No Change", null));
            foreach(ShipmentTypeCode shipmentType in configuredShipmentTypes)
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
