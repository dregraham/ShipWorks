using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// User control for editing dimensions
    /// </summary>
    public partial class DimensionsControl : UserControl
    {
        List<DimensionsAdapter> loadedDimensions = new List<DimensionsAdapter>();

        // This is the weight box for the overall shipment weight.  When this changes via the user
        // manually typing something in, then "add to weight" gets cleared.
        WeightControl shipmentWeightBox = null;

        /// <summary>
        /// The user has edited\changed something about the dimensions
        /// </summary>
        public event EventHandler DimensionsChanged;

        // So we know when not to raise the changed event
        bool suspendChangedEvent = false;

        // No values in the control
        bool cleared = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public DimensionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// One-time initialization
        /// </summary>
        public void Initialize()
        {
            linkManageProfiles.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
            LoadProfiles();

            profiles.SelectedIndexChanged += this.OnChangeProfile;
        }

        /// <summary>
        /// Load the profiles comboBox
        /// </summary>
        private void LoadProfiles()
        {
            List<KeyValuePair<string, long>> dataSource = new List<KeyValuePair<string, long>>();
            dataSource.Add(new KeyValuePair<string, long>("Enter Dimensions", 0));
            dataSource.AddRange(DimensionsManager.Profiles.Select(p => new KeyValuePair<string, long>(
                string.Format("{0} ({1} x {2} x {3})", p.Name, p.Length, p.Width, p.Height),
                p.DimensionsProfileID)));

            profiles.DisplayMember = "Key";
            profiles.ValueMember = "Value";
            profiles.DataSource = dataSource;
        }

        /// <summary>
        /// The weight box the control will listen to changes on to know when to clear the Add to Weight box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public WeightControl ShipmentWeightBox
        {
            get
            {
                return shipmentWeightBox;
            }
            set
            {
                if (shipmentWeightBox != null)
                {
                    shipmentWeightBox.WeightChanged -= new EventHandler(OnShipmentWeightChanged);
                }

                shipmentWeightBox = value;

                if (shipmentWeightBox != null)
                {
                    shipmentWeightBox.WeightChanged += new EventHandler(OnShipmentWeightChanged);
                }
            }
        }

        /// <summary>
        /// Indicates if all the values in the control have been cleared and are not visible
        /// </summary>
        public bool Cleared
        {
            get
            {
                return cleared;
            }
            set
            {
                if (cleared == value)
                {
                    return;
                }

                cleared = value;
                weight.Cleared = cleared;

                if (cleared)
                {
                    profiles.SelectedIndexChanged -= this.OnChangeProfile;
                    profiles.SelectedIndex = -1;
                    profiles.SelectedIndexChanged += this.OnChangeProfile;

                    length.Text = "";
                    width.Text = "";
                    height.Text = "";

                    addToWeight.Checked = false;
                }
                else
                {
                    profiles.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// The user has manually changed the overal weight of the shipment
        /// </summary>
        void OnShipmentWeightChanged(object sender, EventArgs e)
        {
            addToWeight.Checked = false;
        }

        /// <summary>
        /// Load the dimensions into the control
        /// </summary>
        public void LoadDimensions(IEnumerable<DimensionsAdapter> dimensions)
        {
            suspendChangedEvent = true;

            loadedDimensions = dimensions.ToList();

            profiles.SelectedIndexChanged -= this.OnChangeProfile;

            bool allManual = true;

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (DimensionsAdapter adapter in loadedDimensions)
                {
                    if (adapter.ProfileID != 0)
                    {
                        allManual = false;
                    }

                    profiles.ApplyMultiValue(adapter.ProfileID);

                    if (adapter.Length != 0)
                    {
                        length.ApplyMultiText(adapter.Length.ToString());
                    }
                    else
                    {
                        length.ApplyMultiText("");
                    }

                    if (adapter.Width != 0)
                    {
                        width.ApplyMultiText(adapter.Width.ToString());
                    }
                    else
                    {
                        width.ApplyMultiText("");
                    }

                    if (adapter.Height != 0)
                    {
                        height.ApplyMultiText(adapter.Height.ToString());
                    }
                    else
                    {
                        height.ApplyMultiText("");
                    }

                    addToWeight.ApplyMultiCheck(adapter.AddWeight);

                    weight.ApplyMultiWeight(adapter.Weight);
                }
            }

            UpdateEditable(allManual);

            profiles.SelectedIndexChanged += this.OnChangeProfile;

            suspendChangedEvent = false;
        }

        /// <summary>
        /// Update the editable state of the input controls
        /// </summary>
        private void UpdateEditable(bool editable)
        {
            length.ReadOnly = !editable;
            width.ReadOnly = !editable;
            height.ReadOnly = !editable;
            weight.ReadOnly = !editable;
        }

        /// <summary>
        /// Save the values in the control to the given entity\adapter.  The list of loaded entities\adapters is not affected.
        /// </summary>
        public void SaveToEntity(DimensionsAdapter adapter)
        {
            SaveToEntities(new DimensionsAdapter[] { adapter });
        }

        /// <summary>
        /// Save the data in the control to the loaded list of dimensions
        /// </summary>
        public void SaveToEntities()
        {
            SaveToEntities(loadedDimensions);
        }

        /// <summary>
        /// Save the data in the control to the given list of adapters
        /// </summary>
        private void SaveToEntities(IEnumerable<DimensionsAdapter> adapters)
        {
            if (cleared)
            {
                throw new InvalidOperationException("Cannot save when the values have been cleared.");
            }

            foreach (DimensionsAdapter adapter in adapters)
            {
                profiles.ReadMultiValue(v => adapter.ProfileID = (long) v);

                length.ReadMultiText(s => adapter.Length = ReadValue(adapter.Length, s));
                width.ReadMultiText(s => adapter.Width = ReadValue(adapter.Width, s));
                height.ReadMultiText(s => adapter.Height = ReadValue(adapter.Height, s));

                addToWeight.ReadMultiCheck(c => adapter.AddWeight = c);

                weight.ReadMultiWeight(w => adapter.Weight = w);
            }
        }

        /// <summary>
        /// Read the value from a l\w\h textbox and return the effective value
        /// </summary>
        private double ReadValue(double current, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            double lengthValue;
            if (double.TryParse(text, out lengthValue))
            {
                return lengthValue;
            }

            return current;
        }

        /// <summary>
        /// A different profile has been selected
        /// </summary>
        void OnChangeProfile(object sender, EventArgs e)
        {
            Debug.Assert(!profiles.MultiValued);

            // We don't need to call OnDimensionsChanged 4 times (length, width, height, weight),
            // so suspend change events until they are updated.
            suspendChangedEvent = true;

            DimensionsProfileEntity profile = DimensionsManager.GetProfile((long) profiles.SelectedValue);
            if (profile == null)
            {
                length.Text = "";
                width.Text = "";
                height.Text = "";
                weight.Weight = 0;
            }
            else
            {
                length.Text = profile.Length.ToString();
                width.Text = profile.Width.ToString();
                height.Text = profile.Height.ToString();

                weight.Weight = profile.Weight;
            }

            // Go back to listening for changed events
            suspendChangedEvent = false;

            // Manually fire OnDimensionsChanged so we get any rate/ShipSense changes updated
            OnDimensionsChanged(null, null);

            UpdateEditable(profile == null);
        }

        /// <summary>
        /// Open the manager to add\edit\delete profiles
        /// </summary>
        private void OnManageProfiles(object sender, EventArgs e)
        {
            using (DimensionsManagerDlg dlg = new DimensionsManagerDlg())
            {
                dlg.ShowDialog(this);
            }

            bool multiValue = profiles.MultiValued;
            long oldProfile = multiValue ? -1 : (long) profiles.SelectedValue;

            profiles.SelectedIndexChanged -= this.OnChangeProfile;

            LoadProfiles();

            if (multiValue)
            {
                profiles.MultiValued = true;
            }
            else
            {
                if (profiles.SelectedValue == null || oldProfile != (long) profiles.SelectedValue)
                {
                    profiles.SelectedValue = oldProfile;
                }

                if (profiles.SelectedValue == null)
                {
                    profiles.SelectedValue = (long) 0;
                    OnChangeProfile(null, EventArgs.Empty);
                }
                else if (oldProfile != 0)
                {
                    OnChangeProfile(null, EventArgs.Empty);
                }
            }

            profiles.SelectedIndexChanged += this.OnChangeProfile;
        }

        /// <summary>
        /// Indicates that the user has changed the values entered in the dimensions control
        /// </summary>
        private void OnDimensionsChanged(object sender, EventArgs e)
        {
            if (suspendChangedEvent)
            {
                return;
            }

            DimensionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Flush any in-progress changes before saving
        /// </summary>
        /// <remarks>This should cause weight controls to finish, etc.</remarks>
        public void FlushChanges()
        {
            weight.FlushChanges();
        }
    }
}
