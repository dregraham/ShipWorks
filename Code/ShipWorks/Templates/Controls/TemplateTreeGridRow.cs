using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Customized grid row for use in the TemplateTree
    /// </summary>
    public class TemplateTreeGridRow : SandGridTreeRow
    {
        TemplateTreeNode node;

        /// <summary>
        /// Constructor for creating a row that represents a folder
        /// </summary>
        public TemplateTreeGridRow(TemplateTreeNode node)
            : base(node.Name, node.Image)
        {
            this.node = node;
        }

        /// <summary>
        /// The tree node this row represents
        /// </summary>
        public TemplateTreeNode TemplateTreeNode
        {
            get { return node; }
        }

        /// <summary>
        /// Indiciates if the row represents a folder
        /// </summary>
        public override bool IsFolder
        {
            get { return node.IsFolder; }
        }

        /// <summary>
        /// Indicates if the node this row represents is draggable
        /// </summary>
        public override bool IsDraggable
        {
            get
            {
                // This just prevents builtin folders from dragging
                return !node.IsFolder || !node.Folder.IsBuiltin;
            }
        }

        /// <summary>
        /// Indicates if its valid to drop the specified source row onto this one at the given location
        /// </summary>
        public override bool IsValidDrop(SandGridDragDropRow sourceRow, DropTargetState state)
        {
            if (sourceRow == null)
            {
                throw new ArgumentNullException("sourceRow");
            }

            TemplateTreeNode sourceNode = ((TemplateTreeGridRow) sourceRow).TemplateTreeNode;

            // Can't drop on top of oneself
            if (sourceNode.ID == this.node.ID)
            {
                return false;
            }

            // A folder can go at the top level
            if (state == DropTargetState.DropAbove || state == DropTargetState.DropBelow)
            {
                // If it would drop into a top level location...
                if (this.IsFolder && this.node.ParentFolder == null)
                {
                    // It's ok as long as its a folder that doesn't have snippets
                    if (sourceNode.IsFolder && (sourceNode.Folder.Templates.Count == 0 || !sourceNode.IsSnippetNode))
                    {
                        return true;
                    }
                }
            }

            // Dropping right on top of a folder node...
            if (state == DropTargetState.DropInside)
            {
                // An empty folder can go wherever
                if (sourceNode.IsFolder && sourceNode.Folder.Templates.Count == 0)
                {
                    return true;
                }

                // Snippets can only be drug to snippet locations
                if (sourceNode.IsSnippetNode && !this.TemplateTreeNode.IsSnippetNode)
                {
                    return false;
                }

                // Can't drag a non-snippet into a snippet location
                if (!sourceNode.IsSnippetNode && this.TemplateTreeNode.IsSnippetNode)
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// We can't be selected as apart of a HotTrack if we are a folder
        /// </summary>
        public override bool IsValidHotTrackSelection
        {
            get
            {
                return !IsFolder;
            }
        }
    }
}
