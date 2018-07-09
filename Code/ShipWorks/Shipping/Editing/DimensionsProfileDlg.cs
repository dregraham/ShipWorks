using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Window for editing the values of a dimensions profile
    /// </summary>
    public partial class DimensionsProfileDlg : Form
    {
        DimensionsProfileEntity profile;

        /// <summary>
        /// Constructor
        /// </summary>
        public DimensionsProfileDlg(DimensionsProfileEntity profile)
        {
            InitializeComponent();

            this.profile = profile;

            name.Text = profile.Name;

            length.Text = profile.Length.ToString();
            width.Text = profile.Width.ToString();
            height.Text = profile.Height.ToString();

            weight.Weight = profile.Weight;
        }

        /// <summary>
        /// User is ready to save the dimensions profile
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            bool isNew = profile.IsNew;

            if (name.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a name for the profile.");
                return;
            }

            double lengthValue;
            if (!double.TryParse(length.Text, out lengthValue))
            {
                MessageHelper.ShowError(this, "Please enter a valid length.");
                return;
            }

            double widthValue;
            if (!double.TryParse(width.Text, out widthValue))
            {
                MessageHelper.ShowError(this, "Please enter a valid width.");
                return;
            }

            double heightValue;
            if (!double.TryParse(height.Text, out heightValue))
            {
                MessageHelper.ShowError(this, "Please enter a valid height.");
                return;
            }

            profile.Name = name.Text.Trim();

            profile.Length = lengthValue;
            profile.Width = widthValue;
            profile.Height = heightValue;

            profile.Weight = weight.Weight;

            try
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(profile);
                }

                if (isNew)
                {
                    DimensionsManager.CheckForChangesNeeded();
                }

                DialogResult = DialogResult.OK;
            }
            catch (ORMQueryExecutionException ex)
            {
                if (ex.Message.Contains("IX_SWDefault_DimensionsProfile_Name"))
                {
                    MessageHelper.ShowMessage(this, "A profile with the specified name already exists.");
                }
                else
                {
                    throw;
                }
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the profile.");

                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Rollback changes if not saved
            if (DialogResult != DialogResult.OK)
            {
                profile.RollbackChanges();
            }
        }
    }
}
