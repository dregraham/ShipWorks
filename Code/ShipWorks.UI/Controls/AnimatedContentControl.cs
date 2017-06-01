using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A ContentControl that animates the transition between content
    /// </summary>
    [TemplatePart(Name = "PART_PaintArea", Type = typeof(Shape)),
     TemplatePart(Name = "PART_MainContent", Type = typeof(ContentPresenter))]
    public class AnimatedContentControl : ContentControl
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static AnimatedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedContentControl), new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
        }

        private Shape paintArea;
        private ContentPresenter mainContent;
        private bool reverse;

        /// <summary>
        /// This gets called when the template has been applied and we have our visual tree
        /// </summary>
        public override void OnApplyTemplate()
        {
            paintArea = Template.FindName("PART_PaintArea", this) as Shape;
            mainContent = Template.FindName("PART_MainContent", this) as ContentPresenter;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// This gets called when the content we're displaying has changed
        /// </summary>
        /// <param name="oldContent">The content that was previously displayed</param>
        /// <param name="newContent">The new content that is displayed</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (paintArea != null && mainContent != null && oldContent != null)
            {
                paintArea.Fill = CreateBrushFromVisual(mainContent);
                BeginAnimateContentReplacement();
            }

            base.OnContentChanged(oldContent, newContent);
        }

        /// <summary>
        /// Starts the animation for the new content
        /// </summary>
        private void BeginAnimateContentReplacement()
        {
            TranslateTransform newContentTransform = new TranslateTransform();
            mainContent.RenderTransform = newContentTransform;
            newContentTransform.BeginAnimation(TranslateTransform.XProperty,
                CreateAnimation(this.ActualWidth * (reverse ? -1 : 1), 0));

            TranslateTransform oldContentTransform = new TranslateTransform();
            paintArea.RenderTransform = oldContentTransform;
            paintArea.Visibility = Visibility.Visible;
            oldContentTransform.BeginAnimation(TranslateTransform.XProperty,
                CreateAnimation(0, this.ActualWidth * (reverse ? 1 : -1),
                (s, e) => paintArea.Visibility = Visibility.Hidden));

            reverse = !reverse;
        }

        /// <summary>
        /// Creates the animation that moves content in or out of view.
        /// </summary>
        /// <param name="from">The starting value of the animation.</param>
        /// <param name="to">The end value of the animation.</param>
        /// <param name="whenDone">(optional) A callback that will be called when the animation has completed.</param>
        private AnimationTimeline CreateAnimation(double from, double to, EventHandler whenDone = null)
        {
            IEasingFunction ease = new SineEase { EasingMode = EasingMode.EaseOut };
            Duration duration = new Duration(TimeSpan.FromSeconds(0.125));
            DoubleAnimation anim = new DoubleAnimation(from, to, duration) { EasingFunction = ease };

            if (whenDone != null)
            {
                anim.Completed += whenDone;
            }

            anim.Freeze();
            return anim;
        }

        /// <summary>
        /// Creates a brush based on the current appearance of a visual element. The brush is an ImageBrush and once created, won't update its look
        /// </summary>
        /// <param name="v">The visual element to take a snapshot of</param>
        private Brush CreateBrushFromVisual(Visual v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("v");
            }

            RenderTargetBitmap target = new RenderTargetBitmap((int) this.ActualWidth, (int) this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            target.Render(v);

            ImageBrush brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }
    }
}
