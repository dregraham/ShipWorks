using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.Collections;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Wrap panel that respects children's alignment properties
    /// </summary>
    public class AlignmentWrapPanel : Panel
    {
        /// <summary>
        /// Arrange the children
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Size size = InternalChildren.OfType<UIElement>()
                .SplitIntoChunks(x => x.DesiredSize.Width, finalSize.Width)
                .Aggregate(new Size(), (currentSize, row) => ArrangeRow(row, currentSize, finalSize.Width));

            return AdjustSizeForConstraintAndAlignment(size, finalSize);
        }

        /// <summary>
        /// Measure the desired size of the panel
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            Size size = InternalChildren.OfType<UIElement>()
                .Select(x => MeasureElement(x, constraint))
                .SplitIntoChunks(x => x.Width, constraint.Width)
                .Select(rowSizes => new Size(rowSizes.Sum(x => x.Width), rowSizes.Max(x => x.Height)))
                .Aggregate(new Size(), (x, y) => new Size(Math.Max(x.Width, y.Width), x.Height + y.Height));

            return AdjustSizeForConstraintAndAlignment(size, constraint);
        }

        /// <summary>
        /// Arrange the layout of a single row
        /// </summary>
        private Size ArrangeRow(IEnumerable<UIElement> currentElementLine, Size size, double maxWidth)
        {
            double lineHeight = currentElementLine.Max(x => x.DesiredSize.Height);

            double leftOffset = ArrangeLeftAlignedElements(currentElementLine, size.Height, maxWidth, lineHeight);
            double rightOffset = ArrangeRightAlignedElements(currentElementLine, size.Height, maxWidth, lineHeight);
            ArrangeCenterAlignedElements(currentElementLine, size.Height, leftOffset, rightOffset, lineHeight);

            return new Size(
                Math.Max(size.Width, currentElementLine.Sum(x => x.DesiredSize.Width)),
                size.Height + lineHeight);
        }

        /// <summary>
        /// Adjust the given size based on the constraint and alignment of the control
        /// </summary>
        private Size AdjustSizeForConstraintAndAlignment(Size size, Size constraint)
        {
            double width = IsStretchAlignment(this) && !double.IsPositiveInfinity(constraint.Width) ?
                constraint.Width : size.Width;

            return new Size(width, size.Height);
        }

        /// <summary>
        /// Measure the size of an element with the given constraint
        /// </summary>
        private Size MeasureElement(UIElement element, Size constraint)
        {
            element.Measure(constraint);
            return element.DesiredSize;
        }

        /// <summary>
        /// Arrange all elements in the line that should be centered
        /// </summary>
        private void ArrangeCenterAlignedElements(IEnumerable<UIElement> elements, double startY, double leftOffset, double rightOffset, double height)
        {
            List<UIElement> centeredElements = elements.Where(IsCenterAlignment).ToList();
            double centeredWidth = centeredElements.Sum(x => x.DesiredSize.Width);

            if (centeredWidth < 0.1)
            {
                return;
            }

            double newLeftOffset = leftOffset;
            double availableWidth = rightOffset - newLeftOffset;

            foreach (UIElement element in centeredElements)
            {
                double relativeWidth = availableWidth * (element.DesiredSize.Width / centeredWidth);

                element.Arrange(new Rect(newLeftOffset, startY, relativeWidth, height));
                newLeftOffset += relativeWidth;
            }
        }

        /// <summary>
        /// Arrange all elements in the line that should be right aligned
        /// </summary>
        private double ArrangeRightAlignedElements(IEnumerable<UIElement> elements, double startY, double startWidth, double height)
        {
            double width = startWidth;

            foreach (UIElement element in elements.Where(IsRightAlignment).Reverse())
            {
                element.Arrange(new Rect(0, startY, width, height));
                width = Math.Max(width - element.DesiredSize.Width, 0);
            }

            return width;
        }

        /// <summary>
        /// Arrange all elements in the line that should be left aligned
        /// </summary>
        private double ArrangeLeftAlignedElements(IEnumerable<UIElement> elements, double startY, double width, double height)
        {
            double startX = 0;

            foreach (UIElement element in elements.Where(x => IsLeftAlignment(x) || IsStretchAlignment(x)))
            {
                element.Arrange(new Rect(startX, startY, width, height));
                startX += element.DesiredSize.Width;
            }

            return startX;
        }

        /// <summary>
        /// Is the element center aligned
        /// </summary>
        private bool IsCenterAlignment(UIElement element) =>
            GetHorizontalAlignment(element) == HorizontalAlignment.Center;

        /// <summary>
        /// Is the element right aligned
        /// </summary>
        private bool IsRightAlignment(UIElement element) =>
            GetHorizontalAlignment(element) == HorizontalAlignment.Right;

        /// <summary>
        /// Is the element left aligned
        /// </summary>
        private bool IsLeftAlignment(UIElement element) =>
            GetHorizontalAlignment(element) == HorizontalAlignment.Left;

        /// <summary>
        /// Is the element stretch aligned
        /// </summary>
        private bool IsStretchAlignment(UIElement element) =>
            GetHorizontalAlignment(element) == HorizontalAlignment.Stretch;

        /// <summary>
        /// Get the horizontal alignment of an element
        /// </summary>
        private HorizontalAlignment GetHorizontalAlignment(UIElement element) =>
            (HorizontalAlignment) element.GetValue(HorizontalAlignmentProperty);
    }
}
