using System.Drawing;
using System.Windows.Forms;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators
{
    /// <summary>
    /// Decorator that adds hyperlink capabilities to a column
    /// </summary>
    public class GridHyperlinkDecorator : GridColumnDisplayDecorator
    {
        /// <summary>
        /// Event raised when the hyperlink is clicked
        /// </summary>
        public event GridHyperlinkClickEventHandler LinkClicked;

        /// <summary>
        /// Raised to allow consumers control of when the hyperlink is enabled or drawn normally
        /// </summary>
        public event GridHyperlinkQueryEnabledEventHandler QueryEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridHyperlinkDecorator()
        {
            Identifier = "Hyperlink";
        }

        /// <summary>
        /// Controls if hyperlinking is enabled.  Value is the value that was returned from the DisplayType.GetEntityValue
        /// </summary>
        protected virtual bool IsHyperlinkEnabled(GridColumnFormattedValue formattedValue)
        {
            if (QueryEnabled != null)
            {
                GridHyperlinkQueryEnabledEventArgs args = new GridHyperlinkQueryEnabledEventArgs(formattedValue.Value, formattedValue.Entity);
                QueryEnabled(this, args);

                return args.Enabled;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Indicates if a hyperlink is present for the given row and column at the specifed point
        /// </summary>
        private bool IsHyperlinkAtLocation(EntityGridRow row, EntityGridColumn column, Point point)
        {
            GridColumnFormattedValue formattedValue = row.GetFormattedValue(column);

            return
                !string.IsNullOrEmpty(formattedValue.Text) &&
                IsHyperlinkEnabled(formattedValue) &&
                column.GetTextBounds(row, formattedValue).Contains(point);
        }

        /// <summary>
        /// Decorate the given value
        /// </summary>
        public override void ApplyDecoration(GridColumnFormattedValue formattedValue)
        {
            if (IsHyperlinkEnabled(formattedValue))
            {
                formattedValue.ForeColor = Color.Blue;
                formattedValue.FontStyle = FontStyle.Underline;
            }
        }

        /// <summary>
        /// Mouse is moving over a cell in our column.
        /// </summary>
        internal override void OnCellMouseMove(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {
            if (IsHyperlinkAtLocation(row, column, e.Location))
            {
                Cursor.Current = Cursors.Hand;
            }
        }

        /// <summary>
        /// Mouse is pressing down in a cell in our column.
        /// </summary>
        internal override bool OnCellMouseDown(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {
            if (IsHyperlinkAtLocation(row, column, e.Location))
            {
                Cursor.Current = Cursors.Hand;

                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Mouse is being released over a cell.
        /// </summary>
        internal override void OnCellMouseUp(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {
            if (IsHyperlinkAtLocation(row, column, e.Location))
            {
                // We have to POST this.  If we don't, and an action is taken in response like deleting the current row,
                // then we are in the middle of a cell ui activity and the row is deleted out from under.  That crashes.
                row.Grid.SandGrid.BeginInvoke((MethodInvoker) delegate
                {
                    OnLinkClicked(row, column, e);
                });
            }
        }

        /// <summary>
        /// The link for this column and the given row has been clicked.
        /// </summary>
        protected virtual void OnLinkClicked(EntityGridRow row, EntityGridColumn column, MouseEventArgs mouseArgs)
        {
            bool handled = false;

            GridHyperlinkClickEventArgs args = new GridHyperlinkClickEventArgs(row, column, mouseArgs);
            if (LinkClicked != null)
            {
                LinkClicked(this, args);
                handled = args.Handled;
            }

            // If its not handled yet pass it on to the grid
            if (!handled)
            {
                // Now tell the grid that something happened
                ((EntityGrid) row.Grid.SandGrid).OnGridCellLinkClicked(row, column, mouseArgs);
            }
        }
    }
}
