using System;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// The main ScanPack control
    /// </summary>
    public partial class ScanPackControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update column widths when control size changes
        /// </summary>
        private void OnControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnWidths();
        }

        /// <summary>
        /// Update column widths when column grid size changes
        /// </summary>
        private void OnGridControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnWidths();
        }

        /// <summary>
        /// Update column widths when the grid splitter drag is completed
        /// </summary>
        private void OnGridSplitterDragComplete(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            UpdateColumnWidths();
        }

        /// <summary>
        /// Update the column widths so that a single column doesn't extend past the screen
        /// </summary>
        private void UpdateColumnWidths()
        {
            double columnMaxWidth = Math.Min(ColumnGrid.ActualWidth, ColumnGrid.MaxWidth) - (2 * (LeftColumn.MinWidth + 10));

            LeftColumn.MaxWidth = columnMaxWidth;
            MiddleColumn.MaxWidth = columnMaxWidth;
            RightColumn.MaxWidth = columnMaxWidth;
        }
    }
}
