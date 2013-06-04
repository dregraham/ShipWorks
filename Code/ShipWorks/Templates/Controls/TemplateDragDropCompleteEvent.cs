using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Filters;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.Templates.Controls
{
    public delegate void TemplateDragDropCompleteEventHandler(object sender, TemplateDragDropCompleteEventArgs e);

    public class TemplateDragDropCompleteEventArgs : EventArgs
    {
        TemplateTreeNode draggedNode;
        TemplateFolderEntity targetFolder;

        // The original args that triggered this one
        GridRowDroppedEventArgs dropArgs;

        /// <summary>
        /// Construtor
        /// </summary>
        public TemplateDragDropCompleteEventArgs(TemplateTreeNode draggedNode, TemplateFolderEntity targetFolder, GridRowDroppedEventArgs dropArgs)
        {
            this.draggedNode = draggedNode;
            this.targetFolder = targetFolder;
            this.dropArgs = dropArgs;
        }

        /// <summary>
        /// The node that is being dropped
        /// </summary>
        public TemplateTreeNode DraggedNode
        {
            get { return draggedNode; }
        }

        /// <summary>
        /// The folder that the item is being dropped into
        /// </summary>
        public TemplateFolderEntity TargetFolder
        {
            get { return targetFolder; }
        }

        /// <summary>
        /// The state of the CTRL SHIIFT ALT keys, and mouse buttons
        /// </summary>
        public int KeyState
        {
            get { return dropArgs.KeyState; }
        }

        /// <summary>
        /// Indicates if the drop insert position indicator should be automatically cleared
        /// </summary>
        public bool AutoClearDropIndicator
        {
            get
            {
                return dropArgs.AutoClearDropIndicator;
            }
            set
            {
                dropArgs.AutoClearDropIndicator = value;
            }
        }

        /// <summary>
        /// Used to clear the drop indiciator, when AutoClearDropIndicator was false
        /// </summary>
        public void ClearDropIndicator()
        {
            dropArgs.ClearDropIndicator();
        }
    }
}
