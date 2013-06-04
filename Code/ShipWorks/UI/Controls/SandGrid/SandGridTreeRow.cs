using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Filters.Controls;
using System.Drawing;
using Divelements.SandGrid.Rendering;

namespace ShipWorks.UI.Controls.SandGrid
{
    /// <summary>
    /// Customized grid row 
    /// </summary>
    public abstract class SandGridTreeRow : SandGridDragDropRow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected SandGridTreeRow()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected SandGridTreeRow(string text, Image image)
            : base(text, image)
        {

        }

        /// <summary>
        /// Indicates if the given point is in the +\- expansion area for the row
        /// </summary>
        public bool IsPointInExpansionArea(Point hit)
        {
            return base.CalculateExpandButtonBounds(Grid.PrimaryColumn).Contains(hit);
        }

        /// <summary>
        /// Get the next element, used for keyboard navigation.
        /// </summary>
        public override FocusableGridElement GetNextElement(FocusAdvanceDirection direction, bool loop, out bool exposedFurtherElements)
        {
            FocusableGridElement element = base.GetNextElement(direction, loop, out exposedFurtherElements);
            FocusableGridElement first = element;

            bool foldersOnly = ((SandGridTree) Grid.SandGrid).SelectFoldersOnly;

            if (!foldersOnly)
            {
                return element;
            }

            while (element != null)
            {
                SandGridTreeRow row = (SandGridTreeRow) element;

                if (row.IsFolder)
                {
                    return element;
                }

                element = row.GetNextElement(direction, loop, out exposedFurtherElements);

                if (element == first)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
