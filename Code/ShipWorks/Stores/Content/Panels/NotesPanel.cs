using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Control for viewing and managing notes
    /// </summary>
    public partial class NotesPanel : SingleSelectPanelBase
    {
        PanelDataMode dataMode;

        // Only valid if the data mode is local
        List<NoteEntity> localNotes;

        /// <summary>
        /// Raised when a note is added, edited, or deleted
        /// </summary>
        public event EventHandler NotesChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public NotesPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            this.Initialize(settingsKey, definitionSet, PanelDataMode.LiveDatabase, layoutInitializer);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, PanelDataMode dataMode, Action<GridColumnLayout> layoutInitializer)
        {
            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));

            this.dataMode = dataMode;

            if (dataMode == PanelDataMode.LocalPending)
            {
                localNotes = new List<NoteEntity>();
            }
        }

        /// <summary>
        /// The PanelDataMode the panel is in.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PanelDataMode DataMode
        {
            get { return dataMode; }
        }

        /// <summary>
        /// The collection of local notes the user has added.  Only valid if the DataMode is LocalCollection.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<NoteEntity> LocalNotes
        {
            get { return localNotes; }
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType
        {
            get { return EntityType.NoteEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders, FilterTarget.Customers }; }
        }

        /// <summary>
        /// Update the gateway query filter and reload the grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID)
        {
            if (dataMode == PanelDataMode.LiveDatabase)
            {
                // Update our ability to add based on what entity we are now displaying
                addLink.Visible = UserSession.Security.HasPermission(PermissionType.EntityTypeEditNotes, entityID);

                return new QueryableEntityGateway(EntityType.NoteEntity, NoteManager.GetNotesQuery(entityID));
            }
            else
            {
                return new LocalCollectionEntityGateway<NoteEntity>(localNotes);
            }
        }

        /// <summary>
        /// Add a new note
        /// </summary>
        private void OnAddNote(object sender, EventArgs e)
        {
            Debug.Assert(EntityID != null);
            if (EntityID == null)
            {
                return;
            }

            NoteEntity note = new NoteEntity();
            note.EntityID = EntityID.Value;
            note.UserID = UserSession.User.UserID;
            note.Edited = DateTime.UtcNow;
            note.Text = "";
            note.Source = (int) NoteSource.ShipWorksUser;
            note.Visibility = (int) NoteVisibility.Internal;

            // For local ones, they have to have a fakeID until they are ready to save
            if (dataMode == PanelDataMode.LocalPending)
            {
                note.NoteID = localNotes.Count == 0 ? -44 : localNotes.Min(n => n.NoteID) - 1000;
            }

            using (EditNoteDlg dlg = new EditNoteDlg(note, dataMode))
            {
                DialogResult result = dlg.ShowDialog(this);

                if (result == DialogResult.OK && dataMode == PanelDataMode.LocalPending)
                {
                    localNotes.Add(note);
                }

                // OK and Abort both could mean data has changed - reload as long as not cancled
                if (result != DialogResult.Cancel)
                {
                    ReloadContent();

                    RaiseNotesChanged();
                }
            }
        }

        /// <summary>
        /// An action has occurred in one of the cells, like a hyperlink click.
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = (EntityGridRow) e.Row;

            if (row.EntityID == null)
            {
                MessageHelper.ShowMessage(this, "The note data is not yet loaded.");
                return;
            }

            long entityID = row.EntityID.Value;

            GridLinkAction action = (GridLinkAction) ((GridActionDisplayType) e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Delete)
            {
                DeleteNote(entityID);
            }

            if (action == GridLinkAction.Edit)
            {
                EditNote(entityID);
            }
        }

        /// <summary>
        /// Edit the selected note
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                EditNote(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Delete the selected note
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                DeleteNote(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            if (UserSession.Security.HasPermission(PermissionType.RelatedObjectEditNotes, entityID))
            {
                EditNote(entityID);
            }
        }

        /// <summary>
        /// Edit the note with the given ID
        /// </summary>
        private void EditNote(long noteID)
        {
            NoteEntity note = null;

            if (dataMode == PanelDataMode.LiveDatabase)
            {
                note = (NoteEntity) DataProvider.GetEntity(noteID);

                if (note == null)
                {
                    MessageHelper.ShowMessage(this, "The note has been deleted.");
                }
            }
            else
            {
                note = localNotes.Single(n => n.NoteID == noteID);
            }

            if (note != null)
            {
                using (EditNoteDlg dlg = new EditNoteDlg(note, dataMode))
                {
                    var result = dlg.ShowDialog(this);

                    // OK and Abort both could mean data has changed - reload as long as not cancled
                    if (result != DialogResult.Cancel)
                    {
                        ReloadContent();

                        RaiseNotesChanged();
                    }
                }
            }

        }

        /// <summary>
        /// Delete the given note
        /// </summary>
        private void DeleteNote(long noteID)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected note?");

            if (result == DialogResult.OK)
            {
                if (dataMode == PanelDataMode.LiveDatabase)
                {
                    NoteManager.DeleteNote(noteID);
                }
                else
                {
                    localNotes.Remove(localNotes.Single(n => n.NoteID == noteID));
                }

                ReloadContent();

                RaiseNotesChanged();
            }
        }

        /// <summary>
        /// Raises the even to notify lister a note has been modified in some way
        /// </summary>
        private void RaiseNotesChanged()
        {
            if (NotesChanged != null)
            {
                NotesChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuEdit.Enabled = entityGrid.Selection.Count == 1;
            menuDelete.Enabled = entityGrid.Selection.Count == 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;

            bool hasPermission = true;

            if (dataMode == PanelDataMode.LiveDatabase && EntityID != null)
            {
                if (entityGrid.Selection.Count != 1)
                {
                    hasPermission = UserSession.Security.HasPermission(PermissionType.EntityTypeEditNotes, EntityID.Value);
                }
                else
                {
                    hasPermission = UserSession.Security.HasPermission(PermissionType.RelatedObjectEditNotes, entityGrid.Selection.Keys.First());
                }
            }

            menuEdit.Available = hasPermission;
            menuDelete.Available = hasPermission;
            menuSep.Available = hasPermission;
        }
    }
}
