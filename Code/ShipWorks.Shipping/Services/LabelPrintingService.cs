using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Templates;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service that handles printing labels
    /// </summary>
    public class LabelPrintingService : IInitializeForCurrentSession
    {
        private readonly IMessageHelper messageHelper;
        private readonly IObservable<IShipWorksMessage> messages;
        private readonly Control owner;
        private readonly ILog log;
        IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPrintingService(IObservable<IShipWorksMessage> messages, IMessageHelper messageHelper,
            Func<Type, ILog> logManager, Control owner)
        {
            this.messages = messages;
            this.messageHelper = messageHelper;
            this.log = logManager(typeof(LabelPrintingService));
            this.owner = owner;
        }

        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription = messages.OfType<ReprintLabelsMessage>()
                .CatchAndContinue((Exception ex) => log.Error("Unable to print the requested labels", ex))
                .Subscribe(async x => await ReprintLabel(x).ConfigureAwait(false));
        }

        /// <summary>
        /// Reprint labels for the shipments in the message
        /// </summary>
        private async Task ReprintLabel(ReprintLabelsMessage message)
        {
            Control currentOwner = message.Sender as Control ?? owner;

            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(currentOwner,
                "Print Shipments",
                "ShipWorks is printing labels.",
                "Printing {0} of {1}");

            // We are prepared for exceptions
            executor.PropagateException = true;

            // What to do before it gets started (but is on the background thread)
            executor.ExecuteStarting += (object s, EventArgs args) =>
                FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            // Some of the printing will be delayed b\c we are waiting for label sheets to fill up
            Dictionary<TemplateEntity, List<long>> delayedPrints = new Dictionary<TemplateEntity, List<long>>(TemplateHelper.TemplateEqualityComparer);

            // Executes right after things finish - but still on the background thread
            executor.ExecuteCompleting += (object s, EventArgs args) =>
            {
                foreach (PrintJob printJob in delayedPrints.Select(pair => PrintJob.Create(pair.Key, pair.Value)))
                {
                    printJob.Print();
                }
            };

            // Each shipment to print.  Send in a cloned collection so changes on other threads don't affect it.
            IEnumerable<ShipmentEntity> shipmentsToPrint = EntityUtility.CloneEntityCollection(message.Shipments);

            BackgroundExecutorCompletedEventArgs<ShipmentEntity> result =
                await executor.ExecuteAsync(CreatePrintingWorker(delayedPrints), shipmentsToPrint, null);

            if (result.ErrorException == null)
            {
                return;
            }

            if (result.ErrorException is PrintingException)
            {
                if (!(result.ErrorException is PrintingNoTemplateOutputException))
                {
                    MessageHelper.ShowError(currentOwner, result.ErrorException.Message);
                }
            }
            else
            {
                throw new InvalidOperationException(result.ErrorException.Message, result.ErrorException);
            }
        }

        /// <summary>
        /// Create a worker for printing a shipment
        /// </summary>
        private static BackgroundExecutorCallback<ShipmentEntity> CreatePrintingWorker(Dictionary<TemplateEntity, List<long>> delayedPrints)
        {
            return (shipment, state, issueAdder) =>
            {
                List<TemplateEntity> templates = ShipmentPrintHelper.DetermineTemplatesToPrint(shipment);

                // Print with each template
                foreach (TemplateEntity template in templates)
                {
                    PrintTemplate(template, shipment, delayedPrints);
                };
            };
        }

        /// <summary>
        /// Print a given template
        /// </summary>
        private static void PrintTemplate(TemplateEntity template, ShipmentEntity shipment,
            Dictionary<TemplateEntity, List<long>> delayedPrints)
        {
            // If it's standard or thermal we can print it right away
            if (template.Type == (int) TemplateType.Standard || template.Type == (int) TemplateType.Thermal)
            {
                PrintJob printJob = PrintJob.Create(template, new List<long> { shipment.ShipmentID });
                printJob.Print();
            }
            else
            {
                // Get the list of keys that have been delayed so far for this template
                List<long> delayedKeys;
                if (!delayedPrints.TryGetValue(template, out delayedKeys))
                {
                    delayedKeys = new List<long>();
                    delayedPrints[template] = delayedKeys;
                }

                // Add this as a delayed key
                delayedKeys.Add(shipment.ShipmentID);

                PrintLabelTemplate(template, delayedKeys, delayedPrints);
            }
        }

        /// <summary>
        /// Print a label template
        /// </summary>
        private static void PrintLabelTemplate(TemplateEntity template, List<long> delayedKeys,
            IDictionary<TemplateEntity, List<long>> delayedPrints)
        {
            // It must be a label template
            if (template.Type != (int) TemplateType.Label)
            {
                return;
            }

            LabelSheetEntity labelSheet = LabelSheetManager.GetLabelSheet(template.LabelSheetID);

            if (labelSheet != null)
            {
                int cells = labelSheet.Rows * labelSheet.Columns;

                // To know how many cells we'll use, we have to translate
                int inputs = TemplateContextTranslator.Translate(delayedKeys, template).Count;

                // If we have enough to fill a sheet, print now
                if (inputs % cells == 0)
                {
                    PrintJob printJob = PrintJob.Create(template, delayedKeys);
                    printJob.Print();

                    delayedPrints.Remove(template);
                }
            }
            else
            {
                delayedPrints.Remove(template);
            }
        }
    }
}
