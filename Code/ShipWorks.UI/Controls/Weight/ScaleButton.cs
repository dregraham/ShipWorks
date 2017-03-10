using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Shared.IO.KeyboardShortcuts;
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
        public const bool AcceptApplyWeightKeyboardShortcutDefault = false;

        public static readonly RoutedEvent ScaleReadEvent = EventManager.RegisterRoutedEvent(
            "ScaleRead", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScaleButton));

        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight",
                typeof(double),
                typeof(ScaleButton));

        public static readonly DependencyProperty DisplayFormatProperty =
            DependencyProperty.Register("DisplayFormat",
                typeof(WeightDisplayFormat),
                typeof(ScaleButton),
                new PropertyMetadata(WeightDisplayFormat.FractionalPounds));

        public static readonly DependencyProperty AcceptApplyWeightKeyboardShortcutProperty =
            DependencyProperty.Register("AcceptApplyWeightKeyboardShortcut", typeof(bool), typeof(ScaleButton),
                new FrameworkPropertyMetadata(AcceptApplyWeightKeyboardShortcutDefault));

        IDisposable weightSubscription;
        IDisposable applyWeightSubscription;
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
        /// Will this control accept the apply weight keyboard shortcut
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public bool AcceptApplyWeightKeyboardShortcut
        {
            get { return (bool) GetValue(AcceptApplyWeightKeyboardShortcutProperty); }
            set { SetValue(AcceptApplyWeightKeyboardShortcutProperty, value); }
        }

        /// <summary>
        /// The scale was read
        /// </summary>
        public event RoutedEventHandler ScaleRead
        {
            add { AddHandler(ScaleReadEvent, value); }
            remove { RemoveHandler(ScaleReadEvent, value); }
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

            applyWeightSubscription?.Dispose();

            if (visible)
            {
                applyWeightSubscription = Messenger.Current.OfType<KeyboardShortcutMessage>()
                    .Where(x => x.AppliesTo(KeyboardShortcutCommand.ApplyWeight))
                    .ObserveOn(DispatcherScheduler.Current)
                    .Where(_ => AcceptApplyWeightKeyboardShortcut && Focusable && scaleButton.IsEnabled)
                    .Subscribe(_ => OnScaleButtonClick(this, new RoutedEventArgs()));
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

            RaiseEvent(new RoutedEventArgs(ScaleReadEvent, this));
        }

        /// <summary>
        /// Format the weight
        /// </summary>
        private string FormatWeight(double weight) =>
            WeightConverter.Current.FormatWeight(weight, (WeightDisplayFormat) GetValue(DisplayFormatProperty));
    }
}
