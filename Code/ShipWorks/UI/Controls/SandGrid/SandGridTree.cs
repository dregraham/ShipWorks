using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ShipWorks.Templates.Controls;
using Divelements.SandGrid;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Customized SandGrid for showing as a Tree, and providing Drag & Drop functionality
    /// </summary>
    public partial class SandGridTree : SandGridDragDrop
    {
        // Help with hot-tracking
        bool suspendSelectionChangeEvent = false;

        // Only folders are allowed to be selected
        bool selectFoldersOnly = false;
        SandGridTreeRow lastSelectedRow = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public SandGridTree()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Utilty function to every row in the grid, including nested rows
        /// </summary>
        public List<SandGridTreeRow> GetAllRows()
        {
            List<SandGridTreeRow> allRows = new List<SandGridTreeRow>();

            foreach (SandGridTreeRow row in Rows)
            {
                allRows.Add(row);

                foreach (SandGridTreeRow descendant in row.GetDescendants())
                {
                    allRows.Add(descendant);
                }
            }

            return allRows;
        }

        #region Folders Only

        /// <summary>
        /// Indicates if only folders are allowed to be selected
        /// </summary>
        [DefaultValue(false)]
        public bool SelectFoldersOnly
        {
            get { return selectFoldersOnly; }
            set { selectFoldersOnly = value; }
        }

        #endregion

        #region Common

        /// <summary>
        /// Grid selection is changing \ has changed
        /// </summary>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (suspendSelectionChangeEvent)
            {
                return;
            }

            if (selectFoldersOnly)
            {
                // If its a folder, unselect it
                if (SelectedElements.Count == 1 && !((SandGridTreeRow) SelectedElements[0]).IsFolder)
                {
                    suspendSelectionChangeEvent = true;

                    SelectedElements.Clear();

                    if (lastSelectedRow != null)
                    {
                        lastSelectedRow.Selected = true;
                    }

                    suspendSelectionChangeEvent = false;

                    return;
                }
            }

            // Save the last selected row for when we are in Select Folders Only mode
            lastSelectedRow = SelectedElements.Count == 0 ? null : SelectedElements[0] as SandGridTreeRow;

            base.OnSelectionChanged(e);
        }

        #endregion
    }
}
