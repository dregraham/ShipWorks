using System;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using System.Threading.Tasks;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shared.IO.KeyboardShortcuts;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Window for editing a shipping profile
    /// </summary>
    [Component(RegistrationType.Self)]
    public partial class ShippingProfileEditorDlg : Form
    {
        private readonly ShippingProfileEntity profile;
        private readonly IProfileControlFactory profileControlFactory;
        private readonly IShippingProfileLoader shippingProfileLoader;
        private readonly IShortcutManager shortcutManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileEditorDlg(
            ShippingProfileEntity profile, 
            IShippingProfileLoader shippingProfileLoader,
            IShortcutManager shortcutManager,
            IProfileControlFactory profileControlFactory)
        {
            InitializeComponent();

            this.profileControlFactory = profileControlFactory;
            this.profile = profile;
            this.shortcutManager = shortcutManager;
            this.shippingProfileLoader = shippingProfileLoader;
            
            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            profileName.Text = profile.Name;
            provider.Text = EnumHelper.GetDescription((ShipmentTypeCode) profile.ShipmentType);

            EnumHelper.BindComboBox<ShortcutHotkey>(keyboardShortcut, k => shortcutManager.GetAvailableHotkeys().Contains(k));
            LoadProviders();
            LoadProfileEditor();

            profileName.Enabled = !profile.ShipmentTypePrimary;
            provider.Enabled = !profile.ShipmentTypePrimary;
        }

        /// <summary>
        /// Change the shipment type that the profile applies to
        /// </summary>
        private void LoadProfileEditor()
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) profile.ShipmentType;

            // If there was a previous control loaded, have it save itself
            ShippingProfileControlBase oldControl = panelSettings.Controls.Count > 0 ? panelSettings.Controls[0] as ShippingProfileControlBase : null;
            if (oldControl != null)
            {
                oldControl.SaveToEntity();
            }

            ShippingProfileControlBase newControl = null;

            // Create the new profile control
            if (shipmentTypeCode != ShipmentTypeCode.None)
            {
                newControl = profileControlFactory.Create(shipmentTypeCode);

                if (newControl != null)
                {
                    newControl.Width = panelSettings.Width;
                    newControl.Dock = DockStyle.Fill;
                    newControl.BackColor = Color.Transparent;

                    // Ensure the profile is loaded.  If its already there, no need to refresh
                    shippingProfileLoader.LoadProfileData(profile, false);

                    // Load the profile data into the control
                    newControl.LoadProfile(profile);
                }
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
        private async void OnOk(object sender, EventArgs e)
        {
            string name = profileName.Text.Trim();

            if (name.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a name for the profile.");
                return;
            }
            
            profile.Name = name;

            if (ShippingProfileManager.DoesNameExist(profile))
            {
                MessageHelper.ShowError(this, "A profile with the chosen name already exists.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(barcode.Text) && !shortcutManager.IsBarcodeAvailable(barcode.Text))
            {
                MessageHelper.ShowError(this, $"The barcode \"{barcode.Text}\" is already in use.");
                return;
            }

            try
            {
                // Save shortcut if user entered one
                if (keyboardShortcut.SelectedValue != null)
                {
                    await SaveShortcut(profile);
                }

                // Have the profile control save itself
                ShippingProfileControlBase profileControl = panelSettings.Controls.Count > 0 ? panelSettings.Controls[0] as ShippingProfileControlBase : null;
                profileControl?.SaveToEntity();

                ShippingProfileManager.SaveProfile(profile);

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the profile.");

                ShippingProfileManager.InitializeForCurrentSession();
                DialogResult = DialogResult.Abort;
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
        }

        /// <summary>
        /// Load configured providers into the provider combobox
        /// </summary>
        private void LoadProviders()
        {
            this.provider.SelectedValueChanged -= OnChangeProvider;

            provider.Items.Clear();            
            EnumHelper.BindComboBox<ShipmentTypeCode>(provider, t => ShippingManager.IsShipmentTypeConfigured(t));

            if(profile.ShipmentTypeCode == ShipmentTypeCode.None)
            {
                provider.SelectedValue = null;
            }
            else
            {
                provider.SelectedValue = (ShipmentTypeCode) profile.ShipmentType;
            }

            this.provider.SelectedValueChanged += OnChangeProvider;
        }

        /// <summary>
        /// When provider changes, load the appropriate control
        /// </summary>
        private void OnChangeProvider(object sender, EventArgs e)
        {
            if(provider.SelectedValue != null)
            {
                profile.Packages.Clear();
                profile.ShipmentType = (int) provider.SelectedValue;
                LoadProfileEditor();
            }
        }        

        /// <summary>
        /// Save the 
        /// </summary>
        private Task SaveShortcut(ShippingProfileEntity profile)
        {
            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Barcode = barcode.Text,
                Hotkey = (ShortcutHotkey) keyboardShortcut.SelectedValue,
                Action = (int) KeyboardShortcutCommand.ApplyProfile,
                RelatedObjectID = profile.ShippingProfileID
            };

            return shortcutManager.Save(shortcut);
        }
    }
}
