using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Properties;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// User control for copies and collation setting
    /// </summary>
    public partial class PageCopiesControl : UserControl
    {
        bool allowChangeCollate = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public PageCopiesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the number of copies to print
        /// </summary>
        public int Copies
        {
            get { return (int) copies.Value; }
            set { copies.Value = value; }
        }

        /// <summary>
        /// Gets or sets whether the output should be collated
        /// </summary>
        public bool Collate
        {
            get { return collate.Checked; }
            set { collate.Checked = value; }
        }

        /// <summary>
        /// Allows the user to change the collation setting
        /// </summary>
        [DefaultValue(true)]
        public bool AllowChangeCollate
        {
            get 
            { 
                return allowChangeCollate;
            }
            set
            {
                allowChangeCollate = value;

                UpdateEnableCollate();
            }
        }

        /// <summary>
        /// Update whether collating is enabled
        /// </summary>
        private void UpdateEnableCollate()
        {
            collate.Enabled = copies.Value > 1 && allowChangeCollate;
        }

        /// <summary>
        /// The number of copies selected is changing
        /// </summary>
        private void OnChangeCopies(object sender, EventArgs e)
        {
            UpdateEnableCollate();
        }

        /// <summary>
        /// The collation selection has changed
        /// </summary>
        private void OnChangeCollate(object sender, EventArgs e)
        {
            collateImage.Image = collate.Checked ? Resources.print_collate_on : Resources.print_collate_off;
        }

    }
}
