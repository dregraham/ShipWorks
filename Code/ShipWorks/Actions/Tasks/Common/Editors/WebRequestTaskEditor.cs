﻿using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks.Common.Enums;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Holds all information needed for the WebRequestTaskEditor
    /// </summary>
    public partial class WebRequestTaskEditor : TemplateBasedTaskEditor
    {
        readonly WebRequestTask task;

        ContextMenuStrip verbMenu;
        ContextMenuStrip authMenu;

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

            CreateVerbMenu();
            CreateAuthMenu();

            verbLabel.Text = EnumHelper.GetDescription(task.Verb);
            urlTextBox.Text = task.UrlToHit;
            authLabel.Text = task.UseBasicAuthentication ? "basic" : "no";
            userNameTextBox.Text = task.Username;
            passwordTextBox.Text = task.Password;

            oneRequestPerTemplateResult.Checked = task.RequestCardinality == WebRequestCardinality.OneRequestPerTemplateResult;
            oneRequestPerFilterResult.Checked = task.RequestCardinality == WebRequestCardinality.OneRequestPerFilterResult;
            singleRequest.Checked = task.RequestCardinality == WebRequestCardinality.SingleRequest;

            urlTextBox.Validating += OnUrlTextBoxValidating;
            urlTextBox.Validated += OnUrlTextBoxValidated;

            if (null != task.HttpHeaders)
            {
                headersGrid.Values = task.HttpHeaders.Select(x => new KeyValuePair<string, string>(HttpUtility.UrlDecode(x.Key), HttpUtility.UrlDecode(x.Value))).ToList();
            }

            UpdateAuthUI();
            UpdateBodyUI();
        }

        public override void NotifyTaskInputChanged(ActionTrigger trigger, EntityType? inputType)
        {
            UpdateBodyUI();
        }

        /// <summary>
        /// Called when [URL text box validated].
        /// </summary>
        void OnUrlTextBoxValidated(object sender, EventArgs e)
        {
            errorProvider.SetError(urlTextBox, string.Empty);
        }

        /// <summary>
        /// Called when [URL text box validating].
        /// </summary>
        void OnUrlTextBoxValidating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(urlTextBox.Text))
            {
                errorProvider.SetError(urlTextBox, "This value is required.");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Creates the verb menu.
        /// </summary>
        void CreateVerbMenu()
        {
            verbMenu = new ContextMenuStrip();

            foreach (HttpVerb verb in Enum.GetValues(typeof(HttpVerb)))
            {
                var menuItem = new ToolStripMenuItem(EnumHelper.GetDescription(verb));
                menuItem.Click += OnVerbMenuItemClick;
                menuItem.Tag = verb;

                verbMenu.Items.Add(menuItem);
            }
        }

        /// <summary>
        /// Creates the auth menu.
        /// </summary>
        void CreateAuthMenu()
        {
            authMenu = new ContextMenuStrip();

            var noMenuItem = new ToolStripMenuItem("no");
            noMenuItem.Click += OnAuthMenuItemClick;
            noMenuItem.Tag = false;
            authMenu.Items.Add(noMenuItem);

            var basicMenuItem = new ToolStripMenuItem("basic");
            basicMenuItem.Click += OnAuthMenuItemClick;
            basicMenuItem.Tag = true;
            authMenu.Items.Add(basicMenuItem);
        }

        /// <summary>
        /// Called when [click verb label].
        /// </summary>
        void OnVerbLabelClick(object sender, EventArgs e)
        {
            verbMenu.Show(verbLabel.Parent.PointToScreen(new Point(verbLabel.Left, verbLabel.Bottom)));
        }

        /// <summary>
        /// Called when [click verb menu item].
        /// </summary>
        void OnVerbMenuItemClick(object sender, EventArgs e)
        {
            var verbMenuItem = (ToolStripMenuItem) sender;
            var clickedVerb = (HttpVerb) verbMenuItem.Tag;

            if (task.Verb != clickedVerb)
            {
                task.Verb = clickedVerb;
                verbLabel.Text = verbMenuItem.Text;

                UpdateBodyUI();
            }
        }

        /// <summary>
        /// Called when [URL text changed].
        /// </summary>
        void OnUrlTextChanged(object sender, EventArgs e)
        {
            task.UrlToHit = urlTextBox.Text;
        }

        /// <summary>
        /// Called when [click auth label].
        /// </summary>
        void OnAuthLabelClick(object sender, EventArgs e)
        {
            authMenu.Show(authLabel.Parent.PointToScreen(new Point(authLabel.Left, authLabel.Bottom)));
        }

        /// <summary>
        /// Called when [click auth menu item].
        /// </summary>
        void OnAuthMenuItemClick(object sender, EventArgs e)
        {
            var authMenuItem = (ToolStripMenuItem) sender;
            var useBasicAuth = (bool) authMenuItem.Tag;

            if (task.UseBasicAuthentication != useBasicAuth)
            {
                task.UseBasicAuthentication = useBasicAuth;
                authLabel.Text = authMenuItem.Text;

                UpdateAuthUI();
            }
        }

        /// <summary>
        /// Called when [user name text changed].
        /// </summary>
        void OnUserNameTextChanged(object sender, EventArgs e)
        {
            task.Username = userNameTextBox.Text;
        }

        /// <summary>
        /// Called when [password text changed].
        /// </summary>
        void OnPasswordTextChanged(object sender, EventArgs e)
        {
            task.Password = passwordTextBox.Text;
        }

        /// <summary>
        /// Updates the auth UI.
        /// </summary>
        void UpdateAuthUI()
        {
            basicAuthPanel.Visible = task.UseBasicAuthentication;

            if (!basicAuthPanel.Visible)
            {
                userNameTextBox.Text = null;
                passwordTextBox.Text = null;
            }
        }

        /// <summary>
        /// Called when [headers grid data changed].
        /// </summary>
        void OnHeadersGridDataChanged(object sender, EventArgs e)
        {
            task.HttpHeaders = headersGrid.Values.Select(x => new KeyValuePair<string, string>(HttpUtility.UrlEncode(x.Key), HttpUtility.UrlEncode(x.Value))).ToArray();
        }

        /// <summary>
        /// Updates the body UI.
        /// </summary>
        void UpdateBodyUI()
        {
            var inputSource = (ActionTaskInputSource)task.Entity.InputSource;

            oneRequestPerFilterResult.Enabled =
                inputSource == ActionTaskInputSource.FilterContents;

            oneRequestPerTemplateResult.Enabled =
                inputSource != ActionTaskInputSource.Nothing &&
                task.Verb != HttpVerb.Get;

            if (oneRequestPerTemplateResult.Checked && !oneRequestPerTemplateResult.Enabled)
            {
                oneRequestPerFilterResult.Checked = true;
            }

            if (oneRequestPerFilterResult.Checked && !oneRequestPerFilterResult.Enabled)
            {
                singleRequest.Checked = true;
            }

            labelTemplate.Enabled =
            templateCombo.Enabled =
                oneRequestPerTemplateResult.Checked;

            if(!oneRequestPerTemplateResult.Checked)
            {
                templateCombo.SelectedTemplate = null;
            }
        }

        /// <summary>
        /// Called when a cardinality radio button checked state changes.
        /// </summary>
        void OnCardinalityCheckedChanged(object sender, EventArgs e)
        {
            if (oneRequestPerTemplateResult.Checked)
            {
                task.RequestCardinality = WebRequestCardinality.OneRequestPerTemplateResult;
            }
            else if (oneRequestPerFilterResult.Checked)
            {
                task.RequestCardinality = WebRequestCardinality.OneRequestPerFilterResult;
            }
            else
            {
                task.RequestCardinality = WebRequestCardinality.SingleRequest;
            }

            UpdateBodyUI();
        }
    }
}