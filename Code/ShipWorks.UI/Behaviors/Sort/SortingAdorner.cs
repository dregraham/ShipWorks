using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ShipWorks.UI.Behaviors.Sort
{
    /// <summary>
    /// Adorns header with up or down arrow to indicate sorting direction
    /// </summary>
    public class SortingAdorner : Adorner
    {
        private static readonly Geometry arrowUp = Geometry.Parse("M 5,5 15,5 10,0 5,5");
        private static readonly Geometry arrowDown = Geometry.Parse("M 5,0 10,5 15,0 5,0");
        private readonly Geometry sortDirection;
        private const int glyphWidth = 20;

        /// <summary>
        /// Constructor
        /// </summary>
        public SortingAdorner(UIElement adornedElement, ListSortDirection sortDirection)
            : base(adornedElement)
        {
            this.sortDirection = sortDirection == ListSortDirection.Ascending ? arrowUp : arrowDown;
        }

        /// <summary>
        /// Rendering instructions for up and down arrow
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            double offsetX = AdornedElement.RenderSize.Width - glyphWidth;
            double offsetY = (AdornedElement.RenderSize.Height - 5) / 2;

            if (offsetX >= glyphWidth)
            {
                // Right order of the statements is important
                drawingContext.PushTransform(new TranslateTransform(offsetX, offsetY));
                drawingContext.DrawGeometry(Brushes.Black, null, sortDirection);
                drawingContext.Pop();
            }
        }
    }
}