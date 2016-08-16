using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Limit scrolling based on a minimum size
    /// </summary>
    public class LimitScrollSize : DependencyObject
    {
        /// <summary>
        /// Mode of scroll limiting
        /// </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.RegisterAttached("Mode",
            typeof(LimitScrollSizeMode),
            typeof(LimitScrollSize),
            new PropertyMetadata(LimitScrollSizeMode.None, new PropertyChangedCallback(OnModeChanged)));

        /// <summary>
        /// Get the current value of the Mode property
        /// </summary>
        public static LimitScrollSizeMode GetMode(DependencyObject d) => (LimitScrollSizeMode) d.GetValue(ModeProperty);

        /// <summary>
        /// Set the current value of the Mode property
        /// </summary>
        public static void SetMode(DependencyObject d, LimitScrollSizeMode value) => d.SetValue(ModeProperty, value);

        /// <summary>
        /// The selected mode has changed
        /// </summary>
        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer scrollViewer = d as ScrollViewer;
            if (scrollViewer == null)
            {
                return;
            }

            scrollViewer.SizeChanged -= OnScrollViewerSizeChanged;
            scrollViewer.Loaded -= OnScrollViewerLoaded;

            if ((LimitScrollSizeMode) e.NewValue != LimitScrollSizeMode.None)
            {
                scrollViewer.SizeChanged += OnScrollViewerSizeChanged;
                scrollViewer.Loaded += OnScrollViewerLoaded;

                UpdateScroll(d as ScrollViewer, true, true);
            }
        }

        /// <summary>
        /// The scroll viewer has loaded
        /// </summary>
        private static void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            UpdateScroll(sender as ScrollViewer, true, true);
        }

        /// <summary>
        /// The size of the scroll viewer has changed
        /// </summary>
        private static void OnScrollViewerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // This delays the updating until Windows Forms finishes its sizing. This is only necessary because
            // we're using this functionality within a SandDock in Windows Forms.
            DispatcherScheduler.Current.Schedule(() =>
            {
                UpdateScroll(sender as ScrollViewer, e.WidthChanged, e.HeightChanged);
            });
        }

        /// <summary>
        /// Update the scroll viewer
        /// </summary>
        private static void UpdateScroll(ScrollViewer scrollViewer, bool widthChanged, bool heightChanged)
        {
            DependencyObject content = scrollViewer?.Content as DependencyObject;

            if (scrollViewer == null || content == null)
            {
                return;
            }

            LimitScrollSizeMode mode = GetMode(scrollViewer);
            Thickness margin = (Thickness) content.GetValue(FrameworkElement.MarginProperty);

            if ((mode == LimitScrollSizeMode.Width || mode == LimitScrollSizeMode.Both) && widthChanged)
            {
                UpdateScrollDirection(scrollViewer, content, FrameworkElement.MinWidthProperty,
                    ScrollViewer.ViewportWidthProperty, ScrollViewer.HorizontalScrollBarVisibilityProperty,
                    FrameworkElement.WidthProperty, margin.Left + margin.Right);
            }

            if ((mode == LimitScrollSizeMode.Height || mode == LimitScrollSizeMode.Both) && heightChanged)
            {
                UpdateScrollDirection(scrollViewer, content, FrameworkElement.MinHeightProperty,
                    ScrollViewer.ViewportHeightProperty, ScrollViewer.VerticalScrollBarVisibilityProperty,
                    FrameworkElement.HeightProperty, margin.Top + margin.Bottom);
            }
        }

        /// <summary>
        /// Update an individual scroll direction
        /// </summary>
        private static void UpdateScrollDirection(ScrollViewer scrollViewer, DependencyObject content,
            DependencyProperty minSizeProperty, DependencyProperty viewportHeightProperty,
            DependencyProperty scrollBarVisibilityProperty, DependencyProperty sizeProperty,
            double margin)
        {
            double contentMinSize = (double) content.GetValue(minSizeProperty);
            double viewportSize = (double) scrollViewer.GetValue(viewportHeightProperty);

            if (viewportSize < contentMinSize + margin)
            {
                scrollViewer.SetValue(scrollBarVisibilityProperty, ScrollBarVisibility.Visible);
                content.SetValue(sizeProperty, contentMinSize);
            }
            else
            {
                scrollViewer.SetValue(scrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                content.SetValue(sizeProperty, DependencyProperty.UnsetValue);
            }
        }
    }
}
