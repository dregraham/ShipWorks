using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// The master control for choosing the file source and it's settings
    /// </summary>
    public partial class GenericFileSourceMasterControl : UserControl
    {
        bool showChooseOption = false;

        // The current type displayed in the UI, if any
        GenericFileSourceType currentType = null;

        // So if they go back in forth in their selection we reused the same UI, and don't have to somehow persist settings between changes
        Dictionary<GenericFileSourceTypeCode, GenericFileSourceSettingsControlBase> settingsControlMap = new Dictionary<GenericFileSourceTypeCode, GenericFileSourceSettingsControlBase>();

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileSourceMasterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Controls if "Choose..." is the first option in the list, or if only the valid file sources are shown
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ShowChooseOption
        {
            get { return showChooseOption; }
            set { showChooseOption = value; }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize(GenericFileStoreEntity store, bool newStore)
        {
            fileSource.DisplayMember = "Display";
            fileSource.ValueMember = "Value";

            ArrayList list = new ArrayList();

            if (showChooseOption)
            {
                list.Add(new { Display = "Choose...", Value = (GenericFileSourceTypeCode?) null });
            }

            list.AddRange(
                GenericFileSourceTypeManager.GetFileSources(store, newStore).Select(source =>
                    new { Display = source.Description, Value = (GenericFileSourceTypeCode?) source.FileSourceTypeCode }).ToList());

            fileSource.DataSource = list;
        }

        /// <summary>
        /// Load the data from the given store
        /// </summary>
        public void LoadStore(GenericFileStoreEntity store, bool newStore)
        {
            // Lazy initialize
            if (fileSource.DataSource == null)
            {
                Initialize(store, newStore);
            }

            // Ensure all possible settings controls are created and loaded
            GenericFileSourceTypeManager.GetFileSources(store, newStore).ForEach(sourcType => GetSettingsControl(sourcType).LoadStore(store));

            if (store.FileSource == -1 && showChooseOption)
            {
                fileSource.SelectedIndex = 0;
            }
            else
            {
                GenericFileSourceTypeCode typeCode = (GenericFileSourceTypeCode) (store.FileSource == -1 ? 0 : store.FileSource);

                // This will get the UI selected and loaded loaded
                fileSource.SelectedValue = typeCode;
            }
        }

        /// <summary>
        /// Save the data to the in memory copy of the given store
        /// </summary>
        public bool SaveToEntity(GenericFileStoreEntity store)
        {
            if (currentType == null)
            {
                MessageHelper.ShowInformation(this, "Please choose an option for data importing.");
                return false;
            }

            // Save the type
            store.FileSource = (int) currentType.FileSourceTypeCode;

            // Save all the settings
            return GetSettingsControl(currentType).SaveToEntity(store);
        }

        /// <summary>
        /// Changing the file source
        /// </summary>
        private void OnChangeFileSource(object sender, EventArgs e)
        {
            GenericFileSourceType newType = null;

            // Get what's now selected, if any
            if (fileSource.SelectedIndex >= 0 && fileSource.SelectedValue != null)
            {
                newType = GenericFileSourceTypeManager.GetFileSourceType((GenericFileSourceTypeCode) fileSource.SelectedValue);
            }

            // Both null, nothing to do
            if (newType == null && currentType == null)
            {
                return;
            }

            // Hasn't changed, nothing to do
            if (newType != null && currentType != null &&
                newType.GetType() == currentType.GetType())
            {
                return;
            }

            // Get rid of the previous one
            if (currentType != null)
            {
                panelHolder.Controls[0].SizeChanged -= new EventHandler(OnSettingsSizeChanged);
                panelHolder.Controls.RemoveAt(0);
            }

            // Set the new one
            if (newType != null)
            {
                Control settingsControl = GetSettingsControl(newType);

                panelHolder.Controls.Add(settingsControl);
                panelHolder.Controls[0].SizeChanged += new EventHandler(OnSettingsSizeChanged);
            }

            UpdateHeight();

            // This one is now current
            currentType = newType;
        }

        /// <summary>
        /// Update the height of the panel holder and ourself
        /// </summary>
        private void UpdateHeight()
        {
            panelHolder.Height = (panelHolder.Controls.Count == 0) ? 10 : panelHolder.Controls[0].Height;
            this.Height = panelHolder.Bottom;
        }

        /// <summary>
        /// The size of the contained settings control has changed
        /// </summary>
        void OnSettingsSizeChanged(object sender, EventArgs e)
        {
            UpdateHeight();
        }

        /// <summary>
        /// Create the settings control using the given file source type
        /// </summary>
        private GenericFileSourceSettingsControlBase GetSettingsControl(GenericFileSourceType sourceType)
        {
            GenericFileSourceSettingsControlBase control;

            if (!settingsControlMap.TryGetValue(sourceType.FileSourceTypeCode, out control))
            {
                control = sourceType.CreateSettingsControl();
                control.Location = new Point(0, 0);

                settingsControlMap[sourceType.FileSourceTypeCode] = control;
            }

            return control;
        }
    }
}
