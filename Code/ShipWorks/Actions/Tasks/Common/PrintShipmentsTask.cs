using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Shipping;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Printing;
using log4net;
using System.Diagnostics;
using ShipWorks.Templates.Processing;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ActionTask that prints shipping labels using the print output configuration of the shipment type
    /// </summary>
    [ActionTask("Print shipments", "PrintShipments")]
    public class PrintShipmentsTask : ActionTask
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PrintShipmentsTask));

        class PrintQueueEntry
        {
            public TemplateEntity Template { get; set; }
            public List<object> InputList { get; set; }

            public override string ToString()
            {
                return string.Format("{0} ({1} inputs)", Template.FullName, InputList.Count);
            }
        }

        /// <summary>
        /// Create the editor used for editing the task settings
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new PrintShipmentsTaskEditor(this);
        }

        /// <summary>
        /// How to label the input selection for the task
        /// </summary>
        public override string InputLabel
        {
            get { return "Print labels for:"; }
        }

        /// <summary>
        /// This task only operates on shipments
        /// </summary>
        public override EntityType? InputEntityType
        {
            get { return EntityType.ShipmentEntity; }
        }

        /// <summary>
        /// This task reads the current filter contents to evaluate printing rules
        /// </summary>
        public override bool ReadsFilterContents
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Run the task for the given set of input keys
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            try
            {
                // Get the current queue
                var currentQueue = CreatePrintQueue(inputKeys);

                // Get any postponed queues we've previously stored away
                var queueList = context.GetPostponedData().Select(d => (List<PrintQueueEntry>) d).ToList();

                // Add the current queue to the list
                queueList.Add(currentQueue);

                // Combine all the queues into a single queue
                List<PrintQueueEntry> combinedQueue = CombineQueues(queueList);

                // Assume we can print until we find out otherwise
                bool readyToPrint = true;

                // Now determine if each template in the queue is ready to print
                foreach (var entry in combinedQueue)
                {
                    TemplateEntity template = entry.Template;
                    List<object> inputList = entry.InputList;

                    // Reports just keep queing until we hit the threshold
                    if (template.Type == (int) TemplateType.Report)
                    {
                        if (inputList.Count < TemplateBasedTask.MaxReportPostpone)
                        {
                            readyToPrint = false;
                        }
                    }

                    // Labels have to wait until a sheet can be full
                    if (template.Type == (int) TemplateType.Label)
                    {
                        LabelSheetEntity sheet = context.GetLabelSheet(template.LabelSheetID);
                        int cells = sheet.Rows * sheet.Columns;

                        // If it doesn't fit perfectly then we are not ready to print
                        if (inputList.Count % cells > 0)
                        {
                            readyToPrint = false;
                        }
                    }
                }

                // If we are allowed to postpone and not ready to print, then postpone
                if (context.CanPostpone && !readyToPrint)
                {
                    context.Postpone(currentQueue);
                }
                // Otherwise print now
                else
                {
                    context.ConsumingPostponed();

                    // Process
                    PrintNow(combinedQueue);
                }

            }
            catch (TemplateException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Create the queue of stuff to print organized by template
        /// </summary>
        private List<PrintQueueEntry> CreatePrintQueue(List<long> inputKeys)
        {
            log.DebugFormat("CreatingPrintQueue for {0} keys", inputKeys.Count());

            List<PrintQueueEntry> printQueue = new List<PrintQueueEntry>();

            // Build the list of stuff to print for each shipment
            foreach (long shipmentID in inputKeys)
            {
                ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);

                // Deleted. I guess just skip it. What else would make sense?
                if (shipment == null)
                {
                    continue;
                }

                List<TemplateEntity> templates = ShipmentPrintHelper.DetermineTemplatesToPrint(shipment);

                // Print with each template
                foreach (TemplateEntity template in templates)
                {
                    // Get the list of keys\results that have been found so far for this template
                    PrintQueueEntry entry = printQueue.SingleOrDefault(q => q.Template.TemplateID == template.TemplateID);
                    if (entry == null)
                    {
                        entry = new PrintQueueEntry { Template = template, InputList = new List<object>() };
                        printQueue.Add(entry);
                    }

                    // If its a report we just save the input key, since all inputs need to be processed at once together at the end
                    if (template.Type == (int) TemplateType.Report)
                    {
                        entry.InputList.Add(shipment.ShipmentID);
                    }

                    // Otherwise we go ahead and process and get the results now
                    else
                    {
                        entry.InputList.AddRange(
                            TemplateProcessor.ProcessTemplate(
                                template, 
                                new List<long> { shipment.ShipmentID })
                                .Select(r => (object) r));
                    }
                }
            }

            return printQueue;
        }

        /// <summary>
        /// Combine the given list of queues into a single queue
        /// </summary>
        private List<PrintQueueEntry> CombineQueues(List<List<PrintQueueEntry>> queueList)
        {
            List<PrintQueueEntry> combinedQueue = new List<PrintQueueEntry>();

            // Go thread each queue and merge it into the combined queue
            foreach (var queue in queueList)
            {
                foreach (var entry in queue)
                {
                    PrintQueueEntry combinedEntry = combinedQueue.SingleOrDefault(q => q.Template.TemplateID == entry.Template.TemplateID);
                    if (combinedEntry == null)
                    {
                        combinedEntry = new PrintQueueEntry { Template = entry.Template, InputList = new List<object>() };
                        combinedQueue.Add(combinedEntry);
                    }

                    combinedEntry.InputList.AddRange(entry.InputList);
                }
            }

            return combinedQueue;
        }

        /// <summary>
        /// Print everything in the print queue
        /// </summary>
        private void PrintNow(List<PrintQueueEntry> printQueue)
        {
            log.DebugFormat("Printing {0} templates from the queue", printQueue.Count);

            try
            {
                foreach (PrintQueueEntry entry in printQueue)
                {
                    TemplateEntity template = entry.Template;
                    List<object> inputList = entry.InputList;

                    PrintJob job;
                    
                    // For reports the input list is the raw keys
                    if (template.Type == (int) TemplateType.Report)
                    {
                        job = PrintJob.Create(template, inputList.Select(o => (long) o).ToList());
                    }
                    // Otherwise it's already processed results
                    else
                    {
                        job = PrintJob.Create(template, inputList.Select(o => (TemplateResult) o).ToList());
                    }

                    job.Print();
                }
            }
            catch (PrintingNoTemplateOutputException)
            {
                log.Info("PrintTask skipped due to no template output.");
            }
            catch (PrintingException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
