using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Templates.Controls.DefaultPickListTemplate
{
    /// <summary>
    /// Service for printing pick lists
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IPickListPrintingService))]
    public class PickListPrintingService : IPickListPrintingService
    {
        private readonly IMainForm mainForm;
        private readonly IDefaultPickListTemplateDialog pickListTemplateDialog;
        private readonly ITemplateManager templateManager;
        private readonly IMessenger messenger;

        public PickListPrintingService(IMainForm mainForm, IDefaultPickListTemplateDialog pickListTemplateDialog, ITemplateManager templateManager, IMessenger messenger)
        {
            this.mainForm = mainForm;
            this.pickListTemplateDialog = pickListTemplateDialog;
            this.templateManager = templateManager;
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline with the main grid control
        /// </summary>
        public IDisposable Register(IMainGridControl mainGridControl)
        {
            return messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.PrintPickList))
                .Subscribe(message => PrintPickList(mainGridControl.Selection.OrderedKeys));
        }

        public void PrintPickList(IEnumerable<long> selectedOrderIDs)
        {
            // Ensure orders are selected (they should be for this button to be clicked, but just in case)
            if (selectedOrderIDs.None())
            {
                return;
            }

            // get default pick list template
            TemplateEntity pickListTemplate = templateManager.FetchDefaultPickListTemplate();


            // if no default, prompt user to select one
            if (pickListTemplate == null)
            {
                if (pickListTemplateDialog.ShowDialog() == true)
                {
                    pickListTemplate = templateManager.FetchDefaultPickListTemplate();
                }
            }

            // print default template if it exists
            if (pickListTemplate != null)
            {
                if (!templateManager.EnsureTemplateConfigured(mainForm, pickListTemplate))
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                // Create the print job using the default settings from the template
                PrintJob job = PrintJob.Create(pickListTemplate, selectedOrderIDs);

                // Start the printing of the job
                mainForm.StartPrintJob(job, PrintAction.Print);
            }
        }
    }
}
