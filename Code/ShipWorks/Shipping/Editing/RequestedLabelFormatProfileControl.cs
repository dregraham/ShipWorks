using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Editing
{
    public partial class RequestedLabelFormatProfileControl : UserControl, IShippingProfileControl
    {
        private ThermalLanguage[] excludedFormats;

        /// <summary>
        /// Constructor
        /// </summary>
        public RequestedLabelFormatProfileControl()
        {
            InitializeComponent();

            BindComboBox();
        }

        /// <summary>
        /// State is required by IShippingProfileControl, but we don't need it here
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// Remove the specified formats from the list of options
        /// </summary>
        public void ExcludeFormats(params ThermalLanguage[] formats)
        {
            excludedFormats = formats;

            object currentValue = labelFormat.SelectedValue;
            BindComboBox();

            if (currentValue != null && labelFormat.Items.Contains(currentValue))
            {
                labelFormat.SelectedValue = currentValue;   
            }
        }

        /// <summary>
        /// Load settings into the control
        /// </summary>
        public void LoadFromEntity(EntityBase2 entity)
        {
            ShippingProfileEntity profile = entity as ShippingProfileEntity;
            Debug.Assert(profile != null, "Entity must be a profile");

            labelFormat.SelectedValue = ((ThermalLanguage?) profile.RequestedLabelFormat).GetValueOrDefault(ThermalLanguage.None);
        }

        /// <summary>
        /// Save settings into the specified profile
        /// </summary>
        public void SaveToEntity(EntityBase2 entity)
        {
            ShippingProfileEntity profile = entity as ShippingProfileEntity;
            Debug.Assert(profile != null, "Entity must be a profile");

            profile.RequestedLabelFormat = (int)labelFormat.SelectedValue;
        }

        /// <summary>
        /// Bind the combo box to the available label formats
        /// </summary>
        private void BindComboBox()
        {
            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat, x => excludedFormats == null || !excludedFormats.Contains(x));
        }

        /// <summary>
        /// Called when [help click].
        /// </summary>
        private void OnHelpClick(object sender, System.EventArgs e)
        {
            WebHelper.OpenUrl("https://shipworks.zendesk.com/hc/en-us/articles/360022467092", this);
        }
    }
}
