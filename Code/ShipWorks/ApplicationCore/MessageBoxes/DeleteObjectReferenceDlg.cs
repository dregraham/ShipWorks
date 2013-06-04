using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Window for confirming a deletion when there may be object usages in place
    /// </summary>
    public partial class DeleteObjectReferenceDlg : Form
    {
        List<long> keysToDelete;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteObjectReferenceDlg(string message, List<long> keysToDelete)
        {
            InitializeComponent();

            labelMessage.Text = message;
            this.keysToDelete = keysToDelete;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<string> reasons = ObjectReferenceManager.GetReferenceReasons(keysToDelete);

            if (reasons.Count == 0)
            {
                panelUsages.Visible = false;
                Height -= panelUsages.Height;
            }
            else
            {
                usages.Lines = reasons.ToArray();

                // See if it should be singular
                if (keysToDelete.Count == 1)
                {
                    labelInUse.Text = "The item being deleted is used by:";
                }
            }
        }
    }
}
