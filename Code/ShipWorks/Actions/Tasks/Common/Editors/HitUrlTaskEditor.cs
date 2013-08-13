using System.Collections.Generic;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Web;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Holds all information needed for the HitUrlTaskEditor
    /// </summary>
    public partial class HitUrlTaskEditor : TemplateBasedTaskEditor
    {
        readonly HitUrlTask task;
        ContextMenuStrip verbMenu;
        ContextMenuStrip authMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="HitUrlTaskEditor"/> class.
        /// </summary>
        public HitUrlTaskEditor(HitUrlTask task)
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

            if (null != task.HttpHeaders)
                headersGrid.Values = task.HttpHeaders.Select(x => new KeyValuePair<string, string>(HttpUtility.UrlDecode(x.Key), HttpUtility.UrlDecode(x.Value))).ToList();

            UpdateAuthUI();
            UpdateBodyUI();
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
            var verbMenuItem = (ToolStripMenuItem)sender;
            var clickedVerb = (HttpVerb)verbMenuItem.Tag;

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
            var authMenuItem = (ToolStripMenuItem)sender;
            var useBasicAuth = (bool)authMenuItem.Tag;

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
            bool isNotGet = task.Verb != HttpVerb.Get;

            labelTemplate.Visible = isNotGet;
            templateCombo.Visible = isNotGet;
            asBodyLabel.Visible = isNotGet;
            
            if (templateCombo.Visible)
            {
                Height = templateCombo.Bottom + 6;
            }
            else
            {
                templateCombo.SelectedTemplate = null;
                Height = templateCombo.Top - 3;
            }
        }
    }
}
