using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.UI.Utility;
using ShipWorks.Filters;
using Divelements.SandGrid;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Base user control for controls that display the child details of an order
    /// </summary>
    public partial class PanelControlBase : UserControl, IDockingPanelContent
    {
        Guid settingsKey;
        GridColumnDefinitionSet definitionSet;
        Action<GridColumnLayout> layoutInitializer;
        bool detailViewEnabled = false;

        Size idealSize;

        /// <summary>
        /// The IdealSize of the control has changed when this is raised
        /// </summary>
        public event EventHandler IdealSizeChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public PanelControlBase()
        {
            InitializeComponent();

            entityGrid.Renderer = new WindowsXPShipWorksRenderer();
            ThemedBorderProvider.Apply(this);
        }

        /// <summary>
        /// The ideal size of the control where everything would be visible
        /// </summary>
        public Size IdealSize
        {
            get
            {
                return idealSize;
            }
        }

        /// <summary>
        /// Controls if detail view is allowed.  Should be set in the constructor.
        /// </summary>
        protected bool DetailViewEnabled
        {
            get { return detailViewEnabled; }
            set { detailViewEnabled = value; }
        }

        /// <summary>
        /// Initialize the control with the given settings and default column layout info.
        /// </summary>
        public virtual void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            // Prepare the grid
            entityGrid.InitializeGrid();

            // Save the layout creator for when we load columns
            this.settingsKey = settingsKey;
            this.definitionSet = definitionSet;
            this.layoutInitializer = layoutInitializer;
        }

        /// <summary>
        /// Load the column state
        /// </summary>
        public virtual void LoadState()
        {
            StandardGridColumnStrategy columnStrategy = new StandardGridColumnStrategy(settingsKey, definitionSet, layoutInitializer);
            columnStrategy.DetailViewEnabled = detailViewEnabled;

            entityGrid.InitializeColumns(columnStrategy);
        }

        /// <summary>
        /// Save the column state
        /// </summary>
        public void SaveState()
        {
            entityGrid.SaveColumns();
        }

        /// <summary>
        /// The primary EntityType displayed by the panel\grid
        /// </summary>
        public virtual EntityType EntityType
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public virtual FilterTarget[] SupportedTargets
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Must be overridden by derived type
        /// </summary>
        public virtual bool SupportsMultiSelect
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Must be overridden by derived type. Completely reloads the grid based on the given selection.
        /// </summary>
        public virtual Task ChangeContent(IGridSelection selection)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row 
        /// list with up-to-date displayed entity content.
        /// </summary>
        public virtual Task ReloadContent()
        {
            entityGrid.ReloadGridRows();
            return TaskEx.FromResult(true);
        }

        /// <summary>
        /// Refresh the existing displayed content.  Does not try to reset or look for new\deleted rows - just refreshes
        /// the known existing rows and their known corresponding entities.
        /// </summary>
        public virtual Task UpdateContent()
        {
            entityGrid.UpdateGridRows();
            return TaskEx.FromResult(true);
        }

        /// <summary>
        /// Indicates if the footer control section should be visible
        /// </summary>
        protected virtual bool ShowFooterControls
        {
            get { return addLink.Visible; }
        }

        /// <summary>
        /// Update the content based on the current store set
        /// </summary>
        public virtual void UpdateStoreDependentUI()
        {

        }

        /// <summary>
        /// Utility method to reload the content with the given key
        /// </summary>
        public void ChangeContent(long key)
        {
            ChangeContent(new StaticGridSelection(new List<long> { key }));
        }

        /// <summary>
        /// Row was double-clicked
        /// </summary>
        private void OnRowActivated(object sender, GridRowEventArgs e)
        {
            long? entityID = ((EntityGridRow) e.Row).EntityID;
            if (entityID != null)
            {
                OnEntityDoubleClicked(entityID.Value);
            }
        }

        /// <summary>
        /// An entity has been double clicked
        /// </summary>
        protected virtual void OnEntityDoubleClicked(long entityID)
        {

        }

        /// <summary>
        /// The minimum height for displaying the grid without a scrollbar has changed
        /// </summary>
        private void OnGridMinimumSizeChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        /// <summary>
        /// Update the layout as the size of the control changes
        /// </summary>
        private void OnResize(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        /// <summary>
        /// Update the layout of the grid and the Add Note link
        /// </summary>
        protected virtual void UpdateLayout()
        {
            int columnHeight = entityGrid.Columns.DisplayColumns.Length > 0 ? entityGrid.Columns.DisplayColumns[0].Bounds.Height : 20;
            int addHeight = ShowFooterControls ? 10 + addLink.Height : 0;
            int gridHeight = Math.Max(columnHeight, Math.Min(entityGrid.MinimumNoScrollSize.Height, Height - addHeight));

            entityGrid.Height = gridHeight;
            addLink.Top = entityGrid.Bottom + 5;

            int right = Math.Min(entityGrid.Width, entityGrid.MinimumNoScrollSize.Width);
            addLink.Left = Math.Max(4, right - 4 - addLink.Width);

            // Make sure our ideal size is up to date
            UpdateIdealSize();
        }

        /// <summary>
        /// Update the IdealSize and raise event if necessary
        /// </summary>
        protected virtual void UpdateIdealSize()
        {
            int idealHeight = entityGrid.MinimumNoScrollSize.Height + addLink.Height + 10;

            // We add 10 here for a hacky reason.  When a grid column is resized, if there isn't some extra space, then it flashes a
            // scrollbar real quick before the parent container can make this ChildDetailControlBase big enough.  But if its got 10 extra
            // pixels buffer, it's usually OK.
            int idealWidth = entityGrid.MinimumNoScrollSize.Width + 10;

            if (idealSize.Width != idealWidth || idealSize.Height != idealHeight)
            {
                idealSize = new Size(idealWidth, idealHeight);

                if (IdealSizeChanged != null)
                {
                    IdealSizeChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Custom painting of the control
        /// </summary>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (addLink.Visible && entityGrid.Height < entityGrid.MinimumNoScrollSize.Height)
            {
                using (Pen pen = entityGrid.Renderer.CreateGridLinePen())
                {
                    e.Graphics.DrawLine(pen, 0, entityGrid.Bottom + 1, Width, entityGrid.Bottom + 1);
                }
            }
        }
    }
}
