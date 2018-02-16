using System;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Window for editing a shipping profile
    /// </summary>
    public partial class ShippingProfileEditorDlg : Form
    {
        readonly ShippingProfileEntity profile;
        private readonly ILifetimeScope lifetimeScope;
        private readonly IProfileControlFactory profileControlFactory;
        private readonly IShippingProfileRepository shippingProfileRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileEditorDlg(ShippingProfileEntity profile, ILifetimeScope lifetimeScope, IProfileControlFactory profileControlFactory)
        {
            InitializeComponent();

            this.lifetimeScope = lifetimeScope;
            this.profileControlFactory = profileControlFactory;
            this.profile = profile;
            this.shippingProfileRepository = lifetimeScope.Resolve<IShippingProfileRepository>();

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            profileName.Text = profile.Name;
            labelShipmentType.Text = EnumHelper.GetDescription((ShipmentTypeCode) profile.ShipmentType);

            LoadProfileEditor();

            profileName.Enabled = !profile.ShipmentTypePrimary;
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
                    shippingProfileRepository.LoadProfileData(profile, false);

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
        private void OnOK(object sender, EventArgs e)
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

            try
            {
                // Have the profile control save itself
                ShippingProfileControlBase profileControl = panelSettings.Controls.Count > 0 ? panelSettings.Controls[0] as ShippingProfileControlBase : null;
                if (profileControl != null)
                {
                    profileControl.SaveToEntity();
                }

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
    }
}
