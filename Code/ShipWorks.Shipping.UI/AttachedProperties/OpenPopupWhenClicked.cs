using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace ShipWorks.Shipping.UI.AttachedProperties
{
    /// <summary>
    /// Attached property that will open a popup when the clickable control is clicked
    /// </summary>
    /// <remarks>
    /// When this is used on a hyperlink, it needs to be the only (or first) hyperlink in the textblock
    /// or the positioning will be incorrect.</remarks>
    public class OpenPopupWhenClicked : DependencyObject
    {
        /// <summary>
        /// Message type dependency property
        /// </summary>
        public static readonly DependencyProperty PopupProperty = DependencyProperty.RegisterAttached("Popup", typeof(Popup),
                typeof(OpenPopupWhenClicked), new PropertyMetadata(null, new PropertyChangedCallback(PopupChanged)));

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static Popup GetPopup(DependencyObject d) => (Popup)d.GetValue(PopupProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetPopup(DependencyObject d, Popup value) => d.SetValue(PopupProperty, value);

        /// <summary>
        /// Popup that should be open has changed
        /// </summary>
        private static void PopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                (e.NewValue as Popup).CustomPopupPlacementCallback = (popupSize, targetSize, offset) =>
                {
                    Visual element = d as Visual ?? GetClosestVisualAncestor(d);
                    Vector ancestorOffset = VisualTreeHelper.GetOffset(element);

                    return new[] { new CustomPopupPlacement(new Point(ancestorOffset.X, ancestorOffset.Y), PopupPrimaryAxis.None) };
                };
            }

            if (e.NewValue != null && e.OldValue == null)
            {
                AlterClickHandler(d, link => link.Click += OnClick, button => button.Click += OnClick);
            }
            if (e.NewValue == null && e.OldValue != null)
            {
                AlterClickHandler(d, link => link.Click -= OnClick, button => button.Click -= OnClick);
            }
        }

        /// <summary>
        /// Attach or detach the click handler
        /// </summary>
        private static void AlterClickHandler(DependencyObject d, Action<Hyperlink> updateHyperlinkHandler, Action<ButtonBase> updateButtonBaseHandler)
        {
            Hyperlink link = d as Hyperlink;
            if (link != null)
            {
                updateHyperlinkHandler(link);
                return;
            }

            ButtonBase button = d as ButtonBase;
            if (button != null)
            {
                updateButtonBaseHandler(button);
            }
        }

        /// <summary>
        /// Open the popup when the click event fires
        /// </summary>
        private static void OnClick(object sender, RoutedEventArgs e)
        {
            Popup popup = GetPopup(sender as DependencyObject);

            if (popup != null)
            {
                popup.IsOpen = true;
            }
        }

        /// <summary>
        /// Gets the closest ancestor that is a Visual
        /// </summary>
        private static Visual GetClosestVisualAncestor(DependencyObject d)
        {
            DependencyObject parent = GetParent(d);

            while (parent != null)
            {
                Visual element = parent as Visual;
                if (element != null)
                {
                    return element;
                }

                parent = GetParent(d);
            }

            return null;
        }

        /// <summary>
        /// Get the parent of the given object
        /// </summary>
        private static DependencyObject GetParent(DependencyObject d)
        {
            FrameworkContentElement contentElement = d as FrameworkContentElement;
            if (contentElement != null)
            {
                return contentElement.Parent;
            }

            Visual element = d as Visual;
            if (element != null)
            {
                return VisualTreeHelper.GetParent(element);
            }

            return null;
        }
    }
}
