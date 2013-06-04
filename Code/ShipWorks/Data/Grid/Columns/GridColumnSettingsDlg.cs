using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid;
using ShipWorks.Templates;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Window for configuring the settings of a grid that supports detail view
    /// </summary>
    public partial class GridColumnSettingsDlg : Form
    {
        UserColumnSettingsEntity settings;
        GridColumnLayout layout;
        DetailViewSettings detailView;

        bool detailViewEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnSettingsDlg(UserColumnSettingsEntity settings, GridColumnLayout layout, bool detailViewEnabled)
        {
            InitializeComponent();

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (layout == null)
            {
                throw new ArgumentNullException("layout");
            }

            this.detailViewEnabled = detailViewEnabled;

            this.settings = settings;
            this.layout = layout;
            this.detailView = layout.DetailViewSettings;

            if (settings.GridColumnLayoutID != layout.GridColumnLayoutID)
            {
                throw new InvalidOperationException("Settings\\layout mismatch passed.");
            }
            
            if (!detailViewEnabled)
            {
                panelDetailView.Visible = false;

                gridColumnEditor.Top = panelDetailView.Top;
                gridColumnEditor.Height += panelDetailView.Height;

                MinimumSize = new Size(MinimumSize.Width, MinimumSize.Height - panelDetailView.Height);

                Height -= panelDetailView.Height;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Detail view
            if (detailViewEnabled)
            {
                EnumHelper.BindComboBox<DetailViewMode>(detailViewMode);
                detailViewMode.SelectedValue = (DetailViewMode) detailView.DetailViewMode;

                detailViewTemplate.LoadTemplates();
                detailViewTemplate.SelectedTemplate = TemplateManager.Tree.GetTemplate(detailView.TemplateID);

                detailViewHeight.Items.Add("Auto");

                detailViewHeight.MaxDropDownItems = DetailViewSettings.MaxDetailRows + 1;
                for (int i = 1; i <= DetailViewSettings.MaxDetailRows; i++)
                {
                    detailViewHeight.Items.Add(i.ToString());
                }

                detailViewHeight.SelectedIndex = detailView.DetailRows;
            }

            // Grid columns
            gridColumnEditor.LoadGridColumnLayout(layout);

            // Initial sort
            initialSort.SelectedIndex = (settings.InitialSortType == (int) GridInitialSortMethod.DefaultSort) ? 0 : 1;
        }

        /// <summary>
        /// The detail view mode is changing
        /// </summary>
        private void OnChangeDetailViewMode(object sender, EventArgs e)
        {
            groupBoxDetailsView.Enabled = ((DetailViewMode) detailViewMode.SelectedValue != DetailViewMode.Normal);
        }

        /// <summary>
        /// User wants to save current settings
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (detailViewEnabled)
            {
                detailView.DetailViewMode = (DetailViewMode) detailViewMode.SelectedValue;
                detailView.TemplateID = detailViewTemplate.SelectedTemplate != null ? detailViewTemplate.SelectedTemplate.TemplateID : 0;
                detailView.DetailRows = detailViewHeight.SelectedIndex;
            }

            settings.InitialSortType = (int) (initialSort.SelectedIndex == 0 ? GridInitialSortMethod.DefaultSort : GridInitialSortMethod.LastSort);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.SaveAndRefetch(settings);
                layout.Save(adapter);

                adapter.Commit();
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Window is closing, cancel any changes made so far
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                layout.CancelChanges();
            }
        }
    }
}
