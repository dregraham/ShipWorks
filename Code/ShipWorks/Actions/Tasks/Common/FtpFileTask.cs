using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing;
using log4net;

namespace ShipWorks.Actions.Tasks.Common
{

    /// <summary>
    /// Task for ftping a chosen template
    /// </summary>
    [ActionTask("Transfer file(s)", "Ftp")]
    public class FtpFileTask : TemplateBasedTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FtpFileTask));

        /// <summary>
        /// Gets or sets the FTP account unique identifier.
        /// </summary>

        public long? FtpAccountID { get; set; }
        
        /// <summary>
        /// Gets or sets the FTP folder.
        /// </summary>
        public string FtpFolder { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the FTP file.
        /// </summary>
        public string FtpFileName { get; set; }


        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Ftp file(s) using:";
            }
        }
		
        /// <summary>
        /// Gets a value indicating whether to postpone running or not.
        /// </summary>
        public override bool EnablePostpone
        {
            get { return false; }
        }

        public override ActionTaskEditor CreateEditor()
        {
            return new FtpFileTaskEditor(this);
        }

        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            throw new NotImplementedException();
        }
    }
}
