using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Templates.Processing;
using ShipWorks.UI.Utility;
using ShipWorks.Data;
using ShipWorks.Templates;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Outlook
{
    /// <summary>
    /// Window for viewing sent email messages
    /// </summary>
    public partial class EmailSentViewerDlg : Form
    {
        long emailID;
        EmailOutboundEntity email;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSentViewerDlg(long emailID)
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);
            ThemedBorderProvider themeBorder = new ThemedBorderProvider(panelWithBorder);

            this.emailID = emailID;

            UserSession.Security.DemandPermission(PermissionType.RelatedObjectSendEmail, emailID);
        }

        /// <summary>
        /// Don't show the window if the email has been deleted
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            email = (EmailOutboundEntity) DataProvider.GetEntity(emailID);
            if (email == null)
            {
                MessageHelper.ShowInformation(this, "The selected message has been deleted.");
                DialogResult = DialogResult.Cancel;
            }

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            headerGroup.ValuesPrimary.Heading = email.Subject;
            headerGroup.ValuesPrimary.Description = "Sent: " + StringUtility.FormatFriendlyDateTime(email.SentDate);

            from.Text = email.FromAddress;
            to.Text = email.ToList;
            cc.Text = email.CcList;
            bcc.Text = email.BccList;
            subject.Text = email.Subject;
        }

        /// <summary>
        /// The window has been shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (email.HtmlPartResourceID != null)
            {
                // Make sure all our image resources are loaded
                DataResourceManager.LoadConsumerResourceReferences(emailID);

                // We need to get our base href added for displaying images
                TemplateHtmlDocument htmlDocument = new TemplateHtmlDocument(
                    DataResourceManager.LoadResourceReference(email.HtmlPartResourceID.Value).ReadAllText(), 
                    TemplateResultUsage.ShipWorksDisplay);

                htmlControl.Html = htmlDocument.CompleteHtml;
            }
            else
            {
                htmlControl.Html = TemplateResultFormatter.EncodeForHtml(
                    DataResourceManager.LoadResourceReference(email.PlainPartResourceID).ReadAllText(), 
                    TemplateOutputFormat.Text);
            }
        }
    }
}
