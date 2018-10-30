using System;
using System.Windows;
using System.Windows.Controls;
using DragDrop = GongSolutions.Wpf.DragDrop.DragDrop;

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Interaction logic for MainOrderLookupControl.xaml
    /// </summary>
    public partial class MainOrderLookupControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainOrderLookupControl()
        {
            InitializeComponent();

            // the only reason for this to be in here is to force the gong dll to be
            // included in the project.
            System.Windows.DataFormat _ = DragDrop.DataFormat;
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
