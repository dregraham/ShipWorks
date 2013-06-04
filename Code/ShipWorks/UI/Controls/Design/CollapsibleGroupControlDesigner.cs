using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls.Design
{
    /// <summary>
    /// Designer for the CollapsibleGroupControl class
    /// </summary>
    class CollapsibleGroupControlDesigner : ParentControlDesigner
    {
        CollapsibleGroupControl sectionControl;
        IDesignerHost desinerHost;

        /// <summary>
        /// Initialize the designer
        /// </summary>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            AutoResizeHandles = true;

            sectionControl = component as CollapsibleGroupControl;
            desinerHost = (IDesignerHost) GetService(typeof(IDesignerHost));

            if (sectionControl != null)
            {
                EnableDesignMode(sectionControl.ContentPanel, "ContentPanel");
            }
        }

        /// <summary>
        ///  We don't directly parent anything
        /// </summary>
        public override bool CanParent(Control control)
        {
            return false;
        }

        /// <summary>
        /// Get the number of internal designers managed by this parent.  We just have one, the KryptonHeaderGroup designer.
        /// </summary>
        public override int NumberOfInternalControlDesigners()
        {
            if (sectionControl != null)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Get the internal designer
        /// </summary>
        public override ControlDesigner InternalControlDesigner(int internalControlIndex)
        {
            if ((sectionControl != null) && (internalControlIndex == 0))
            {
                return (ControlDesigner) desinerHost.GetDesigner(sectionControl.ContentPanel);
            }

            return null;
        }
    }
}
