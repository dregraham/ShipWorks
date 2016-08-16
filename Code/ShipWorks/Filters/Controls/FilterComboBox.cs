using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Core.Messaging;
using ShipWorks.Filters.Management;
using ShipWorks.UI.Controls;
using System.Windows.Forms;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid.Rendering;
using ShipWorks.Filters;
using ShipWorks.Users;
using System.Drawing.Imaging;
using System.ComponentModel;
using Interapptive.Shared;
using ShipWorks.UI;
using ShipWorks.Properties;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Custom combo box for displaying 
    /// </summary>
    public class FilterComboBox : PopupComboBox
    {
        PopupController popupController;

        FilterTree filterTree;

        FilterNodeEntity selectedNode;
        byte[] selectedFilterVersion = null;
        byte[] selectedNodeVersion = null;
        FilterCount selectedNodeCount = null;

        TextFormattingInformation textFormat;

        bool sizeToContent = false;
        string sizeToContentLastName = string.Empty;

        public event EventHandler SelectedFilterNodeChanged;

        Image countingImageNormal = Resources.arrows_blue;
        Image countingImageSelected = Resources.arrows_white;
        bool isAnimating = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterComboBox()
        {
            textFormat = new TextFormattingInformation();
            textFormat.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            textFormat.TextFormatFlags = TextFormatFlags.Top;

            // Create the filter tree that will be popped up
            filterTree = new FilterTree();
            filterTree.BorderStyle = BorderStyle.None;
            filterTree.HotTracking = true;
            filterTree.AutoRefreshCalculatingCounts = true;
            
            // When it becomes visible we want to move the selected item into view
            filterTree.VisibleChanged += new EventHandler(OnFilterTreeVisibleChanged);

            // Create the drop-down
            popupController = new PopupController(filterTree);
            popupController.Animate = PopupAnimation.System;
            popupController.PopupClosing += new EventHandler(OnPopupClosing);

            // The filter is what we are going to be dropping down
            this.PopupController = popupController;
        }

        /// <summary>
        /// Load the tree that will display the filters
        /// </summary>
        public void LoadLayouts(params FilterTarget[] targets)
        {
            filterTree.SelectedFilterNodeChanged -= new EventHandler(OnFilterSelected);
            filterTree.LoadLayouts(targets);
            filterTree.SelectedFilterNodeChanged += new EventHandler(OnFilterSelected);

            filterTree.ApplyFolderState(new FolderExpansionState(UserSession.User.Settings.OrderFilterExpandedFolders));
        }

        /// <summary>
        /// Controls if the user can choose a Quick Filter, or just the default standard filters
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool AllowQuickFilter
        {
            get
            {
                return filterTree.AllowQuickFilter;
            }
            set
            {
                filterTree.AllowQuickFilter = value;
            }
        }


        /// <summary>
        /// Indicates if the active search node - if any - will be displayed in the grid.
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FilterNodeEntity ActiveSearchNode
        {
            get
            {
                return filterTree.ActiveSearchNode;
            }
            set
            {
                filterTree.ActiveSearchNode = value;
            }
        }

        /// <summary>
        /// The currently selected filter node ID
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SelectedFilterNodeID
        {
            get
            {
                if (selectedNode != null)
                {
                    return selectedNode.FilterNodeID;
                }

                return 0;
            }
            set
            {
                filterTree.SelectedFilterNodeChanged -= new EventHandler(OnFilterSelected);
                filterTree.SelectedFilterNodeID = value;
                filterTree.SelectedFilterNodeChanged += new EventHandler(OnFilterSelected);

                if (SelectedFilterNodeID == filterTree.SelectedFilterNodeID)
                {
                    return;
                }

                selectedNode = filterTree.SelectedFilterNode;

                UpdateSize();

                if (SelectedFilterNodeChanged != null)
                {
                    SelectedFilterNodeChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }
        /// <summary>
        /// The currently selected filter node
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FilterNodeEntity SelectedFilterNode
        {
            get
            {
                return selectedNode;
            }
            set
            {
                filterTree.SelectedFilterNodeChanged -= new EventHandler(OnFilterSelected);
                filterTree.SelectedFilterNode = value;
                filterTree.SelectedFilterNodeChanged += new EventHandler(OnFilterSelected);

                if (SelectedFilterNodeID == filterTree.SelectedFilterNodeID)
                {
                    if (filterTree.SelectedFilterNode == null)
                    {
                        return;
                    }

                    // See if the Filter (which has the Name) or the Node (which has the definition) has any changes, or the count.  If not, get out
                    if (ByteUtility.AreEqual(selectedNodeVersion, filterTree.SelectedFilterNode.RowVersion) &&
                        ByteUtility.AreEqual(selectedFilterVersion, filterTree.SelectedFilterNode.Filter.RowVersion) &&
                        selectedNodeCount == FilterContentManager.GetCount(filterTree.SelectedFilterNodeID))
                    {
                        return;
                    }
                }

                selectedNode = filterTree.SelectedFilterNode;

                // So we can tell not only when the selected node has changed - but also if it has been edited
                if (selectedNode != null)
                {
                    selectedNodeVersion = selectedNode.RowVersion;
                    selectedFilterVersion = selectedNode.Filter.RowVersion;
                    selectedNodeCount = FilterContentManager.GetCount(filterTree.SelectedFilterNodeID);
                }
                else
                {
                    selectedNodeVersion = null;
                    selectedFilterVersion = null;
                    selectedNodeCount = null;
                }

                UpdateSize();

                if (SelectedFilterNodeChanged != null)
                {
                    SelectedFilterNodeChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is selected filter disabled.
        /// </summary>
        public bool IsSelectedFilterDisabled
        {
            get
            {
                if (selectedNode != null)
                {
                    return selectedNode.Filter.State == (byte)FilterState.Disabled;                    
                }

                return false;
            }
        }

        /// <summary>
        /// When a filter is selected, we close the drop down
        /// </summary>
        private void OnFilterSelected(object sender, EventArgs e)
        {
            SelectedFilterNode = filterTree.SelectedFilterNode;

            // And we can close the drop down
            if (popupController.IsPopupVisible)
            {
                popupController.Close(DialogResult.OK);
            }
        }

        /// <summary>
        /// Select the first node in the tree, whatever it is
        /// </summary>
        public void SelectFirstNode()
        {
            filterTree.SelectFirstNode();
        }

        /// <summary>
        /// If a Quick Filter had been created and is present, selected or not, clear it.
        /// </summary>
        public void ClearQuickFilter()
        {
            filterTree.ClearQuickFilter();
        }

        /// <summary>
        /// If true, the ComboBox will auto-size itself to fit whatever text is displayed in its text area.
        /// </summary>
        [DefaultValue(false)]
        public bool SizeToContent
        {
            get
            {
                return sizeToContent;
            }
            set
            {
                sizeToContent = value;

                UpdateSize();
            }
        }

        /// <summary>
        /// Update the size based on the Size To Content setting
        /// </summary>
        private void UpdateSize()
        {
            if (sizeToContent)
            {
                if (selectedNode != null)
                {
                    sizeToContentLastName = selectedNode.Filter.Name;

                    string text = selectedNode.Filter.Name;

                    if (selectedNode != null)
                    {
                        text += GetCountText(selectedNode);
                    }

                    using (Graphics g = CreateGraphics())
                    {
                        SizeF size = g.MeasureString(text, Font);
                        Width = Math.Max(100, (int) size.Width + SystemInformation.VerticalScrollBarWidth + 40);
                    }
                }
                else
                {
                    Width = 140;
                }
            }
        }

        /// <summary>
        /// The visibility of the tree has changed
        /// </summary>
        void OnFilterTreeVisibleChanged(object sender, EventArgs e)
        {
            if (filterTree.Visible)
            {
                if (selectedNode != null)
                {
                    filterTree.EnsureNodeVisible(selectedNode);
                }

                filterTree.SelectedFilterNodeChanged -= new EventHandler(OnFilterSelected);
                filterTree.SelectedFilterNode = null;
                filterTree.SelectedFilterNodeChanged += new EventHandler(OnFilterSelected);

                if (selectedNode != null)
                {
                    filterTree.HotTrackNode = selectedNode;
                }
                else
                {
                    filterTree.HotTrackNode = null;
                }
            }
        }

        /// <summary>
        /// The popup is closing
        /// </summary>
        void OnPopupClosing(object sender, EventArgs e)
        {
            // Save the folder state (could have crashed with it open - check to be sure)
            if (UserSession.IsLoggedOn)
            {
                UserSession.User.Settings.OrderFilterExpandedFolders = filterTree.GetFolderState().GetState();

                // If what we think is the selected node is no longer present in the tree then the user removed
                // it from the tree but never selected a new one.
                if (selectedNode != null && !filterTree.IsFilterNodeAvailable(selectedNode.FilterNodeID))
                {
                    filterTree.SelectFirstNode();
                }
            }
        }

        /// <summary>
        /// Get the ideal size of the popup
        /// </summary>
        protected override Size GetIdealPopupSize()
        {
            // Have to make sure its all set before calculating the size
            if (selectedNode != null)
            {
                filterTree.EnsureNodeVisible(selectedNode);
            }

            return filterTree.IdealSize;
        }

        /// <summary>
        /// Draw the selected filter
        /// </summary>
        [NDependIgnoreLongMethod]
        protected override void OnDrawSelectedItem(Graphics g, Color foreColor, Rectangle bounds)
        {
            bool selected = false;
            
            if (selectedNode != null)
            {
                if (sizeToContentLastName != selectedNode.Filter.Name)
                {
                    UpdateSize();
                }

                bounds.Offset(1, 0);
                Image image = FilterHelper.GetFilterImage(selectedNode, false);

                if (Enabled)
                {
                    g.DrawImage(image, bounds.Left, bounds.Top, image.Width, image.Height);
                }
                else
                {
                    ControlPaint.DrawImageDisabled(g, image, bounds.Left, bounds.Top, BackColor);
                }

                int imageTextSeparation = 5;
                bounds.Offset(image.Width + imageTextSeparation, 1);
                bounds.Width -= (image.Width + imageTextSeparation);

                Font itemFont = new Font(Font, Font.Style);
                Color itemColor = foreColor;
                
                DisabledFilterFont disabledFont = new DisabledFilterFont(Font);
                bool isFilterDisabled = selectedNode.Filter.State == (byte)FilterState.Disabled;
                if (isFilterDisabled)
                {
                    itemFont = disabledFont.Font;
                    itemColor = disabledFont.TextColor;
                }

                IndependentText.DrawText(g, selectedNode.Filter.Name, itemFont, bounds, textFormat, itemColor);

                string countText = GetCountText(selectedNode);

                // If we are animating, but now have a count, then we need to raise the changed event to notify consumers that there has
                // been an update.  Otherwise they will not know that the filter has gone from unknown to having an actual count.
                if (isAnimating && countText != null)
                {
                    BeginInvoke(new MethodInvoker(() => { SelectedFilterNode = selectedNode; }));
                }

                // Enable\disable animation based on if we have a count
                Animate(countText == null);

                Color countColor = (selected || !Enabled) ? foreColor : Color.Blue;

                if (isFilterDisabled)
                {
                    countColor = disabledFont.TextColor;
                }

                // How big the text is drawn by default
                Size size = IndependentText.MeasureText(g, selectedNode.Filter.Name, Font, textFormat);

                Rectangle countBounds = bounds;
                countBounds.X += size.Width;
                countBounds.Width -= size.Width;

                if (countText != null)
                {
                    IndependentText.DrawText(g, countText, itemFont, countBounds, textFormat, countColor);
                }
                else
                {
                    IndependentText.DrawText(g, "(", Font, countBounds, textFormat, countColor);
                    countBounds.Offset(4, 0);

                    Image countingImage = selected ? countingImageSelected : countingImageNormal;

                    g.DrawImage(countingImage, countBounds.Left, countBounds.Top + 2, 10, 10);
                    countBounds.Offset(10, 0);

                    IndependentText.DrawText(g, ")", Font, countBounds, textFormat, countColor);
                }
            }
        }

        /// <summary>
        /// Get the text to display for the filter count
        /// </summary>
        private string GetCountText(FilterNodeEntity node)
        {
            FilterCount count = FilterContentManager.GetCount(node.FilterNodeID);

            string countText = null;

            if (count != null)
            {
                if (count.Status == FilterCountStatus.Ready)
                {
                    countText = string.Format("({0:#,##0})", count.Count);
                }
            }

            return countText;
        }

        /// <summary>
        /// Start or stop animation
        /// </summary>
        private void Animate(bool animate)
        {
            if (animate && !isAnimating)
            {
                isAnimating = true;
                ImageAnimator.Animate(countingImageNormal, new EventHandler(OnFrameChanged));
                ImageAnimator.Animate(countingImageSelected, new EventHandler(OnFrameChanged));
            }

            if (!animate && isAnimating)
            {
                isAnimating = false;
                ImageAnimator.StopAnimate(countingImageNormal, new EventHandler(OnFrameChanged));
                ImageAnimator.StopAnimate(countingImageSelected, new EventHandler(OnFrameChanged));
            }
        }

        /// <summary>
        /// The next frame is ready to be drawn
        /// </summary>
        private void OnFrameChanged(object o, EventArgs e)
        {
            if (InvokeRequired)
            {
                Program.MainForm.BeginInvoke(new EventHandler(OnFrameChanged), o, e);
                return;
            }

            if (!isAnimating)
            {
                return;
            }

            ImageAnimator.UpdateFrames(countingImageNormal);
            ImageAnimator.UpdateFrames(countingImageSelected);
            Invalidate();
        }

        /// <summary>
        /// Disposing
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            Animate(false);

            if (filterTree != null)
            {
                filterTree.Dispose();
                filterTree = null;
            }

            base.Dispose(disposing);
        }
    }
}
