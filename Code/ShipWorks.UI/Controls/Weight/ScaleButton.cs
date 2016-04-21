using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.Controls.Weight
{
    /// <summary>
    /// Control to read weight from a scale
    /// </summary>
    [TemplatePart(Name = "PART_Button", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_Display", Type = typeof(TextBlock))]
    public class ScaleButton : Control
    {
        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight",
                typeof(double),
                typeof(ScaleButton));

        public static readonly DependencyProperty DisplayFormatProperty =
            DependencyProperty.Register("DisplayFormat",
                typeof(WeightDisplayFormat),
                typeof(ScaleButton),
                new PropertyMetadata(WeightDisplayFormat.FractionalPounds));

        IDisposable weightSubscription;
        ButtonBase scaleButton;
        TextBlock display;

        /// <summary>
        /// Static constructor
        /// </summary>
        static ScaleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScaleButton), new FrameworkPropertyMetadata(typeof(ScaleButton)));
            WeightControl.ErrorMessageProperty.AddOwner(typeof(ScaleButton));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScaleButton()
        {
            IsVisibleChanged += OnIsVisibleChanged;
        }

        /// <summary>
        /// Weight from the scale
        /// </summary>
        public double Weight
        {
            get { return (double) GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        /// <summary>
        /// Most recent error message
        /// </summary>
        public string ErrorMessage
        {
            get { return (string) GetValue(WeightControl.ErrorMessageProperty); }
        }

        /// <summary>
        /// Apply the template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetupScaleButton();

            display = GetTemplateChild("PART_Display") as TextBlock;
            SetupWeightEventStream(IsVisible);
        }

        /// <summary>
        /// Handle visibility changes
        /// </summary>
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) =>
            SetupWeightEventStream((bool) e.NewValue);

        /// <summary>
        /// Set up the weight event stream
        /// </summary>
        private void SetupWeightEventStream(bool visible)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            weightSubscription?.Dispose();

            if (visible && display != null)
            {
                weightSubscription = ScaleReader.ReadEvents
                    .ObserveOn(DispatcherScheduler.Current)
                    .Subscribe(DisplayWeight);
            }
        }

        /// <summary>
        /// Setup the scale button
        /// </summary>
        private void SetupScaleButton()
        {
            if (scaleButton != null)
            {
                scaleButton.Click -= OnScaleButtonClick;
            }

            scaleButton = GetTemplateChild("PART_Button") as ButtonBase;

            if (scaleButton == null)
            {
                throw new InvalidOperationException("PART_Button is not available in the template");
            }

            scaleButton.Click += OnScaleButtonClick;
        }

        /// <summary>
        /// Display the weight read from the scale
        /// </summary>
        private void DisplayWeight(ScaleReadResult readResult)
        {
            if (display == null)
            {
                return;
            }

            if (readResult.Status == ScaleReadStatus.Success && readResult.Weight >= 0)
            {
                SetValue(WeightControl.ErrorMessageProperty, string.Empty);
                display.Visibility = Visibility.Visible;
                display.Text = FormatWeight(readResult.Weight);
            }
            else
            {
                display.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Read scale into the weight property
        /// </summary>
        private async void OnScaleButtonClick(object sender, RoutedEventArgs e)
        {
            SetValue(WeightControl.ErrorMessageProperty, string.Empty);
            scaleButton.IsEnabled = false;

            ScaleReadResult result = await ScaleReader.ReadScale();

            scaleButton.IsEnabled = true;

            if (result.Status != ScaleReadStatus.Success)
            {
                SetValue(WeightControl.ErrorMessageProperty, result.Message);
                return;
            }

            SetCurrentValue(WeightProperty, result.Weight);
        }

        /// <summary>
        /// Format the weight
        /// </summary>
        private string FormatWeight(double weight) =>
            WeightConverter.Current.FormatWeight(weight, (WeightDisplayFormat) GetValue(DisplayFormatProperty));
    }
}
