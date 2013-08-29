using Interapptive.Shared.Net;
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
        ContextMenuStrip cardinalityMenu;

        ToolStripMenuItem getMenuItem;
        ToolStripMenuItem postMenuItem;

        ToolStripMenuItem singleRequestMenuItem;
        ToolStripMenuItem oneRequestPerFilterResultMenuItem;
        ToolStripMenuItem oneRequestPerTemplateResultMenuItem;

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
            CreateCardinalityMenu();

            cardinalityLabel.Text = EnumHelper.GetDescription(task.RequestCardinality);
            verbLabel.Text = EnumHelper.GetDescription(task.Verb);
            urlTextBox.Text = task.UrlToHit;

            // We want to get the decrypted password method here instead of using the EncryptedPassword 
            // property to avoid to having it encrypted twice when the task is saved again.
            authLabel.Text = task.UseBasicAuthentication ? "basic" : "no";
            userNameTextBox.Text = task.Username;
            passwordTextBox.Text = task.GetDecryptedPassword();

            if (null != task.HttpHeaders)
            {
                headersGrid.Values = task.HttpHeaders.Select(x => new KeyValuePair<string, string>(HttpUtility.UrlDecode(x.Key), HttpUtility.UrlDecode(x.Value))).ToList();
            }

            UpdateAuthUI();
            UpdateBodyUI();
        }

        public override void NotifyTaskInputChanged(ActionTrigger trigger, EntityType? inputType)
        {
            var entityDescription = ActionTaskBubble.GetTriggeringEntityDescription(inputType) ?? "entry";

            oneRequestPerFilterResultMenuItem.Text =
                string.Format(EnumHelper.GetDescription(WebRequestCardinality.OneRequestPerFilterResult), entityDescription);

            if (task.RequestCardinality == WebRequestCardinality.OneRequestPerFilterResult)
            {
                cardinalityLabel.Text = oneRequestPerFilterResultMenuItem.Text;
            }

            UpdateBodyUI();
        }

        /// <summary>
        /// Creates the verb menu.
        /// </summary>
        void CreateVerbMenu()
        {
            verbMenu = new ContextMenuStrip();

            this.getMenuItem = new ToolStripMenuItem(EnumHelper.GetDescription(HttpVerb.Get));
            getMenuItem.Click += OnVerbMenuItemClick;
            getMenuItem.Tag = HttpVerb.Get;
            verbMenu.Items.Add(getMenuItem);

            this.postMenuItem = new ToolStripMenuItem(EnumHelper.GetDescription(HttpVerb.Post));
            postMenuItem.Click += OnVerbMenuItemClick;
            postMenuItem.Tag = HttpVerb.Post;
            verbMenu.Items.Add(postMenuItem);

            var putMenuItem = new ToolStripMenuItem(EnumHelper.GetDescription(HttpVerb.Put));
            putMenuItem.Click += OnVerbMenuItemClick;
            putMenuItem.Tag = HttpVerb.Put;
            verbMenu.Items.Add(putMenuItem);
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
        /// Creates the cardinality menu.
        /// </summary>
        void CreateCardinalityMenu()
        {
            cardinalityMenu = new ContextMenuStrip();

            this.singleRequestMenuItem = new ToolStripMenuItem(EnumHelper.GetDescription(WebRequestCardinality.SingleRequest));
            singleRequestMenuItem.Click += OnCardinalityMenuItemClick;
            singleRequestMenuItem.Tag = WebRequestCardinality.SingleRequest;
            cardinalityMenu.Items.Add(singleRequestMenuItem);

            this.oneRequestPerFilterResultMenuItem = new ToolStripMenuItem();
            oneRequestPerFilterResultMenuItem.Click += OnCardinalityMenuItemClick;
            oneRequestPerFilterResultMenuItem.Tag = WebRequestCardinality.OneRequestPerFilterResult;
            cardinalityMenu.Items.Add(oneRequestPerFilterResultMenuItem);

            this.oneRequestPerTemplateResultMenuItem = new ToolStripMenuItem(EnumHelper.GetDescription(WebRequestCardinality.OneRequestPerTemplateResult));
            oneRequestPerTemplateResultMenuItem.Click += OnCardinalityMenuItemClick;
            oneRequestPerTemplateResultMenuItem.Tag = WebRequestCardinality.OneRequestPerTemplateResult;
            cardinalityMenu.Items.Add(oneRequestPerTemplateResultMenuItem);
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
        /// Called when the auth label is clicked.
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
            task.SetPassword(passwordTextBox.Text);
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
            // Update cardinality menu items and selection
            var inputSource = (ActionTaskInputSource)task.Entity.InputSource;

            oneRequestPerFilterResultMenuItem.Available =
                inputSource == ActionTaskInputSource.FilterContents;
            oneRequestPerTemplateResultMenuItem.Available =
                inputSource != ActionTaskInputSource.Nothing;

            if ((task.RequestCardinality == WebRequestCardinality.OneRequestPerFilterResult && !oneRequestPerFilterResultMenuItem.Available) ||
                (task.RequestCardinality == WebRequestCardinality.OneRequestPerTemplateResult && !oneRequestPerTemplateResultMenuItem.Available))
            {
                singleRequestMenuItem.PerformClick();
                return;
            }

            if (task.RequestCardinality != WebRequestCardinality.OneRequestPerTemplateResult)
            {
                templateCombo.SelectedTemplate = null;
                requestPanel.Top = templateCombo.Top;
            }
            else
            {
                requestPanel.Top = templateCombo.Bottom + 8;
            }

            this.Height = requestPanel.Top + requestPanel.Height;

            labelTemplate.Visible =
            templateCombo.Visible =
                task.RequestCardinality == WebRequestCardinality.OneRequestPerTemplateResult;

            // Update verb menu items and selection
            getMenuItem.Available =
                task.RequestCardinality != WebRequestCardinality.OneRequestPerTemplateResult;

            if(task.Verb == HttpVerb.Get && !getMenuItem.Available)
            {
                postMenuItem.PerformClick();
            }
        }

        /// <summary>
        /// Manually sizes and aligns the controls affected by the verb text size.
        /// </summary>
        void OnVerbLabelSizeChanged(object sender, EventArgs e)
        {
            requestToLabel.Left = verbLabel.Right - 3;
            verbPanel.Width = requestToLabel.Right - 2;
        }

        /// <summary>
        /// Manually sizes and aligns the controls affected by the auth text size.
        /// </summary>
        void OnAuthLabelSizeChanged(object sender, EventArgs e)
        {
            authLabelSuffix.Left = authLabel.Right - 3;
            authTypePanel.Width = authLabelSuffix.Right - 3;
        }

        /// <summary>
        /// Shows the cardinality (a request per...) menu.
        /// </summary>
        void OnCardinalityLabelClick(object sender, EventArgs e)
        {
            cardinalityMenu.Show(cardinalityLabel.Parent.PointToScreen(new Point(cardinalityLabel.Left, cardinalityLabel.Bottom)));
        }

        /// <summary>
        /// Called when a cardinality menu item is clicked.
        /// </summary>
        void OnCardinalityMenuItemClick(object sender, EventArgs e)
        {
            var cardinalityMenuItem = (ToolStripMenuItem)sender;
            var clickedCardinality = (WebRequestCardinality)cardinalityMenuItem.Tag;

            if (task.RequestCardinality != clickedCardinality)
            {
                task.RequestCardinality = clickedCardinality;
                cardinalityLabel.Text = cardinalityMenuItem.Text;

                UpdateBodyUI();
            }
        }
    }
}
