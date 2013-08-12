using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class HitUrlTaskEditor : TemplateBasedTaskEditor
    {
        readonly HitUrlTask task;
        ContextMenuStrip verbMenu;
        ContextMenuStrip authMenu;

        public HitUrlTaskEditor(HitUrlTask task)
            : base(task)
        {
            this.task = task;

            InitializeComponent();
        }

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

            if(null != task.HttpHeaders)
                headersGrid.Values = task.HttpHeaders;

            UpdateAuthUI();
            UpdateBodyUI();
        }

        void CreateVerbMenu()
        {
            verbMenu = new ContextMenuStrip();

            foreach (HttpVerb verb in Enum.GetValues(typeof(HttpVerb)))
            {
                var menuItem = new ToolStripMenuItem(EnumHelper.GetDescription(verb));
                menuItem.Click += OnClickVerbMenuItem;
                menuItem.Tag = verb;

                verbMenu.Items.Add(menuItem);
            }
        }

        void CreateAuthMenu()
        {
            authMenu = new ContextMenuStrip();

            var noMenuItem = new ToolStripMenuItem("no");
            noMenuItem.Click += OnClickAuthMenuItem;
            noMenuItem.Tag = false;
            authMenu.Items.Add(noMenuItem);

            var basicMenuItem = new ToolStripMenuItem("basic");
            basicMenuItem.Click += OnClickAuthMenuItem;
            basicMenuItem.Tag = true;
            authMenu.Items.Add(basicMenuItem);
        }

        void OnClickVerbLabel(object sender, EventArgs e)
        {
            verbMenu.Show(verbLabel.Parent.PointToScreen(new Point(verbLabel.Left, verbLabel.Bottom)));
        }

        void OnClickVerbMenuItem(object sender, EventArgs e)
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

        void OnUrlTextChanged(object sender, EventArgs e)
        {
            task.UrlToHit = urlTextBox.Text;
        }

        void OnClickAuthLabel(object sender, EventArgs e)
        {
            authMenu.Show(authLabel.Parent.PointToScreen(new Point(authLabel.Left, authLabel.Bottom)));
        }

        void OnClickAuthMenuItem(object sender, EventArgs e)
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

        void OnUserNameTextChanged(object sender, EventArgs e)
        {
            task.Username = userNameTextBox.Text;
        }

        void OnPasswordTextChanged(object sender, EventArgs e)
        {
            task.Password = passwordTextBox.Text;
        }

        void UpdateAuthUI()
        {
            basicAuthPanel.Visible =
                task.UseBasicAuthentication;

            if (!basicAuthPanel.Visible)
            {
                userNameTextBox.Text = null;
                passwordTextBox.Text = null;
            }
        }

        void OnHeadersGridDataChanged(object sender, EventArgs e)
        {
            task.HttpHeaders = headersGrid.Values.ToArray();
        }

        void UpdateBodyUI()
        {
            labelTemplate.Visible =
            templateCombo.Visible =
            asBodyLabel.Visible =
                task.Verb != HttpVerb.Get;

            if (templateCombo.Visible)
            {
                this.Height = templateCombo.Bottom + 6;
            }
            else
            {
                templateCombo.SelectedTemplate = null;
                this.Height = templateCombo.Top - 3;
            }
        }
    }
}
