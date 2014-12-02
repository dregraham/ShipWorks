using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Drawing.Design;
using System.ComponentModel.Design;
using ShipWorks.UI.Wizard.Design;

namespace ShipWorks.UI.Wizard
{
    /// <summary>
    /// Base for windows that are wizard-based.
    /// </summary>
    [DefaultProperty("Pages")]
    [Designer(typeof(WizardFormDesigner), typeof(IRootDesigner))]
    public partial class WizardForm : Form
    {
        #region WizardPageCollection

        /// <summary>
        /// Strong typed collection of WizardPage
        /// </summary>
        public sealed class WizardPageCollection : IList
        {
            WizardForm owner = null;

            /// <summary>
            /// Constructor
            /// </summary>
            public WizardPageCollection(WizardForm owner)
            {
                if (owner == null)
                {
                    throw new ArgumentNullException("owner");
                }

                this.owner = owner;
            }

            /// <summary>
            /// Add a WizardPage to the the WizardForm control
            /// </summary>
            public int Add(WizardPage page)
            {
                if (page == null)
                {
                    throw new ArgumentNullException("page");
                }

                // Add the page to the wizard
                return owner.InternalAddPage(page);
            }

            /// <summary>
            /// Add a range of pages to the WizardForm control
            /// </summary>
            public void AddRange(WizardPage[] pages)
            {
                if (pages == null)
                {
                    throw new ArgumentNullException("pages");
                }

                for (int i = 0; i < pages.Length; i++)
                {
                    Add(pages[i]);
                }
            }

            /// <summary>
            /// Removes a page from the wizard
            /// </summary>
            public void Remove(WizardPage page)
            {
                int index = IndexOf(page);

                if (index != -1)
                {
                    RemoveAt(index);
                }
            }

            /// <summary>
            /// Removes the page at the specified index from the wizard
            /// </summary>
            public void RemoveAt(int index)
            {
                if ((index < 0) || (index >= owner.pages.Count))
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                owner.InternalRemoveAt(index);
            }

            /// <summary>
            /// Inserts a page into the wizard
            /// </summary>
            public void Insert(int index, WizardPage page)
            {
                if (page == null)
                {
                    throw new ArgumentNullException("page");
                }

                if ((index < 0) || (index > owner.pages.Count))
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                owner.InternalInsertPage(index, page);
            }

            /// <summary>
            /// Returns whether the page is contained in this collection
            /// </summary>
            public bool Contains(WizardPage page)
            {
                return (IndexOf(page) != -1);
            }

            /// <summary>
            /// Returns the index of the page in the collection
            /// </summary>
            public int IndexOf(WizardPage page)
            {
                return owner.pages.IndexOf(page);
            }

            /// <summary>
            /// Clear all the pages from the contorl
            /// </summary>
            public void Clear()
            {
                if (owner.pages.Count == 0)
                {
                    return;
                }

                for (int index = owner.pages.Count; index > 0; index--)
                {
                    owner.InternalRemoveAt(index - 1);
                }
            }

            /// <summary>
            /// Strong typed indexer
            /// </summary>
            public WizardPage this[int index]
            {
                get
                {
                    if ((index < 0) || (index >= owner.pages.Count))
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }

                    return (WizardPage) owner.pages[index];
                }

                set
                {
                    if ((index < 0) || (index >= owner.pages.Count))
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }

                    if (value == null)
                    {
                        throw new ArgumentNullException("value");
                    }

                    owner.InternalUpdatePage(index, value);
                }
            }

            #region IList Members

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    WizardPage page = value as WizardPage;
                    if (page == null)
                    {
                        throw new ArgumentException("Parameter 'value' must be of type WizardPage.");
                    }

                    this[index] = page;
                }
            }

            void IList.Insert(int index, object value)
            {
                WizardPage page = value as WizardPage;
                if (page == null)
                {
                    throw new ArgumentException("Parameter 'value' must be of type WizardPage'");
                }

                this.Insert(index, page);
            }

            void IList.Remove(object value)
            {
                WizardPage page = value as WizardPage;
                if (page != null)
                {
                    this.Remove(page);
                }
            }

            bool IList.Contains(object value)
            {
                WizardPage page = value as WizardPage;
                if (page != null)
                {
                    return this.Contains(page);
                }

                return false;
            }

            int IList.IndexOf(object value)
            {
                WizardPage page = value as WizardPage;
                if (page != null)
                {
                    return this.IndexOf(page);
                }

                return -1;
            }

            int IList.Add(object value)
            {
                WizardPage page = value as WizardPage;
                if (page == null)
                {
                    throw new ArgumentException("The parameter 'value' must be of type WizardPage.");
                }

                return this.Add(page);
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            #endregion

            #region ICollection Members

            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            public int Count
            {
                get
                {
                    return owner.pages.Count;
                }
            }

            void ICollection.CopyTo(Array array, int index)
            {
                if (owner.pages.Count > 0)
                {
                    owner.pages.ToArray().CopyTo(array, index);
                }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    return this;
                }
            }

            #endregion

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return owner.pages.GetEnumerator();
            }

            #endregion
        }

        #endregion

        // If we are in WinForms initialization
        bool loaded;

        // If we can currently be closed\cancelled
        bool canCancel = true;

        // Determines if the last page is just a Finish page, or it it can be cancelled still
        // at that point.
        bool lastPageCancelable = false;
        
        // Internal collection actually holding the pages
        List<WizardPage> pages = new List<WizardPage>();

        // Exposed strongly typed collection of pages
        WizardPageCollection pageCollection;

        // Used for back navigation
        Dictionary<WizardPage, WizardPage> backNavigation = new Dictionary<WizardPage, WizardPage>();

        // List of pages that have been navigated to, so we know when its the first time or not
        List<WizardPage> firstTimeStepInto = new List<WizardPage>();

        /// <summary>
        /// Constructor
        /// </summary>
		public WizardForm()
		{
			InitializeComponent();

            FinishCancels = false;

            // Create our page collection 
            pageCollection = new WizardPageCollection(this);
		}

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void  OnLoad(EventArgs e)
        {
            loaded = true;

            if (pages.Count > 0)
            {
                mainPanel.Controls.Clear();
                ShowPage(0, WizardStepReason.StepForward);
            }

            // Raise the event after we show the initial page, in case someone's Load handler changes the first page
 	        base.OnLoad(e);

            // Maybe the removed the first page?
            if (CurrentIndex == -1 && Pages.Count > 0)
            {
                ShowPage(0, WizardStepReason.StepForward);
            }
        }

        #region Collection Handling

        /// <summary>
        /// Add the page to our internal collection
        /// </summary>
        private int InternalAddPage(WizardPage page)
        {
            if (pages.Contains(page))
            {
                throw new ArgumentException("The page already exists in the collection.", "page");
            }

            page.Wizard = this;
            pages.Add(page);

            if (pages.Count == 1)
            {
                // Make this the new visible page
                ShowPage(0);
            }

            // Update the control buttons
            UpdateWizardButtons();

            return pages.Count - 1;
        }

        /// <summary>
        /// Removes the page at the specified index
        /// </summary>
        private void InternalRemoveAt(int index)
        {
            WizardPage page = (WizardPage) pages[index];

            // If this page is currently displayed, remove it
            if (CurrentPage == page)
            {
                if (index > 0)
                    ShowPage(index - 1);
                else
                    mainPanel.Controls.Clear();
            }

            // Update the control buttons
            UpdateWizardButtons();

            pages.RemoveAt(index);
            page.Parent = null;
            page.Wizard = null;
        }

        /// <summary>
        /// Inserts a page at the specified index
        /// </summary>
        private void InternalInsertPage(int index, WizardPage page)
        {
            if (pages.Contains(page))
            {
                throw new ArgumentException("The page already exists in the collection.", "page");
            }

            page.Wizard = this;

            pages.Insert(index, page);

            if (pages.Count == 1)
            {
                // Show the newly inserted page
                ShowPage(index);
            }

            // Update the control buttons
            UpdateWizardButtons();
        }

        /// <summary>
        /// Updates the specified index with the new page object
        /// </summary>
        private void InternalUpdatePage(int index, WizardPage page)
        {
            if (pages[index] != page && pages.Contains(page))
            {
                throw new ArgumentException("The page already exists in the collection.", "page");
            }

            WizardPage oldPage = (WizardPage) pages[index];

            pages[index] = page;

            // If this page is currently displayed, refresh it
            if (CurrentPage == oldPage)
            {
                ShowPage(index);
            }
        }

        /// <summary>
        /// Find the first page of the given System.Type
        /// </summary>
        public WizardPage FindPage(Type pageType)
        {
            foreach (WizardPage page in pages)
            {
                if (page.GetType() == pageType)
                {
                    return page;
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// All of the wizard's pages
        /// </summary>
        [EditorAttribute(typeof(CollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Behavior")]
        public WizardPageCollection Pages
        {
            get
            {
                return pageCollection;
            }
        }

        /// <summary>
        /// Determine's of the last page is just a Finish button, or if its cancelable and can be 
        /// gone back from.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool LastPageCancelable
        {
            get
            {
                return lastPageCancelable;
            }
            set
            {
                lastPageCancelable = value;
            }
        }

        /// <summary>
        /// Returns the page currently being displayed by the wizard
        /// </summary>
        [Browsable(false)]
        public WizardPage CurrentPage
        {
            get
            {
                if (mainPanel.Controls.Count != 1)
                {
                    return null;
                }

                return mainPanel.Controls[0] as WizardPage;
            }
        }

        /// <summary>
        /// Returns the index of the page being displayed
        /// </summary>
        [Browsable(false)]
        public int CurrentIndex
        {
            get
            {
                return (CurrentPage != null) ? pages.IndexOf(CurrentPage) : -1;
            }
        }

        /// <summary>
        /// Controls if the Next\Finish button is visible
        /// </summary>
        [DefaultValue(true)]
        public bool NextVisible
        {
            get
            {
                return next.Visible;
            }
            set
            {
                next.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets if the next button is enabled
        /// </summary>
        public bool NextEnabled
        {
            get
            {
                return next.Enabled;
            }
            set
            {
                next.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets if the back button is enabled
        /// </summary>
        public bool BackEnabled
        {
            get
            {
                return back.Enabled;
            }
            set
            {
                back.Enabled = value;
            }
        }

        /// <summary>
        /// Indicates if the wizard can currently be cancelled
        /// </summary>
        public bool CanCancel
        {
            get
            {
                return canCancel;
            }
            set
            {
                canCancel = value;

                UpdateWizardButtons();
            }
        }

        /// <summary>
        /// If true, canceling 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [finish cancels]; otherwise, <c>false</c>.
        /// </value>
        public bool FinishCancels { get; set; }
        

        /// <summary>
        /// Show the page at the given index
        /// </summary>
        private WizardPage ShowPage(int index)
        {
            return ShowPage(index, WizardStepReason.None);
        }

        /// <summary>
        /// Show the page at the given index
        /// </summary>
        private WizardPage ShowPage(int index, WizardStepReason reason)
        {
            // Make sure the new index is valid
            if (index < 0)
            {
                Debug.Fail("Invalid wizard page.");
                return null;
            }

            WizardPage page = (WizardPage) pages[index];

            // Already shown
            if (mainPanel.Controls.Contains(page))
            {
                // Just refresh it
                page.Invalidate();
                return page;
            }

            // Reset the enable state of next right-before entering the page.
            next.Enabled = true;
            back.Enabled = true;
            canCancel = true;

            if (!DesignMode)
            {
                // If we are moving as a result of the user clicking next or back, we tell
                // the page they are going to that they are being gone to.  The page is given
                // the chance to skip itself.
                if (reason != WizardStepReason.None)
                {
                    WizardPage skipToPage = null;

                    if (reason == WizardStepReason.StepForward)
                    {
                        if (index + 1 != pageCollection.Count)
                        {
                            skipToPage = pageCollection[index + 1];
                        }
                    }
                    else
                    {
                        skipToPage = GetPreviousPage(page);
                    }

                    bool firstTime = !firstTimeStepInto.Contains(page);

                    // Create the event args for the step
                    WizardSteppingIntoEventArgs args = new WizardSteppingIntoEventArgs(page, reason, firstTime, skipToPage);

                    // Mark this page as seen so we get the "firstTime" flag correct
                    if (firstTime)
                    {
                        firstTimeStepInto.Add(page);
                    }

                    // Tell the current page we are about to step into it
                    page.RaiseSteppingInto(args);

                    // See if they changed the page we are supposed to go to
                    if (args.Skip)
                    {
                        // See if we still have to raise the step event
                        if (args.RaiseStepEventWhenSkipping)
                        {
                            WizardStepEventArgs stepArgs = new WizardStepEventArgs(args.SkipToPage, true);

                            // Raise the step event
                            if (reason == WizardStepReason.StepForward)
                            {
                                page.RaiseSteppingNext(stepArgs);
                            }
                            else
                            {
                                page.RaiseSteppingBack(stepArgs);
                            }

                            if (stepArgs.NextPage == null)
                            {
                                throw new InvalidOperationException("NextPage cannot be null.");
                            }
                                
                            // Update the page to skip to
                            args.SkipToPage = stepArgs.NextPage;

                            // If the SkipToPage is the same page being skipped, then their stuck on the page 
                            // that was supposed to be skipped.  Just go back to the page before this, which will
                            // look to the user like no page change, which is probably what is desired.
                            if (args.SkipToPage == page)
                            {
                                args.SkipToPage = CurrentPage;
                            }
                        }

                        if (args.SkipToPage == null)
                        {
                            throw new InvalidOperationException("Cannot Skip when SkipToPage is null.");
                        }

                        int newIndex = pages.IndexOf(args.SkipToPage);

                        return ShowPage(newIndex, reason);
                    }
                }
            }

            // Add the page to the main panel for display
            mainPanel.SuspendLayout();
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(page);
            page.Dock = DockStyle.Fill;
            mainPanel.ResumeLayout();

            // Update the header
            title.Text = page.Title;
            description.Text = page.Description;

            // Update the control buttons
            UpdateWizardButtons();

            // Now raise the shown event
            if (loaded && !DesignMode)
            {
                page.Focus();

                Refresh();
                page.RaisePageShown();
            }

            return page;
        }

        /// <summary>
        /// Update the state of the wizard buttons
        /// </summary>
        private void UpdateWizardButtons()
        {
            int index = CurrentIndex;
    
            // Nav buttons
            back.Enabled = back.Enabled && canCancel && index > 0;

            // Even setting this to the same value causes it to update the window.  This was causing flashing as the wizard initially opened.  Adding the
            // if condition fixed it.
            if (ControlBox != canCancel)
            {
                ControlBox = canCancel;
            }

            cancel.Visible = true;
            cancel.Enabled = canCancel;

            back.Visible = (index > 0);

            next.ShowShield = CurrentPage != null && CurrentPage.NextRequiresElevation;

            // Last page
            if (index == pages.Count - 1)
            {
                // Added the check for count > 1 to prevent flashing before OnLoad was complete and not all pages were present yet.
                if (!lastPageCancelable && pages.Count > 1)
                {
                    ControlBox = false;
                    cancel.Visible = false;
                    back.Visible = false;
                }

                next.Text = "Finish";
                next.DialogResult = FinishCancels ? DialogResult.Cancel : DialogResult.OK;
            }
            else
            {
                next.Text = "Next >";
                next.DialogResult = DialogResult.None;
            }
        }

        /// <summary>
        /// The Back button was pressed
        /// </summary>
        private void OnBack(object sender, System.EventArgs e)
        {
            if (CurrentPage == null)
                return;

            WizardPage backPage = GetPreviousPage(CurrentPage);

            if (backPage == null)
            {
                throw new InvalidOperationException(string.Format("No previous page coming from page '{0}'.", CurrentPage.Title));
            }

            // Remove the current page from our "Back" memory.  It will be added back in the next time it's navigated to
            backNavigation.Remove(CurrentPage);

            // Create the event args for the step
            WizardStepEventArgs args = new WizardStepEventArgs(backPage);

            if (!DesignMode)
            {
                // See if the current page is ok with going back
                CurrentPage.RaiseSteppingBack(args);
            }

            // Update the new index
            int newIndex = pages.IndexOf(args.NextPage);

            ShowPage(newIndex, WizardStepReason.StepBack);
        }

        /// <summary>
        /// Get the page that is before the specified page in the page collection or traversal history.
        /// </summary>
        private WizardPage GetPreviousPage(WizardPage page)
        {
            // Assume going back one page
            int newIndex = pages.IndexOf(page) - 1;

            // But if there is a memory, use it
            if (backNavigation.ContainsKey(page))
            {
                newIndex = pages.IndexOf(backNavigation[page]);
            }

            // Make sure the new index is valid
            if (newIndex < 0)
            {
                return null;
            }

            return pageCollection[newIndex];
        }

        /// <summary>
        /// The Next button was pressed
        /// </summary>
        private void OnNext(object sender, System.EventArgs e)
        {
            if (CurrentPage == null)
                return;

            int newIndex = pages.IndexOf(CurrentPage) + 1;

            WizardStepEventArgs args;

            // If its the last page, then we pass in null for NextPage
            if (newIndex >= pages.Count)
            {
                args = new WizardStepEventArgs(null);
            }

            else
            {
                // Create the event args for the step
                args = new WizardStepEventArgs(pageCollection[newIndex]);
            }

            if (!DesignMode)
            {
                // See if the current page is ok with going next
                CurrentPage.RaiseSteppingNext(args);
            }

            // If its the last page, then if NextPage is still null 
            // we just close
            if (newIndex >= pages.Count)
            {
                if (args.NextPage == null)
                {
                    // If a different result was explicitly set, leave it
                    if (DialogResult == DialogResult.None)
                    {
                        DialogResult = DialogResult.OK;
                    }

                    return;
                }
                else
                {
                    DialogResult = DialogResult.None;
                }
            }

            // Update the new index
            newIndex = pages.IndexOf(args.NextPage);

            WizardPage fromPage = CurrentPage;
            WizardPage shownPage = ShowPage(newIndex, WizardStepReason.StepForward);

            if (shownPage != fromPage)
            {
                // If its already in the back navigation, then we assume the wizard skipped backwards,
                // and we leave the back navigation as it would be if the user had clicked back as many 
                // times as it took to get to the shownPage.
                if (!backNavigation.ContainsKey(shownPage))
                {
                    backNavigation[shownPage] = fromPage;
                }
            }
        }

        /// <summary>
        /// The cancel button was pressed
        /// </summary>
        private void OnCancel(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// The wizard is trying to be closed.
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentPage == null)
                return;

            if (!DesignMode)
            {
                CancelEventArgs args = new CancelEventArgs(e.Cancel);

                if (!canCancel)
                {
                    args.Cancel = true;
                }

                // See if the current page is ok with going back
                CurrentPage.RaiseCancelling(args);

                e.Cancel = args.Cancel;
            }
        }

        /// <summary>
        /// Used by the designer to move to the next page
        /// </summary>
        public void MoveNext()
        {
            OnNext(next, EventArgs.Empty);
        }

        /// <summary>
        /// Used by the designer to move back a page
        /// </summary>
        public void MoveBack()
        {
            OnBack(back, EventArgs.Empty);
        }

        /// <summary>
        /// Used by the designer to set the current page
        /// </summary>
        internal void SetCurrent(int index)
        {
            ShowPage(index);
        }
    }
}