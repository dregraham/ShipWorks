using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using System.Xml.XPath;
using ShipWorks.Data;
using log4net;
using System.Data.SqlClient;
using ShipWorks.Templates;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Media;
using System.Diagnostics;
using ShipWorks.Templates.Processing;
using System.Collections;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for printing with a chosen template
    /// </summary>
    [ActionTask("Print", "Print", ActionTaskCategory.Output)]
    public class PrintTask : TemplateBasedTask, IPrintWithTemplates
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PrintTask));

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Print using:";
            }
        }

        /// <summary>
        /// Return all the template id's that the user has chosen to be used as templaets to print with
        /// </summary>
        IEnumerable<long> IPrintWithTemplates.TemplatesToPrintWith
        {
            get { return new long[] { TemplateID }; }
        }

        /// <summary>
        /// Do the print for the given template and input
        /// </summary>
        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            // Create the print job using the default settings from the template
            PrintJob job = PrintJob.Create(template, templateResults);

            try
            {
                job.Print();
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
