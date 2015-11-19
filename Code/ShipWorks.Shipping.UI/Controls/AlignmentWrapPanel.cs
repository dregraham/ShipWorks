using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.Collections;

namespace ShipWorks.Shipping.UI.Controls
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
            Size size = new Size();
            double startY = 0;

            foreach (IEnumerable<UIElement> currentElementLine in
                InternalChildren.OfType<UIElement>().SplitIntoChunks(x => x.DesiredSize.Width, finalSize.Width))
            {
                double lineHeight = currentElementLine.Max(x => x.DesiredSize.Height);

                double leftOffset = ArrangeLeftAlignedElements(currentElementLine, startY, finalSize.Width, lineHeight);
                double rightOffset = ArrangeRightAlignedElements(currentElementLine, startY, finalSize.Width, lineHeight);
                ArrangeCenterAlignedElements(currentElementLine, startY, leftOffset, rightOffset, lineHeight);

                startY += lineHeight;

                size.Width = Math.Max(size.Width, currentElementLine.Max(x => x.DesiredSize.Width));
                size.Height += lineHeight;
            }

            return size;
        }

        /// <summary>
        /// Measure the desired size of the panel
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            return InternalChildren.OfType<UIElement>()
                .Select(element =>
                {
                    element.Measure(constraint);
                    return element.DesiredSize;
                })
                .SplitIntoChunks(x => x.Width, constraint.Width)
                .Select(rowSizes => new Size(rowSizes.Sum(x => x.Width), rowSizes.Max(x => x.Height)))
                .Aggregate(new Size(), (x, y) => new Size(Math.Max(x.Width, y.Width), x.Height + y.Height));






            //Size currentLineSize = new Size();
            //Size panelSize = new Size();

            //foreach (UIElement element in InternalChildren.OfType<UIElement>())
            //{
            //    element.Measure(constraint);
            //    Size desiredSize = element.DesiredSize;

            //    if (currentLineSize.Width + desiredSize.Width > constraint.Width)
            //    {
            //        panelSize.Width = Math.Max(currentLineSize.Width, panelSize.Width);
            //        panelSize.Height += currentLineSize.Height;

            //        if (desiredSize.Width > constraint.Width)
            //        {
            //            panelSize.Width = Math.Max(desiredSize.Width, panelSize.Width);
            //            panelSize.Height += desiredSize.Height;
            //            currentLineSize = new Size();
            //        }
            //        else
            //        {
            //            currentLineSize = new Size(desiredSize.Width, desiredSize.Height);
            //        }
            //    }
            //    else
            //    {
            //        currentLineSize.Width += desiredSize.Width;
            //        currentLineSize.Height = Math.Max(desiredSize.Height, currentLineSize.Height);
            //    }
            //}

            //panelSize.Width = double.IsPositiveInfinity(constraint.Width) ?
            //    Math.Max(currentLineSize.Width, panelSize.Width) :
            //    constraint.Width;
            //panelSize.Height += currentLineSize.Height;
            //return panelSize;
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

            foreach (UIElement element in elements.Where(IsLeftOrStretchAlignment))
            {
                element.Arrange(new Rect(startX, startY, width, height));
                startX += element.DesiredSize.Width;
            }

            return startX;
        }

        /// <summary>
        /// Is the element center aligned
        /// </summary>
        private bool IsCenterAlignment(UIElement element)
        {
            HorizontalAlignment alignment = (HorizontalAlignment)element.GetValue(HorizontalAlignmentProperty);
            return alignment == HorizontalAlignment.Center;
        }

        /// <summary>
        /// Is the element right aligned
        /// </summary>
        private bool IsRightAlignment(UIElement element)
        {
            HorizontalAlignment alignment = (HorizontalAlignment)element.GetValue(HorizontalAlignmentProperty);
            return alignment == HorizontalAlignment.Right;
        }

        /// <summary>
        /// Is the element left or stretch aligned
        /// </summary>
        private bool IsLeftOrStretchAlignment(UIElement element)
        {
            HorizontalAlignment alignment = (HorizontalAlignment)element.GetValue(HorizontalAlignmentProperty);
            return alignment == HorizontalAlignment.Left || alignment == HorizontalAlignment.Stretch;
        }
    }
}
