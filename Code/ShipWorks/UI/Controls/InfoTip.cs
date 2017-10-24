using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// UserControl for displaying an information message tip to the user when the mouse hovers
    /// </summary>
    public partial class InfoTip : UserControl
    {
        string title = "Title";
        string caption = "Caption";

        public event PopupEventHandler Popup;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfoTip()
        {
            InitializeComponent();

            toolTip.ToolTipTitle = title;
            toolTip.SetToolTip(pictureBox, caption);
        }

        /// <summary>
        /// The title that is displayed for the InfoTip
        /// </summary>
        [Category("InfoTip")]
        [DefaultValue("Title")]
        public virtual string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;

                toolTip.ToolTipTitle = title;
            }
        }

        /// <summary>
        /// The Text to display as the body of the InfoTip
        /// </summary>
        [Category("InfoTip")]
        [DefaultValue("Caption")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public virtual string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;

                toolTip.SetToolTip(pictureBox, caption);
            }
        }

        /// <summary>
        /// ToolTip is popping up
        /// </summary>
        private void OnPopup(object sender, PopupEventArgs e)
        {
            if (Popup != null)
            {
                Popup(this, e);
            }
        }

        /// <summary>
        /// Make sure enter works as expected from a blackbox perespective
        /// </summary>
        private void OnPictureMouseEnter(object sender, EventArgs e)
        {
            toolTip.Active = false;
            toolTip.Active = true;

            OnMouseEnter(e);
        }

        /// <summary>
        /// Make sure leave works as expected from a blackbox perespective
        /// </summary>
        private void OnPictureMouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }
    }
}
