using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Allows user to edit all information needed for the FtpFileTask
    /// </summary>
    public class FtpFileTaskEditor:TemplateBasedTaskEditor
    {
        private FtpFileTask ftpFileTask;

        public FtpFileTaskEditor(FtpFileTask ftpFileTask)
        {
            // TODO: Complete member initialization
            this.ftpFileTask = ftpFileTask;
        }
    }
}
