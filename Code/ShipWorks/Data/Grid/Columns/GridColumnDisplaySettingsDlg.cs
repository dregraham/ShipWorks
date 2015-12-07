using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.Filters;
using Interapptive.Shared;
using ShipWorks.Data.Grid.Columns;
using Divelements.SandGrid;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using Divelements.SandGrid.Rendering;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Window for editing the display settings of grid columns
    /// </summary>
    public partial class GridColumnDisplaySettingsDlg : Form
    {
        List<GridColumnDefinition> definitions;

        #region Specialize GridRow and GridColumn for preview

        /// <summary>
        /// Special row used for the preview grid
        /// </summary>
        class GridPreviewRow : GridColumnDefinitionRow
        {
            EntityGridColumn previewProvider;

            /// <summary>
            /// Constructor
            /// </summary>
            public GridPreviewRow(GridColumnDefinition definition)
                : base(definition)
            {

            }

            /// <summary>
            /// The column that knows how to draw the preview
            /// </summary>
            public EntityGridColumn PreviewProvider
            {
                get { return previewProvider; }
            }

            /// <summary>
            /// Update the column used to do the preview
            /// </summary>
            public void UpdatePreviewProvider()
            {
                if (previewProvider != null)
                {
                    Grid.Columns.Remove(previewProvider);
                }

                // Add on the real column at the end - has to be present in the Grid to work  - but we make it not visible
                previewProvider = Definition.CreateGridColumn();
                previewProvider.Visible = false;
                Grid.Columns.Add(previewProvider);

                RedrawNeeded();
            }
        }

        // Special column for drawing a preview of the column display
        class GridPreviewColumn : GridColumn
        {
            /// <summary>
            /// Override the drawing to use the column type of the selected row
            /// </summary>
            [NDependIgnoreTooManyParams]
            protected override void DrawCell(RenderingContext context, GridRow row, object value, Font cellFont, Image image, Rectangle bounds, bool selected, TextFormattingInformation textFormat, Color cellForeColor)
            {
                GridPreviewRow previewRow = (GridPreviewRow) row;
                EntityGridColumn previewProvider = previewRow.PreviewProvider;

                if (previewProvider != null)
                {
                    previewProvider.DrawPreview(context, row, previewRow.Definition.ExampleValue, cellFont, image, bounds, selected, cellForeColor);
                }
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnDisplaySettingsDlg(List<GridColumnDefinition> definitions)
        {
            InitializeComponent();
            WindowStateSaver.Manage(this);

            this.definitions = definitions;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Load all the definitions
            foreach (GridColumnDefinition definition in definitions)
            {
                GridPreviewRow gridRow = new GridPreviewRow(definition);
                sandGrid.Rows.Add(gridRow);

                gridRow.UpdatePreviewProvider();
            }
        }

        /// <summary>
        /// Column selected in the grid has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            groupBoxSettings.SuspendLayout();
            Control editorToRemove = null;

            if (groupBoxSettings.Controls.Count == 1)
            {
                GridColumnDisplayEditor editor = (GridColumnDisplayEditor) groupBoxSettings.Controls[0];
                editor.ValueChanged -= new EventHandler(OnEditorValueChanged);

                editorToRemove = editor;
            }

            if (sandGrid.SelectedElements.Count == 1)
            {
                GridPreviewRow gridRow = (GridPreviewRow) sandGrid.SelectedElements[0];
                GridColumnDisplayEditor editor = gridRow.Definition.DisplayType.CreateEditor();
                editor.ValueChanged += new EventHandler(OnEditorValueChanged);

                editor.Visible = false;
                editor.Location = new Point(6, 16);
                groupBoxSettings.Controls.Add(editor);

                editor.SendToBack();
                editor.Visible = true;
                editor.BringToFront();

                groupBoxSettings.Height = Math.Max(50, editor.Bottom + 5);
            }

            if (editorToRemove != null)
            {
                groupBoxSettings.Controls.Remove(editorToRemove);
            }

            groupBoxSettings.ResumeLayout();
        }

        /// <summary>
        /// A value in the current editor has changed
        /// </summary>
        void OnEditorValueChanged(object sender, EventArgs e)
        {
            GridPreviewRow gridRow = (GridPreviewRow) sandGrid.SelectedElements[0];
            gridRow.UpdatePreviewProvider();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                foreach (GridColumnDefinition definition in definitions)
                {
                    definition.DisplayType.SaveSettings(adapter);
                }

                adapter.Commit();
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Window is being closed
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                foreach (GridColumnDefinition definition in definitions)
                {
                    definition.DisplayType.CancelChanges();
                }
            }
        }
    }
}