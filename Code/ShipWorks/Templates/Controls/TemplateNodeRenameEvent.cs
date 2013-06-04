using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.ComponentModel;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// delegate signature for the rename event
    /// </summary>
    public delegate void TemplateNodeRenameEventHandler(object sender, TemplateNodeRenameEventArgs e);

    /// <summary>
    /// EventArgs for when an object is being renamed
    /// </summary>
    public class TemplateNodeRenameEventArgs : CancelEventArgs
    {
        // The object that is being renamed
        TemplateTreeNode treeNode;

        // The proposed name
        string proposed;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateNodeRenameEventArgs(TemplateTreeNode treeNode)
            : this(treeNode, null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateNodeRenameEventArgs(TemplateTreeNode treeNode, string proposed)
        {
            this.treeNode = treeNode;
            this.proposed = proposed;
        }

        /// <summary>
        /// The object that is being renamed
        /// </summary>
        public TemplateTreeNode TemplateTreeNode
        {
            get { return treeNode; }
        }

        /// <summary>
        /// The proposed value that the node will be renamed to
        /// </summary>
        public string Proposed
        {
            get
            {
                return proposed;
            }
            set
            {
                proposed = value;
            }
        }
    }
}
