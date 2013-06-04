using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace ShipWorks.UI.Wizard.Design
{
    /// <summary>
    /// Designer for the WizardForm control
    /// </summary>
    public class WizardFormDesigner : DocumentDesigner
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WizardFormDesigner()
        {

        }

        /// <summary>
        /// The WizardForm that is being designed.
        /// </summary>
        public WizardForm Wizard
        {
            get { return (WizardForm) Control; }
        }

        /// <summary>
        /// Get the Designer verbs
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs = new DesignerVerbCollection();

                verbs.Add(new DesignerVerb("New", new EventHandler(OnAddPage)));
                verbs.Add(new DesignerVerb("Back", new EventHandler(OnBack)));
                verbs.Add(new DesignerVerb("Next", new EventHandler(OnNext)));

                foreach (WizardPage page in Wizard.Pages)
                {
                    verbs.Add(new DesignerVerb(page.Site.Name, new EventHandler(OnGoToPage)));
                }

                return verbs;
            }
        }

        /// <summary>
        /// Go to a specific wizard page
        /// </summary>
        private void OnGoToPage(object sender, EventArgs e)
        {
            DesignerVerb verb = (DesignerVerb) sender;

            foreach (WizardPage page in Wizard.Pages)
            {
                if (page.Site.Name == verb.Text)
                {
                    Wizard.SetCurrent(Wizard.Pages.IndexOf(page));
                    return;
                }
            }
        }

        /// <summary>
        /// Go back a page
        /// </summary>
        private void OnBack(object sender, EventArgs e)
        {
            Wizard.MoveBack();
        }

        /// <summary>
        /// Go on to the next page
        /// </summary>
        private void OnNext(object sender, EventArgs e)
        {
            Wizard.MoveNext();
        }

        /// <summary>
        /// Adds a page to the wizard
        /// </summary>
        private void OnAddPage(object sender, EventArgs e)
        {
            WizardPage page = new WizardPage();
            int index = Wizard.Pages.Add(page);

            page.Title = string.Format("Wizard page {0}.", index + 1);
            page.Description = "The description of the page.";

            Wizard.SetCurrent(index);

            IDesignerHost host = (IDesignerHost) GetService(typeof(IDesignerHost));
            if (host != null)
            {
                host.Container.Add(page);
            }
        }

        /// <summary>
        /// Removes a page from the wizard
        /// </summary>
        private void OnRemovePage(object sender, EventArgs e)
        {
            if (Wizard.CurrentPage != null)
            {
                Wizard.Pages.Remove(Wizard.CurrentPage);
            }
        }
    }
}
