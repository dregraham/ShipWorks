using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control for selecting a time zone
    /// </summary>
    public partial class TimeZoneSelection : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TimeZoneSelection()
        {
            InitializeComponent();

            // load timezone combo
            timeZone.DataSource = TimeZoneInfo.GetSystemTimeZones();
            timeZone.DisplayMember = "DisplayName";
            timeZone.ValueMember = "Id";

            timeZone.SelectedValue = TimeZoneInfo.Local.Id;
        }

        /// <summary>
        /// Gets/Sets the selected time zone
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeZoneInfo SelectedTimeZone
        {
            get
            {
                return timeZone.SelectedItem as TimeZoneInfo;
            }
            set
            {
                if (value != null)
                {
                    timeZone.SelectedValue = value.Id;
                }
            }
        }
    }
}
