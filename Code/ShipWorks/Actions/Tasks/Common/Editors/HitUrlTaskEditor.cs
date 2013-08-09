using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class HitUrlTaskEditor : TemplateBasedTaskEditor
    {
        readonly HitUrlTask task;
        ContextMenuStrip verbMenu;

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

            verbLabel.Text = EnumHelper.GetDescription(task.Verb);
            urlTextBox.Text = task.UrlToHit;

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
                verbLabel.Text = EnumHelper.GetDescription(clickedVerb);
                UpdateBodyUI();
            }
        }

        void OnUrlTextChanged(object sender, EventArgs e)
        {
            task.UrlToHit = urlTextBox.Text;
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
                this.Height = templateCombo.Top - 6;
            }
        }
    }
}
