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
    public class PopupClickManager : DependencyObject
    {
        /// <summary>
        /// Message type dependency property
        /// </summary>
        public static readonly DependencyProperty OpenPopupProperty = DependencyProperty.RegisterAttached("OpenPopup", typeof(Popup),
                typeof(PopupClickManager), new PropertyMetadata(null, new PropertyChangedCallback(OpenPopupChanged)));

        /// <summary>
        /// Message type dependency property
        /// </summary>
        public static readonly DependencyProperty ClosePopupProperty = DependencyProperty.RegisterAttached("ClosePopup", typeof(Popup),
                typeof(PopupClickManager), new PropertyMetadata(null, new PropertyChangedCallback(ClosePopupChanged)));

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static Popup GetOpenPopup(DependencyObject d) => (Popup)d.GetValue(OpenPopupProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetOpenPopup(DependencyObject d, Popup value) => d.SetValue(OpenPopupProperty, value);

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static Popup GetClosePopup(DependencyObject d) => (Popup)d.GetValue(ClosePopupProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetClosePopup(DependencyObject d, Popup value) => d.SetValue(ClosePopupProperty, value);

        /// <summary>
        /// Popup that should be open has changed
        /// </summary>
        private static void OpenPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                Popup popup = e.NewValue as Popup;
                if (popup != null)
                {
                    popup.Placement = PlacementMode.Bottom;
                    popup.PlacementTarget = GetClosestVisualAncestor<UIElement>(d);
                }
            }

            if (e.NewValue != null && e.OldValue == null)
            {
                AlterClickHandler(d, link => link.Click += OnOpenPopupClick, button => button.Click += OnOpenPopupClick, element => element.MouseUp += OnOpenPopupClick);
            }
            if (e.NewValue == null && e.OldValue != null)
            {
                AlterClickHandler(d, link => link.Click -= OnOpenPopupClick, button => button.Click -= OnOpenPopupClick, element => element.MouseUp -= OnOpenPopupClick);
            }
        }

        /// <summary>
        /// Popup that should be Close has changed
        /// </summary>
        private static void ClosePopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && e.OldValue == null)
            {
                AlterClickHandler(d, link => link.Click += OnClosePopupClick, button => button.Click += OnClosePopupClick, element => element.MouseUp += OnClosePopupClick);
            }
            if (e.NewValue == null && e.OldValue != null)
            {
                AlterClickHandler(d, link => link.Click -= OnClosePopupClick, button => button.Click -= OnClosePopupClick, element => element.MouseUp -= OnClosePopupClick);
            }
        }

        /// <summary>
        /// Attach or detach the click handler
        /// </summary>
        private static void AlterClickHandler(DependencyObject d, Action<Hyperlink> updateHyperlinkHandler, Action<ButtonBase> updateButtonBaseHandler, Action<UIElement> updateElementHandler)
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
                return;
            }

            UIElement element = d as UIElement;
            if (element != null) {
                updateElementHandler(element);
            }
        }

        /// <summary>
        /// Open the popup when the click event fires
        /// </summary>
        private static void OnOpenPopupClick(object sender, RoutedEventArgs e)
        {
            Popup popup = GetOpenPopup(sender as DependencyObject);

            if (popup != null)
            {
                popup.IsOpen = true;
            }
        }

        /// <summary>
        /// Open the popup when the click event fires
        /// </summary>
        private static void OnClosePopupClick(object sender, RoutedEventArgs e)
        {
            Popup popup = GetClosePopup(sender as DependencyObject);

            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        /// <summary>
        /// Gets the closest ancestor that is a Visual
        /// </summary>
        private static T GetClosestVisualAncestor<T>(DependencyObject d) where T : Visual
        {
            DependencyObject parent = GetParent(d);

            while (parent != null)
            {
                T element = parent as T;
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
