using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Interapptive.Shared;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Design
{
    /// <summary>
    /// Designer for the OptionControl
    /// </summary>
    public class OptionControlDesigner : ParentControlDesigner
    {
        // bool optionControlSelected;
        bool forwardOnDrag;
        bool addingOnInitialize;

        bool isHitTesting = false;
        bool inMenuListArea = false;

        int persistedSelectedIndex;

        DesignerVerbCollection verbs;
        DesignerVerb removeVerb;

        /// <summary>
        /// Initialize the designer to desig the given component
        /// </summary>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            AutoResizeHandles = true;

            ISelectionService selectionService = (ISelectionService) GetService(typeof(ISelectionService));
            if (selectionService != null)
            {
                selectionService.SelectionChanged += new EventHandler(this.OnSelectionChanged);
            }

            IComponentChangeService componentService = (IComponentChangeService) GetService(typeof(IComponentChangeService));
            if (componentService != null)
            {
                componentService.ComponentChanged += new ComponentChangedEventHandler(this.OnComponentChanged);
            }

            OptionControl control = component as OptionControl;
            if (control != null)
            {
                control.SelectedIndexChanged += new EventHandler(this.OnPageSelectedIndexChanged);
                control.ControlAdded += new ControlEventHandler(this.OnControlAdded);
            }
        }

        /// <summary>
        /// Initialize a newly created OptionControl
        /// </summary>
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            try
            {
                addingOnInitialize = true;

                OnAdd(this, EventArgs.Empty);
                OnAdd(this, EventArgs.Empty);
            }
            finally
            {
                addingOnInitialize = false;
            }

            MemberDescriptor member = TypeDescriptor.GetProperties(base.Component)["Controls"];
            RaiseComponentChanging(member);
            RaiseComponentChanged(member, null, null);

            OptionControl control = Component as OptionControl;
            if (control != null)
            {
                control.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Designer is disposing
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ISelectionService selectionService = (ISelectionService) GetService(typeof(ISelectionService));
                if (selectionService != null)
                {
                    selectionService.SelectionChanged -= new EventHandler(this.OnSelectionChanged);
                }

                IComponentChangeService componentService = (IComponentChangeService) GetService(typeof(IComponentChangeService));
                if (componentService != null)
                {
                    componentService.ComponentChanged -= new ComponentChangedEventHandler(this.OnComponentChanged);
                }

                OptionControl control = Component as OptionControl;
                if (control != null)
                {
                    control.SelectedIndexChanged -= new EventHandler(this.OnPageSelectedIndexChanged);
                    control.ControlAdded -= new ControlEventHandler(this.OnControlAdded);
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Indicate if we can be a parent of the given control
        /// </summary>
        public override bool CanParent(Control control)
        {
            return ((control is OptionPage) && ! this.Control.Contains(control));
        }
        
        /// <summary>
        /// Indicates if a mouse message at the specified point should pass through
        /// </summary>
        protected override bool GetHitTest(Point point)
        {
            Point clientPt = Control.PointToClient(point);
            return !Control.DisplayRectangle.Contains(clientPt);
        }

        /// <summary>
        /// Special WndProc processing
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84 && !isHitTesting)
            {
                isHitTesting = true;

                Point point = new Point((short) NativeMethods.LOWORD((int) m.LParam), (short) NativeMethods.HIWORD((int) m.LParam));
                
                try
                {
                    inMenuListArea = GetHitTest(point);
                }
                catch
                {
                    inMenuListArea = false;

                    throw;
                }

                isHitTesting = false;
            }

            if (inMenuListArea)
            {
                DefWndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Designer property filtering
        /// </summary>
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            string[] names = new string[] { "SelectedIndex" };
            Attribute[] attributes = new Attribute[0];

            for (int i = 0; i < names.Length; i++)
            {
                PropertyDescriptor oldDescriptor = (PropertyDescriptor) properties[names[i]];
                if (oldDescriptor != null)
                {
                    properties[names[i]] = TypeDescriptor.CreateProperty(typeof(OptionControlDesigner), oldDescriptor, attributes);
                }
            }
        }

        /// <summary>
        /// When the OptionControl is selected, actually create the items in the page
        /// </summary>
        [NDependIgnoreTooManyParams]
        protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
        {
            OptionControl control = (OptionControl) this.Control;
            if (control.SelectedPage == null)
            {
                throw new ArgumentException("OptionControlInvalidOptionPageType" + tool.DisplayName);
            }

            IDesignerHost designerHost = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if (designerHost != null)
            {
                OptionPageDesigner toInvoke = (OptionPageDesigner) designerHost.GetDesigner(control.SelectedPage);
                ParentControlDesigner.InvokeCreateTool(toInvoke, tool);
            }

            return null;
        }

        /// <summary>
        /// Don't allow the lasso
        /// </summary>
        protected override bool AllowControlLasso
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Snap line participation
        /// </summary>
        public override bool ParticipatesWithSnapLines
        {
            get
            {
                if (!forwardOnDrag)
                {
                    return false;
                }

                OptionPageDesigner pageDesigner = GetSelectedOptionPageDesigner();
                if (pageDesigner != null)
                {
                    return pageDesigner.ParticipatesWithSnapLines;
                }

                return true;
            }
        }

        /// <summary>
        /// Designer Property
        /// </summary>
        private int SelectedIndex
        {
            get
            {
                return persistedSelectedIndex;
            }
            set
            {
                persistedSelectedIndex = value;
            }
        }
 
        /// <summary>
        /// Get the supported collection of verbs
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (verbs == null)
                {

                    verbs = new DesignerVerbCollection();
                    verbs.Add(new DesignerVerb("Add Page", new EventHandler(this.OnAdd)));

                    removeVerb = new DesignerVerb("Remove Page", new EventHandler(this.OnRemove));
                    verbs.Add(this.removeVerb);
                }

                CheckVerbStatus();

                return verbs;
            }
        }

        /// <summary>
        /// Add a new page to the OptionControl
        /// </summary>
        private void OnAdd(object sender, EventArgs eevent)
        {
            OptionControl optionControl = (OptionControl) base.Component;
            IDesignerHost designerHost = (IDesignerHost) GetService(typeof(IDesignerHost));

            if (designerHost != null)
            {
                DesignerTransaction transaction = null;

                try
                {
                    try
                    {
                        transaction = designerHost.CreateTransaction("OptionControlAddPage" + base.Component.Site.Name);
                    }
                    catch (CheckoutException ex)
                    {
                        if (ex != CheckoutException.Canceled)
                        {
                            throw;
                        }

                        return;
                    }

                    MemberDescriptor memberControls = TypeDescriptor.GetProperties(optionControl)["Controls"];
                    OptionPage page = (OptionPage) designerHost.CreateComponent(typeof(OptionPage));

                    if (!addingOnInitialize)
                    {
                        RaiseComponentChanging(memberControls);
                    }

                    string name = null;
                    PropertyDescriptor nameDescriptor = TypeDescriptor.GetProperties(page)["Name"];
                    if ((nameDescriptor != null) && (nameDescriptor.PropertyType == typeof(string)))
                    {
                        name = (string) nameDescriptor.GetValue(page);
                    }

                    if (name != null)
                    {
                        PropertyDescriptor textDescriptor = TypeDescriptor.GetProperties(page)["Text"];
                        if (textDescriptor != null)
                        {
                            textDescriptor.SetValue(page, name);
                        }
                    }

                    page.Padding = new Padding(3);

                    optionControl.Controls.Add(page);
                    optionControl.SelectedIndex = optionControl.OptionPages.Count - 1;

                    if (!addingOnInitialize)
                    {
                        RaiseComponentChanged(memberControls, null, null);
                    }
                }
                finally
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Remove a page from the OptionControl
        /// </summary>
        private void OnRemove(object sender, EventArgs eevent)
        {
            OptionControl optionControl = (OptionControl) base.Component;
            if ((optionControl != null) && (optionControl.OptionPages.Count != 0))
            {
                MemberDescriptor memberControls = TypeDescriptor.GetProperties(base.Component)["Controls"];
                OptionPage selectedPage = optionControl.SelectedPage;
                IDesignerHost designerHost = (IDesignerHost) this.GetService(typeof(IDesignerHost));

                if (designerHost != null)
                {
                    DesignerTransaction transaction = null;

                    try
                    {
                        try
                        {
                            transaction = designerHost.CreateTransaction("OptionControlRemoveTab" + base.Component.Site.Name);
                            RaiseComponentChanging(memberControls);
                        }
                        catch (CheckoutException ex)
                        {
                            if (ex != CheckoutException.Canceled)
                            {
                                throw;
                            }

                            return;
                        }

                        designerHost.DestroyComponent(selectedPage);

                        RaiseComponentChanged(memberControls, null, null);
                    }
                    finally
                    {
                        if (transaction != null)
                        {
                            transaction.Commit();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the designer of the selected page
        /// </summary>
        private OptionPageDesigner GetSelectedOptionPageDesigner()
        {
            OptionPageDesigner designer = null;
            OptionPage selectePage = ((OptionControl) base.Component).SelectedPage;

            if (selectePage != null)
            {
                IDesignerHost service = (IDesignerHost) GetService(typeof(IDesignerHost));
                if (service != null)
                {
                    designer = service.GetDesigner(selectePage) as OptionPageDesigner;
                }
            }

            return designer;
        }
 
        /// <summary>
        /// The selection in the designer has changed
        /// </summary>
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            ISelectionService selectionService = (ISelectionService) GetService(typeof(ISelectionService));

            // optionControlSelected = false;

            if (selectionService != null)
            {
                ICollection selectedComponents = selectionService.GetSelectedComponents();
                OptionControl optionControl = (OptionControl) base.Component;

                foreach (object component in selectedComponents)
                {
                    if (component == optionControl)
                    {
                        // optionControlSelected = true;
                    }

                    OptionPage pageOfComponent = GetOptionPageOfComponent(component);

                    if (pageOfComponent != null && pageOfComponent.Parent == optionControl)
                    {
                        // optionControlSelected = false;
                        optionControl.SelectedPage = pageOfComponent;

                        //CANT ((SelectionManager) this.GetService(typeof(SelectionManager))).Refresh();

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// A control has been added to the OptionControl
        /// </summary>
        private void OnControlAdded(object sender, ControlEventArgs e)
        {
            if ((e.Control != null) && !e.Control.IsHandleCreated)
            {
                IntPtr handle = e.Control.Handle;
            }
        }

        /// <summary>
        /// A component on the design surface has changed
        /// </summary>
        private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            CheckVerbStatus();
        }

        /// <summary>
        /// The selected OptionPage has changed via the OptionControl
        /// </summary>
        private void OnPageSelectedIndexChanged(object sender, EventArgs e)
        {
            ISelectionService selectionService = (ISelectionService) this.GetService(typeof(ISelectionService));
            if (selectionService != null)
            {
                ICollection selectedComponents = selectionService.GetSelectedComponents();
                OptionControl optionControl = (OptionControl) base.Component;

                bool ourPageHasSelection = false;
                foreach (object component in selectedComponents)
                {
                    OptionPage pageOfComponent = GetOptionPageOfComponent(component);
                    if (pageOfComponent != null && pageOfComponent.Parent == optionControl && pageOfComponent == optionControl.SelectedPage)
                    {
                        ourPageHasSelection = true;
                        break;
                    }
                }

                if (!ourPageHasSelection)
                {
                    selectionService.SetSelectedComponents(new object[] { base.Component });
                }
            }
        }

        /// <summary>
        /// Ensure our verbs are up-to-date
        /// </summary>
        private void CheckVerbStatus()
        {
            if (removeVerb != null)
            {
                removeVerb.Enabled = ((OptionControl) Control).SelectedPage != null;
            }
        }

        /// <summary>
        /// Get the OptionPage, if any, that contains the specified component
        /// </summary>
        private OptionPage GetOptionPageOfComponent(object component)
        {
            if (!(component is Control))
            {
                return null;
            }

            Control parent = (Control) component;
            while ((parent != null) && !(parent is OptionPage))
            {
                parent = parent.Parent;
            }

            return (OptionPage) parent;
        }

        /// <summary>
        /// Drag enter
        /// </summary>
        protected override void OnDragEnter(DragEventArgs de)
        {
            forwardOnDrag = true;

            OptionPageDesigner pageDesigner = GetSelectedOptionPageDesigner();
            if (pageDesigner != null)
            {
                pageDesigner.OnDragEnterInternal(de);
            }
        }

        /// <summary>
        /// Drag over
        /// </summary>
        protected override void OnDragOver(DragEventArgs de)
        {
            if (forwardOnDrag)
            {
                OptionControl control = (OptionControl) this.Control;
                Point pt = Control.PointToClient(new Point(de.X, de.Y));

                if (!control.DisplayRectangle.Contains(pt))
                {
                    de.Effect = DragDropEffects.None;
                }
                else
                {
                    OptionPageDesigner pageDesigner = GetSelectedOptionPageDesigner();
                    if (pageDesigner != null)
                    {
                        pageDesigner.OnDragOverInternal(de);
                    }
                }
            }
            else
            {
                base.OnDragOver(de);
            }
        }

        /// <summary>
        /// Drag leave
        /// </summary>
        protected override void OnDragLeave(EventArgs e)
        {
            if (forwardOnDrag)
            {
                OptionPageDesigner pageDesigner = GetSelectedOptionPageDesigner();
                if (pageDesigner != null)
                {
                    pageDesigner.OnDragLeaveInternal(e);
                }
            }
            else
            {
                base.OnDragLeave(e);
            }
            
            forwardOnDrag = false;
        }

        /// <summary>
        /// Drag & drop
        /// </summary>
        protected override void OnDragDrop(DragEventArgs de)
        {
            if (forwardOnDrag)
            {
                OptionPageDesigner pageDesigner = GetSelectedOptionPageDesigner();
                if (pageDesigner != null)
                {
                    pageDesigner.OnDragDropInternal(de);
                }
            }
            else
            {
                base.OnDragDrop(de);
            }

            forwardOnDrag = false;
        }

        /// <summary>
        /// Give drag feedback
        /// </summary>
        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            if (forwardOnDrag)
            {
                OptionPageDesigner pageDesigner = GetSelectedOptionPageDesigner();
                if (pageDesigner != null)
                {
                    pageDesigner.OnGiveFeedbackInternal(e);
                }
            }
            else
            {
                base.OnGiveFeedback(e);
            }
        }
     }
}
