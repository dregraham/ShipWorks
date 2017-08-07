using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control for showing a MenuList of options, and a page for each one.
    /// </summary>
    [Designer(typeof(OptionControlDesigner))]
    public class OptionControl : Control
    {
        #region ChildControlCollection

        /// <summary>
        /// Custom child control colleciton
        /// </summary>
        class ChildControlCollection : Control.ControlCollection
        {
            OptionControl owner;

            /// <summary>
            /// Constructor
            /// </summary>
            public ChildControlCollection(OptionControl owner)
                : base(owner)
            {
                this.owner = owner;
            }

            /// <summary>
            /// Adding a control to the collection
            /// </summary>
            public override void Add(Control value)
            {
                // If its the menu list, then its ok
                if (value == owner.menuList)
                {
                    base.Add(value);
                    return;
                }

                OptionPage optionPage = value as OptionPage;

                // Otherwise has to be an OptionPage
                if (optionPage == null)
                {
                    throw new ArgumentException(string.Format("Can only add OptionPage to OptionControl.  Not {0}.", value.GetType().Name));
                }

                if (!owner.insertingItem)
                {
                    owner.AddOptionPage(optionPage);
                }

                base.Add(optionPage);

                optionPage.Visible = false;

                if (owner.IsHandleCreated)
                {
                    optionPage.Bounds = owner.DisplayRectangle;
                }

                ISite site = this.owner.Site;
                if ((site != null) && (optionPage.Site == null))
                {
                    IContainer container = site.Container;
                    if (container != null)
                    {
                        container.Add(optionPage);
                    }
                }

                owner.UpdatePageSelection(false);
            }

            /// <summary>
            /// Removing a control from the collection
            /// </summary>
            public override void Remove(Control value)
            {
                if (value == owner.menuList)
                {
                    Debug.Assert(false, "Cannot remove the MenuList");
                    throw new ArgumentException("Cannot remove the MenuList");
                }

                base.Remove(value);

                OptionPage optionPage = value as OptionPage;
                if (optionPage != null)
                {
                    int index = owner.OptionPages.IndexOf(optionPage);

                    int selectedIndex = owner.SelectedIndex;
                    if (index != -1)
                    {
                        owner.RemoveOptionPage(index);
                        if (index == selectedIndex && owner.optionPages.Count > 0)
                        {
                            owner.SelectedIndex = 0;
                        }
                    }

                    owner.UpdatePageSelection(false);
                }
            }
        }

        #endregion

        #region OptionPageCollection

        /// <summary>
        /// Collection of pages found in the OptionControl
        /// </summary>
        public sealed class OptionPageCollection : IList, ICollection, IEnumerable
        {
            OptionControl owner;

            /// <summary>
            /// Constructor
            /// </summary>
            public OptionPageCollection(OptionControl owner)
            {
                if (owner == null)
                {
                    throw new ArgumentNullException("owner");
                }

                this.owner = owner;
            }

            /// <summary>
            /// Add a page with the specified text in the MenuList
            /// </summary>
            public void Add(string text)
            {
                OptionPage page = new OptionPage();
                page.Text = text;

                Add(page);
            }

            /// <summary>
            /// Add the specified page
            /// </summary>
            public void Add(OptionPage value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Add(OptionPage)");
                }

                owner.Controls.Add(value);
            }

            /// <summary>
            /// Add the list of pages
            /// </summary>
            public void AddRange(OptionPage[] pages)
            {
                if (pages == null)
                {
                    throw new ArgumentNullException("pages");
                }

                foreach (OptionPage page in pages)
                {
                    Add(page);
                }
            }

            /// <summary>
            /// Remove all of the pages
            /// </summary>
            public void Clear()
            {
                owner.RemoveAllPages();
            }

            /// <summary>
            /// Determine if the given page is contained in the collection.
            /// </summary>
            public bool Contains(OptionPage page)
            {
                if (page == null)
                {
                    throw new ArgumentNullException("page", "Contains(OptionPage)");
                }

                return (IndexOf(page) != -1);
            }

            /// <summary>
            /// Get the proper enumerator
            /// </summary>
            public IEnumerator GetEnumerator()
            {
                return owner.optionPages.GetEnumerator();
            }

            /// <summary>
            /// Get the index of the specified page in the collection.  -1 if not found
            /// </summary>
            public int IndexOf(OptionPage page)
            {
                if (page == null)
                {
                    throw new ArgumentNullException("page", "IndexOf(OptionPage)");
                }

                return owner.optionPages.IndexOf(page);
            }

            /// <summary>
            /// Insert a page at the specified index with the given text.
            /// </summary>
            public void Insert(int index, string text)
            {
                OptionPage tabPage = new OptionPage();
                tabPage.Text = text;

                Insert(index, tabPage);
            }

            /// <summary>
            /// Insert the specified page at the given index.
            /// </summary>
            public void Insert(int index, OptionPage tabPage)
            {
                owner.InsertPage(index, tabPage);

                try
                {
                    owner.insertingItem = true;
                    owner.Controls.Add(tabPage);
                }
                finally
                {
                    owner.insertingItem = false;
                }

                owner.Controls.SetChildIndex(tabPage, index);
            }

            /// <summary>
            /// Indicates if the given index is valid
            /// </summary>
            private bool IsValidIndex(int index)
            {
                return ((index >= 0) && (index < this.Count));
            }

            /// <summary>
            /// Remove the specified page from the collection
            /// </summary>
            public void Remove(OptionPage value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Remove(OptionPage)");
                }

                owner.Controls.Remove(value);
            }

            /// <summary>
            /// Remove from the specified index
            /// </summary>
            public void RemoveAt(int index)
            {
                OptionPage page = owner.optionPages[index];

                Remove(page);
            }

            /// <summary>
            /// Copy the pages to an array
            /// </summary>
            void ICollection.CopyTo(Array array, int index)
            {
                if (Count > 0)
                {
                    Array.Copy(owner.optionPages.ToArray(), 0, array, index, Count);
                }
            }

            /// <summary>
            /// Add
            /// </summary>
            int IList.Add(object value)
            {
                OptionPage optionPage = value as OptionPage;

                if (optionPage == null)
                {
                    Debug.Assert(false, "Value: " + value);
                    throw new ArgumentException("IList.Add", "value");
                }

                this.Add(optionPage);

                return IndexOf(optionPage);
            }

            /// <summary>
            /// Contains
            /// </summary>
            bool IList.Contains(object value)
            {
                return ((value is OptionPage) && Contains((OptionPage) value));
            }

            /// <summary>
            /// Index
            /// </summary>
            int IList.IndexOf(object value)
            {
                OptionPage optionPage = value as OptionPage;

                if (optionPage != null)
                {
                    return IndexOf(optionPage);
                }

                return -1;
            }

            /// <summary>
            /// Insert
            /// </summary>
            void IList.Insert(int index, object value)
            {
                OptionPage optionPage = value as OptionPage;

                if (optionPage == null)
                {
                    throw new ArgumentException("IList.Insert", "value");
                }

                Insert(index, optionPage);
            }

            /// <summary>
            /// Remove
            /// </summary>
            void IList.Remove(object value)
            {
                OptionPage optionPage = value as OptionPage;

                if (optionPage != null)
                {
                    Remove(optionPage);
                }
            }

            /// <summary>
            /// Count of items in the collection
            /// </summary>
            [Browsable(false)]
            public int Count
            {
                get
                {
                    return owner.optionPages.Count;
                }
            }

            /// <summary>
            /// We are not read only
            /// </summary>
            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Indexor
            /// </summary>
            public OptionPage this[int index]
            {
                get
                {
                    return owner.optionPages[index];
                }
                set
                {
                    owner.SetOptionPage(index, value);
                }
            }


            /// <summary>
            /// We are not synced
            /// </summary>
            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Dumb SyncRoot
            /// </summary>
            object ICollection.SyncRoot
            {
                get
                {
                    return this;
                }
            }

            /// <summary>
            /// We do not have a fixed size
            /// </summary>
            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Indexor
            /// </summary>
            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    OptionPage optionPage = value as OptionPage;

                    if (optionPage != null)
                    {
                        throw new ArgumentException("Indexor", "value");
                    }

                    this[index] = optionPage;
                }
            }
        }

        #endregion

        public event OptionControlCancelEventHandler Deselecting;
        public event OptionControlEventHandler Deselected;

        public event OptionControlCancelEventHandler Selecting;
        public event OptionControlEventHandler Selected;
        public event EventHandler SelectedIndexChanged;

        bool inScaleCore;
        bool insertingItem;

        OptionPage lastSelectedPage;

        MenuList menuList = new MenuList();

        // Collection of our actual pages
        List<OptionPage> optionPages = new List<OptionPage>();

        // Collection we expost
        OptionPageCollection pageCollection;

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionControl()
        {
            pageCollection = new OptionPageCollection(this);

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            menuList.Width = 150;
        }

        /// <summary>
        /// Handle a Deselecting async event
        /// </summary>
        public Func<OptionPage, int, Task<bool>> DeselectingAsync;

        /// <summary>
        /// The collection of pages contained in the tab
        /// </summary>
        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MergableProperty(false)]
        [Editor(typeof(OptionPageCollectionEditor), typeof(UITypeEditor))]
        public OptionPageCollection OptionPages
        {
            get
            {
                return pageCollection;
            }
        }

        /// <summary>
        /// Controls the width of the MenuList portion of the control
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(150)]
        public int MenuListWidth
        {
            get
            {
                return menuList.Width;
            }
            set
            {
                menuList.Width = value;

                if (IsHandleCreated)
                {
                    ResizePages();
                }
            }
        }

        /// <summary>
        /// The current selected index
        /// </summary>
        [Browsable(false)]
        [Category("Behavior")]
        [DefaultValue(-1)]
        public int SelectedIndex
        {
            get
            {
                return menuList.SelectedIndex;
            }
            set
            {
                menuList.SelectedIndex = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        public OptionPage SelectedPage
        {
            get
            {
                int selectedIndex = this.SelectedIndex;
                if (selectedIndex == -1)
                {
                    return null;
                }

                return optionPages[selectedIndex];
            }
            set
            {
                SelectedIndex = optionPages.IndexOf(value);
            }
        }

        /// <summary>
        /// Just specify a new designer default
        /// </summary>
        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        /// <summary>
        /// Selection is changing
        /// </summary>
        private async Task<bool> DoSelectionChanging()
        {
            if (lastSelectedPage == null)
            {
                return true;
            }

            IContainerControl container = GetContainerControl();
            if ((container != null) && !base.DesignMode)
            {
                container.ActiveControl = this;
            }

            var wasCanceled = await OnDeselecting(lastSelectedPage, optionPages.IndexOf(lastSelectedPage))
                .ConfigureAwait(true);

            if (!wasCanceled)
            {
                OnDeselected(new OptionControlEventArgs(lastSelectedPage, optionPages.IndexOf(lastSelectedPage), TabControlAction.Deselected));
            }

            return !wasCanceled;
        }

        /// <summary>
        /// The selection has changed
        /// </summary>
        private bool DoSelectionChanged()
        {
            OptionControlCancelEventArgs e = new OptionControlCancelEventArgs(SelectedPage, SelectedIndex, false, TabControlAction.Selecting);
            OnSelecting(e);

            if (!e.Cancel)
            {
                OnSelected(new OptionControlEventArgs(SelectedPage, this.SelectedIndex, TabControlAction.Selected));
                OnSelectedIndexChanged(EventArgs.Empty);

                lastSelectedPage = this.SelectedPage;
            }

            return !e.Cancel;
        }

        /// <summary>
        /// Raise the Selecting event
        /// </summary>
        protected virtual void OnSelecting(OptionControlCancelEventArgs e)
        {
            Selecting?.Invoke(this, e);
        }

        /// <summary>
        /// Raise the Selected event
        /// </summary>
        protected virtual void OnSelected(OptionControlEventArgs e)
        {
            Selected?.Invoke(this, e);

            if (SelectedPage != null)
            {
                SelectedPage.FireEnter(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raise the SelectedIndexChanged event
        /// </summary>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            UpdatePageSelection(false);

            SelectedIndexChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raise the Deselecting event
        /// </summary>
        protected virtual async Task<bool> OnDeselecting(OptionPage lastSelectedPage, int optionPageIndex)
        {
            bool wasCanceled = false;

            if (DeselectingAsync != null)
            {
                wasCanceled = await DeselectingAsync(lastSelectedPage, optionPageIndex).ConfigureAwait(false);
            }

            OptionControlCancelEventArgs e = new OptionControlCancelEventArgs(
                lastSelectedPage,
                optionPageIndex,
                wasCanceled,
                TabControlAction.Deselecting);

            Deselecting?.Invoke(this, e);

            return e.Cancel;
        }

        /// <summary>
        /// Raise the Deselected event
        /// </summary>
        protected virtual void OnDeselected(OptionControlEventArgs e)
        {
            Deselected?.Invoke(this, e);

            if (SelectedPage != null)
            {
                SelectedPage.FireLeave(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Our control is creating
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            menuList.IntegralHeight = false;
            menuList.Location = new Point(0, 0);

            // Anchor didn't always work in the designer, so we hook to Resize.  Seemed to fix it.
            menuList.Height = Height;
            Resize += (o, e) => { menuList.Height = Height; };

            menuList.SelectedIndexChanged += OnMenuListSelectedIndexChanged;
            lastSelectedPage = SelectedPage;

            Controls.Add(menuList);

            ResizePages();
        }

        /// <summary>
        /// Use our own custom control collection
        /// </summary>
        protected override Control.ControlCollection CreateControlsInstance()
        {
            return new ChildControlCollection(this);
        }

        /// <summary>
        /// Provides the actual space where the OptionPage goes
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get
            {
                return new Rectangle(menuList.Width + 3, 0, Width - (menuList.Width + 3), Height);
            }
        }

        /// <summary>
        /// The selected index of the menu list has changed
        /// </summary>
        private async void OnMenuListSelectedIndexChanged(object sender, EventArgs e)
        {
            bool canceled = true;

            try
            {
                Cursor = Cursors.WaitCursor;
                menuList.Enabled = false;

                if (await DoSelectionChanging().ConfigureAwait(true))
                {
                    if (DoSelectionChanged())
                    {
                        canceled = false;
                    }
                }

                if (canceled)
                {
                    menuList.SelectedIndexChanged -= OnMenuListSelectedIndexChanged;
                    menuList.SelectedIndex = optionPages.IndexOf(lastSelectedPage);
                    menuList.SelectedIndexChanged += OnMenuListSelectedIndexChanged;
                }

                UpdatePageSelection(canceled);
            }
            finally
            {
                Cursor = Cursors.Default;
                menuList.Enabled = true;
            }
        }

        /// <summary>
        /// Updates the specified page from its properties. Not intended to be called by consumer code.
        /// </summary>
        internal void UpdatePage(OptionPage optionPage)
        {
            int index = optionPages.IndexOf(optionPage);

            SetOptionPage(index, optionPage);

            UpdatePageSelection(false);
        }

        /// <summary>
        /// Update the selection state of the tab page
        /// </summary>
        private void UpdatePageSelection(bool updateFocus)
        {
            if (base.IsHandleCreated)
            {
                int selectedIndex = this.SelectedIndex;

                if (selectedIndex != -1)
                {
                    if (inScaleCore)
                    {
                        optionPages[selectedIndex].SuspendLayout();
                    }

                    optionPages[selectedIndex].Bounds = this.DisplayRectangle;

                    if (inScaleCore)
                    {
                        optionPages[selectedIndex].ResumeLayout(false);
                    }

                    optionPages[selectedIndex].Visible = true;

                    if (updateFocus && !this.Focused)
                    {
                        bool flag = optionPages[selectedIndex].SelectNextControl(null, true, true, false, false);

                        if (flag)
                        {
                            if (!base.ContainsFocus)
                            {
                                IContainerControl containerControl = GetContainerControl();
                                if (containerControl != null)
                                {
                                    while (containerControl.ActiveControl is ContainerControl)
                                    {
                                        containerControl = (IContainerControl) containerControl.ActiveControl;
                                    }

                                    if (containerControl.ActiveControl != null)
                                    {
                                        containerControl.ActiveControl.Focus();
                                    }
                                }
                            }
                        }
                        else
                        {
                            IContainerControl containerControl = GetContainerControl();
                            if ((containerControl != null) && !base.DesignMode)
                            {
                                containerControl.ActiveControl = this;
                            }
                        }
                    }
                }

                for (int i = 0; i < optionPages.Count; i++)
                {
                    if (i != SelectedIndex)
                    {
                        optionPages[i].Visible = false;
                    }
                }

                if (Parent != null)
                {
                    Parent.Invalidate();
                }
            }
        }

        /// <summary>
        /// Add the given page to the MenuList and our internal collection
        /// </summary>
        private int AddOptionPage(OptionPage optionPage)
        {
            int index = menuList.Items.Add(optionPage);
            optionPages.Insert(index, optionPage);

            if (optionPages.Count == 1)
            {
                SelectedIndex = 0;
            }

            return index;
        }

        /// <summary>
        /// Insert the page at the given index
        /// </summary>
        private void InsertPage(int index, OptionPage optionPage)
        {
            if (index < 0 || index > optionPages.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (optionPage == null)
            {
                throw new ArgumentNullException("optionPage");
            }

            optionPages.Insert(index, optionPage);
            menuList.Items.Insert(index, optionPage);

            if (optionPages.Count == 1)
            {
                SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Remove the page at the given index
        /// </summary>
        private void RemoveOptionPage(int index)
        {
            if ((index < 0) || (index >= optionPages.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (optionPages[index] == lastSelectedPage)
            {
                lastSelectedPage = null;
            }

            optionPages.RemoveAt(index);
            menuList.Items.RemoveAt(index);
        }

        /// <summary>
        /// Remove all of the OptionPages
        /// </summary>
        private void RemoveAllPages()
        {
            foreach (OptionPage control in new List<OptionPage>(optionPages))
            {
                Controls.Remove(control);
            }
        }

        /// <summary>
        /// Set the page at the specified index
        /// </summary>
        public void SetOptionPage(int index, OptionPage value)
        {
            optionPages[index] = value;
            menuList.Items[index] = value;
        }

        /// <summary>
        /// Handle resizing
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ResizePages();
            UpdatePageSelection(false);
        }

        /// <summary>
        /// Track when we are scaling
        /// </summary>
        protected override void ScaleCore(float dx, float dy)
        {
            inScaleCore = true;
            base.ScaleCore(dx, dy);
            inScaleCore = false;
        }

        /// <summary>
        /// Resize all the pages to fit in the correct area
        /// </summary>
        private void ResizePages()
        {
            Rectangle displayRectangle = this.DisplayRectangle;

            foreach (OptionPage page in optionPages)
            {
                page.Bounds = displayRectangle;
            }
        }

        /// <summary>
        /// Raises the Enter event of the OptionControl
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (SelectedPage != null)
            {
                SelectedPage.FireEnter(e);
            }
        }

        /// <summary>
        /// Raises the leave event of the tab control
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            if (SelectedPage != null)
            {
                SelectedPage.FireLeave(e);
            }

            base.OnLeave(e);
        }

        /// <summary>
        /// Handle is being crerated
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            ResizePages();
            UpdatePageSelection(false);
        }
    }
}
