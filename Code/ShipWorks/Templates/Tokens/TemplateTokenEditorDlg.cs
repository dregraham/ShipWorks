using Interapptive.Shared.UI;
using ShipWorks.Templates.Processing;
using System;
using System.Windows.Forms;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Window for editing a token in a bigger view
    /// </summary>
    public partial class TemplateTokenEditorDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateTokenEditorDlg()
        {
            InitializeComponent();

            xslEditor.HeaderXsl = TemplateTokenProcessor.TokenXslHeader;
            xslEditor.FooterXsl = TemplateTokenProcessor.TokenXslFooter;

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// The token text value.  This can include the { } shortcuts.
        /// </summary>
        public string TokenText
        {
            get
            {
                return xslEditor.TemplateXsl;
            }
            set
            {
                xslEditor.TemplateXsl = value;
            }
        }

        /// <summary>
        /// User OK
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            TemplateXsl templateXsl = TemplateXslProvider.FromToken(TokenText);

            if (!templateXsl.IsValid)
            {
                MessageHelper.ShowError(this, "There are errors in the token.\n\n" + templateXsl.CompileException.Message);
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
