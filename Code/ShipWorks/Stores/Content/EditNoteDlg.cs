using System;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Window for editing (or adding) notes
    /// </summary>
    public partial class EditNoteDlg : Form
    {
        NoteEntity note;
        PanelDataMode dataMode;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditNoteDlg(NoteEntity note, PanelDataMode dataMode)
        {
            InitializeComponent();

            if (note == null)
            {
                throw new ArgumentNullException("note");
            }

            this.note = note;
            this.dataMode = dataMode;

            Text = note.IsNew ? "Add Note" : "Edit Note";

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (dataMode == PanelDataMode.LiveDatabase)
            {
                UserSession.Security.DemandPermission(PermissionType.EntityTypeEditNotes, note.EntityID);
            }

            visibility.DisplayMember = "Key";
            visibility.ValueMember = "Value";
            visibility.DataSource = EnumHelper.GetEnumList<NoteVisibility>().ToList();
            visibility.SelectedValue = (NoteVisibility) note.Visibility;

            ObjectLabel label = ObjectLabelManager.GetLabel(note.EntityID);
            labelName.Text = label.GetCustomText(true, false, true);

            pictureBox.Image = EntityUtility.GetEntityImage(EntityUtility.GetEntityType(note.EntityID), 32);

            noteText.Text = note.Text;

            if (label.IsDeleted)
            {
                visibility.Enabled = false;
                noteText.Enabled = false;
                ok.Enabled = false;
            }
        }

        /// <summary>
        /// User is saving the note
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            note.Text = noteText.Text;
            note.Visibility = (int) visibility.SelectedValue;
            note.UserID = UserSession.User.UserID;
            note.Edited = DateTime.UtcNow;

            try
            {
                if (dataMode == PanelDataMode.LiveDatabase)
                {
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        NoteManager.SaveNote(note);

                        adapter.Commit();
                    }
                }

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this,
                    "The note has been edited or deleted by another user, and your changes could not be saved.");

                DialogResult = DialogResult.Abort;
            }
            catch (SqlForeignKeyException)
            {
                MessageHelper.ShowError(this,
                    string.Format("{0} has been deleted, and the note cannot be saved.", labelName.Text));

                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            // Rollback changes if canceling
            if (dataMode == PanelDataMode.LiveDatabase && DialogResult != DialogResult.OK)
            {
                note.RollbackChanges();
            }
        }
    }
}
