using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks.Common.Enums;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Holds all information needed for the WebRequestTaskEditor
    /// </summary>
    public partial class WebRequestTaskEditor : TemplateBasedTaskEditor
    {
        private readonly WebRequestTask task;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestTaskEditor"/> class.
        /// </summary>
        public WebRequestTaskEditor(WebRequestTask task)
            : base(task)
        {
            this.task = task;

            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UpdateCardinalityUI(null);
            UpdateVerbUI();
            UpdateTemplateUI();
            UpdateAdditionalOptionsUI();

            comboCardinality.SelectedValue = task.RequestCardinality;
            comboVerb.SelectedValue = task.Verb;
            urlTextBox.Text = task.Url;

            // We want to get the decrypted password method here instead of using the EncryptedPassword 
            // property to avoid to having it encrypted twice when the task is saved again.
            userNameTextBox.Text = task.Username;
            passwordTextBox.Text = task.GetDecryptedPassword();

            if (null != task.HttpHeaders)
            {
                headersGrid.Values = task.HttpHeaders.Select(x => new KeyValuePair<string, string>(HttpUtility.UrlDecode(x.Key), HttpUtility.UrlDecode(x.Value))).ToList();
            }

            useBasicAuth.Checked = task.UseBasicAuthentication;
            includeExtraHeaders.Checked = task.IncludeCustomHttpHeaders;
        }

        /// <summary>
        /// Allows derived editors to update themselves based on the current trigger
        /// </summary>
        public override void NotifyTaskInputChanged(ActionTrigger trigger, ActionTaskInputSource inputSource, EntityType? inputType)
        {
            UpdateCardinalityUI(inputType);
        }

        /// <summary>
        /// Load the cardinality selection box
        /// </summary>
        private void UpdateCardinalityUI(EntityType? inputType)
        {
            ActionTaskInputSource inputSource = (ActionTaskInputSource) task.Entity.InputSource;

            comboCardinality.SelectedValueChanged -= OnChangeCardinality;

            object selected = comboCardinality.SelectedValue;

            comboCardinality.DataSource = null;
            comboCardinality.DisplayMember = "Key";
            comboCardinality.ValueMember = "Value";

            comboCardinality.DataSource = EnumHelper.GetEnumList<WebRequestCardinality>
                (
                    e =>
                    {
                        switch (e)
                        {
                            case WebRequestCardinality.OneRequestPerFilterResult:
                                return inputSource == ActionTaskInputSource.FilterContents;

                            case WebRequestCardinality.OneRequestPerTemplateResult:
                                return inputSource != ActionTaskInputSource.Nothing;

                            default:
                                return true;
                        }
                    }
                )
                .Select(e =>
                    new KeyValuePair<string, WebRequestCardinality>
                        (
                            string.Format(e.Description, ActionTaskBubble.GetTriggeringEntityDescription(inputType) ?? "entry"),
                            e.Value)
                        ).ToList();

            if (selected != null)
            {
                comboCardinality.SelectedValue = selected;
            }
            
            comboCardinality.SelectedValueChanged += OnChangeCardinality;

            if (comboCardinality.SelectedIndex < 0)
            {
                comboCardinality.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Changing the cardinality
        /// </summary>
        private void OnChangeCardinality(object sender, EventArgs e)
        {
            task.RequestCardinality = (WebRequestCardinality) comboCardinality.SelectedValue;

            UpdateTemplateUI();
            UpdateVerbUI();
        }

        /// <summary>
        /// Update the verb combo box
        /// </summary>
        private void UpdateVerbUI()
        {
            comboVerb.SelectedValueChanged -= OnChangeVerb;

            object selected = comboVerb.SelectedValue;

            comboVerb.DataSource = null;
            comboVerb.DisplayMember = "Key";
            comboVerb.ValueMember = "Value";

            comboVerb.DataSource = EnumHelper.GetEnumList<HttpVerb>
                (
                    v =>
                    {
                        switch (v)
                        {
                            case HttpVerb.Get:
                                return task.RequestCardinality != WebRequestCardinality.OneRequestPerTemplateResult;

                            default:
                                return true;
                        }
                    }
                )
                .Select(e =>
                    new KeyValuePair<string, HttpVerb>
                        (
                            e.Description,
                            e.Value)
                        ).ToList();

            if (selected != null)
            {
                comboVerb.SelectedValue = selected;
            }

            comboVerb.SelectedValueChanged += OnChangeVerb;

            if (comboVerb.SelectedIndex < 0)
            {
                comboVerb.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Update the UI for additional options
        /// </summary>
        private void UpdateAdditionalOptionsUI()
        {
            panelBasicAuth.Visible = useBasicAuth.Checked;
            includeExtraHeaders.Top = (useBasicAuth.Checked) ? panelBasicAuth.Bottom + 4 : useBasicAuth.Bottom + 4;
            headersGrid.Top = includeExtraHeaders.Bottom + 4;

            headersGrid.Visible = includeExtraHeaders.Checked;
            requestPanel.Height = (includeExtraHeaders.Checked) ? headersGrid.Bottom + 4 : includeExtraHeaders.Bottom + 4;

            this.Height = requestPanel.Bottom;
        }

        /// <summary>
        /// Updates the template display UI
        /// </summary>
        private void UpdateTemplateUI()
        {
            if (task.RequestCardinality != WebRequestCardinality.OneRequestPerTemplateResult)
            {
                templateCombo.SelectedTemplate = null;
                requestPanel.Top = templateCombo.Top;

                labelTemplate.Visible = false;
                templateCombo.Visible = false;
            }
            else
            {
                requestPanel.Top = templateCombo.Bottom + 8;

                labelTemplate.Visible = true;
                templateCombo.Visible = true;
            }

            this.Height = requestPanel.Bottom;
        }

        /// <summary>
        /// The verb to use has changed
        /// </summary>
        private void OnChangeVerb(object sender, EventArgs e)
        {
            task.Verb = (HttpVerb) comboVerb.SelectedValue;
        }

        /// <summary>
        /// Called when [URL text changed].
        /// </summary>
        private void OnUrlTextChanged(object sender, EventArgs e)
        {
            task.Url = urlTextBox.Text;
        }

        /// <summary>
        /// Called when [user name text changed].
        /// </summary>
        private void OnUserNameTextChanged(object sender, EventArgs e)
        {
            task.Username = userNameTextBox.Text;
        }

        /// <summary>
        /// Called when [password text changed].
        /// </summary>
        private void OnPasswordTextChanged(object sender, EventArgs e)
        {
            task.SetPassword(passwordTextBox.Text);
        }

        /// <summary>
        /// Changing whether or not to use basic auth
        /// </summary>
        private void OnChangeUseBasicAuth(object sender, EventArgs e)
        {
            task.UseBasicAuthentication = useBasicAuth.Checked;

            UpdateAdditionalOptionsUI();
        }

        /// <summary>
        /// Changing whether or not to include extra headers
        /// </summary>
        private void OnChangeIncludeHttpHeaders(object sender, EventArgs e)
        {
            task.IncludeCustomHttpHeaders = includeExtraHeaders.Checked;

            UpdateAdditionalOptionsUI();
        }

        /// <summary>
        /// Called when [headers grid data changed].
        /// </summary>
        private void OnHeadersGridDataChanged(object sender, EventArgs e)
        {
            task.HttpHeaders = headersGrid.Values.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray();
        }

        /// <summary>
        /// Performs validation outside of the Windows Forms flow to make dealing with navigation easier
        /// </summary>
        /// <param name="errors">Collection of errors to which new errors will be added</param>
        public override void ValidateTask(ICollection<TaskValidationError> errors)
        {
            ActionTaskDescriptor descriptor = new ActionTaskDescriptor(task.GetType());
            TaskValidationError error = new TaskValidationError(string.Format("The '{0}' task is missing some information.", descriptor.BaseName));

            if (string.IsNullOrWhiteSpace(urlTextBox.Text))
            {
                error.Details.Add("Please enter a request url.");
            }

            // Add the error to the main errors collection if there are any validation errors
            if (error.Details.Any())
            {
                errors.Add(error);
            }
        }
    }
}