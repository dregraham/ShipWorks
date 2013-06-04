using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extention of the LLBLGen ActionTaskEntity
    /// </summary>
    public partial class ActionTaskEntity
    {
        /// <summary>
        /// If FlowSuccess is set to move to a specific task, this is the bubble its in.  Only valid within the ActionEditorDlg.
        /// </summary>
        public object FlowSuccessBubble
        {
            get;
            set;
        }

        /// <summary>
        /// If FlowSkipped is set to move to a specific task, this is the bubble its in.  Only valid within the ActionEditorDlg.
        /// </summary>
        public object FlowSkippedBubble
        {
            get;
            set;
        }

        /// <summary>
        /// If FlowError is set to move to a specific task, this is the bubble its in.  Only valid within the ActionEditorDlg.
        /// </summary>
        public object FlowErrorBubble
        {
            get;
            set;
        }
    }
}
