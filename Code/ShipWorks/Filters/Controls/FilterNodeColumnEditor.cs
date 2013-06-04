using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using System.Collections;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Filters;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Filters.Grid;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Control for editing grid columns for a filter node
    /// </summary>
    public partial class FilterNodeColumnEditor : UserControl
    {
        FilterNodeColumnSettings settings;

        // Controls if the UI for editing a parent's filter settings is enabled
        bool enableParentEditing = true;

        // Raised when any aspect of the settings changes
        public event EventHandler SettingsChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterNodeColumnEditor()
        {
            InitializeComponent();

            layoutEditor.GridColumnLayoutChanged += new EventHandler(OnGridColumnLayoutChanged);
        }

        /// <summary>
        /// The loaded grid settings
        /// </summary>
        [Browsable(false)]
        public FilterNodeColumnSettings Settings
        {
            get { return settings; }
        }

        /// <summary>
        /// Controls if the column UI when inherit columns is selected is enabled.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool EnableParentEditing
        {
            get
            {
                return enableParentEditing;
            }
            set
            {
                enableParentEditing = value;
            }
        }

        /// <summary>
        /// Load the editor for the given settings
        /// </summary>
        public void LoadSettings(FilterNodeColumnSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;

            radioUseCustom.CheckedChanged -= new EventHandler(OnChangeUseInherited);
            radioUseParent.CheckedChanged -= new EventHandler(OnChangeUseInherited);

            radioUseCustom.Checked = !settings.Inherited;
            radioUseParent.Checked = settings.Inherited;

            radioUseCustom.CheckedChanged += new EventHandler(OnChangeUseInherited);
            radioUseParent.CheckedChanged += new EventHandler(OnChangeUseInherited);

            panelInheritance.Visible = (settings.InheritedColumnsNode != null);

            LoadLayoutDetails();
            UpdateParentEditingUI();
        }

        /// <summary>
        /// Load the layout details into the GUI
        /// </summary>
        private void LoadLayoutDetails()
        {
            layoutEditor.LoadGridColumnLayout(settings.EffectiveLayout);
        }

        /// <summary>
        /// Update the UI based on if inherited is enabled
        /// </summary>
        private void UpdateParentEditingUI()
        {
            bool enableUI = !settings.Inherited || enableParentEditing;

            layoutEditor.Enabled = enableUI;

            if (!enableUI)
            {
                layoutEditor.ClearSelection();
            }
        }

        /// <summary>
        /// The choice on whether inheritance should be used has changed
        /// </summary>
        void OnChangeUseInherited(object sender, EventArgs e)
        {
            settings.Inherited = radioUseParent.Checked;
            LoadLayoutDetails();

            UpdateParentEditingUI();

            RaiseSettingsChanged();
        }

        /// <summary>
        /// The user has edited something about the grid column layout
        /// </summary>
        void OnGridColumnLayoutChanged(object sender, EventArgs e)
        {
            RaiseSettingsChanged();
        }

        /// <summary>
        /// Raise the SettingsChanged event
        /// </summary>
        private void RaiseSettingsChanged()
        {
            if (SettingsChanged != null)
            {
                SettingsChanged(this, EventArgs.Empty);
            }
        }
    }
}
