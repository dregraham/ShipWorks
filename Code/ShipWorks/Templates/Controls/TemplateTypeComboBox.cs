using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Controls;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// ComboBox for selecting TemplateType
    /// </summary>
    public partial class TemplateTypeComboBox : ImageComboBox, ISupportInitialize
    {
        bool loading = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateTypeComboBox()
        {
            InitializeComponent();

            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Load the template types into the combo box
        /// </summary>
        private void LoadTemplateTypes()
        {
            loading = true;

            try
            {
                DisplayMember = "Text";
                ValueMember = "Value";

                DataSource = new ImageComboBoxItem[]
                {
                    CreateTemplateTypeComboItem(TemplateType.Standard),
                    CreateTemplateTypeComboItem(TemplateType.Label),
                    CreateTemplateTypeComboItem(TemplateType.Report),
                    CreateTemplateTypeComboItem(TemplateType.Thermal)
                };

                SelectedIndex = -1;
            }
            finally
            {
                loading = false;
            }
        }

        /// <summary>
        /// Create the combobox item for the specified templatetype
        /// </summary>
        private ImageComboBoxItem CreateTemplateTypeComboItem(TemplateType templateType)
        {
            return new ImageComboBoxItem(
                EnumHelper.GetDescription(templateType),
                templateType,
                TemplateHelper.GetTemplateImage(templateType));
        }

        /// <summary>
        /// Raises the SelectedIndexChanged event
        /// </summary>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (loading)
            {
                return;
            }

            base.OnSelectedIndexChanged(e);
        }

        #region ISupportInitialize Members

        public void BeginInit()
        {

        }

        public void EndInit()
        {
            if (!DesignModeDetector.IsDesignerHosted() && DataSource == null)
            {
                LoadTemplateTypes();
            }
        }

        #endregion
    }
}
