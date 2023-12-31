﻿using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Autofac;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages;
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

        public static readonly DependencyProperty TelemetrySourceProperty =
            DependencyProperty.Register("TelemetrySource",
                typeof(string),
                typeof(ScaleButton));

        public static readonly DependencyProperty AcceptApplyWeightKeyboardShortcutProperty =
            DependencyProperty.Register("AcceptApplyWeightKeyboardShortcut", typeof(bool), typeof(ScaleButton),
                new FrameworkPropertyMetadata(AcceptApplyWeightKeyboardShortcutDefault));
        private IDisposable weightSubscription;
        private IDisposable applyWeightSubscription;
        private ButtonBase scaleButton;
        private TextBlock display;
        private readonly IShortcutManager shortcutManager;
        private readonly Func<string, ITrackedDurationEvent> startDurationEvent;
        private readonly ControlOwnerProvider ownerProvider;

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

            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            shortcutManager = IoC.UnsafeGlobalLifetimeScope.Resolve<IShortcutManager>();
            startDurationEvent = IoC.UnsafeGlobalLifetimeScope.Resolve<Func<string, ITrackedDurationEvent>>();

            ownerProvider = new ControlOwnerProvider(this);
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
        /// Source of the weight for telemetry
        /// </summary>
        public string TelemetrySource
        {
            get { return (string) GetValue(TelemetrySourceProperty); }
            set { SetValue(TelemetrySourceProperty, value); }
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
        /// Function gets called when dimensions received from a dimensionalizer
        /// </summary>
        public Action<ScaleReadResult> ChangeDimensions { get; set; }
        
        /// <summary>
        /// Most recent error message
        /// </summary>
        public string ErrorMessage => (string) GetValue(WeightControl.ErrorMessageProperty);

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
                applyWeightSubscription = Messenger.Current.OfType<ShortcutMessage>()
                    .Where(x => x.AppliesTo(KeyboardShortcutCommand.ApplyWeight))
                    .ObserveOn(DispatcherScheduler.Current)
                    .Where(_ => AcceptApplyWeightKeyboardShortcut &&
                        Focusable &&
                        scaleButton.IsEnabled &&
                        ownerProvider.Owner.Handle == NativeMethods.GetActiveWindow())
                    .SelectMany(_ => ApplyWeight(Controls.WeightControl.KeyboardShortcutTelemetryKey).ToObservable())
                    .Subscribe();
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
            await ApplyWeight(Controls.WeightControl.ButtonTelemetryKey);
        }

        /// <summary>
        /// Apply the weight from the scale
        /// </summary>
        private async Task ApplyWeight(string invocationMethod)
        {
            SetValue(WeightControl.ErrorMessageProperty, string.Empty);
            scaleButton.IsEnabled = false;

            using (ITrackedDurationEvent durationEvent = StartTrackingApplyWeight())
            {
                ScaleReadResult result = await ScaleReader.ReadScale();
                SetTelemetryProperties(durationEvent, result, invocationMethod);

                scaleButton.IsEnabled = true;

                if (result.Status != ScaleReadStatus.Success)
                {
                    SetValue(WeightControl.ErrorMessageProperty, result.Message);
                    return;
                }

                SetCurrentValue(WeightProperty, result.Weight);

                // Only send the message if this is a field that supports autoweight shortcut.
                // We only accept the shortcut when it is a shipment field. This keeps us from
                // updating dimensions in the customs control 
                if (AcceptApplyWeightKeyboardShortcut)
                {
                    Messenger.Current.Send<ChangeDimensionsMessage>(new ChangeDimensionsMessage(this, result));
                    ChangeDimensions(result);
                }

                RaiseEvent(new RoutedEventArgs(ScaleReadEvent, this));
            }
        }

        /// <summary>
        /// Start tracking the apply weight command
        /// </summary>
        private ITrackedDurationEvent StartTrackingApplyWeight()
        {
            return string.IsNullOrEmpty(TelemetrySource) ?
                TrackedDurationEvent.Dummy :
                startDurationEvent("Shipment.Scale.Weight.Applied");
        }

        /// <summary>
        /// Set telemetry properties when applying weight
        /// </summary>
        private void SetTelemetryProperties(ITrackedDurationEvent telemetryEvent, ScaleReadResult result, string invocationMethod)
        {
            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.Source", TelemetrySource);
            telemetryEvent.AddMetric(Controls.WeightControl.ShipmentQuantityTelemetryKey, 1);
            telemetryEvent.AddMetric(Controls.WeightControl.PackageQuantityTelemetryKey, 1);
            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.InvocationMethod", invocationMethod);
            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.ScaleType", result.ScaleType.ToString());
            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.ShortcutKey.Used",
                invocationMethod == Controls.WeightControl.KeyboardShortcutTelemetryKey ?
                    new KeyboardShortcutData(shortcutManager.GetWeighShortcut()).ShortcutText :
                    "N/A");
            telemetryEvent.AddMetric("Shipment.Scale.Weight.Applied.ShortcutKey.ConfiguredQuantity", 1);
        }

        /// <summary>
        /// Format the weight
        /// </summary>
        private string FormatWeight(double weight) =>
            WeightConverter.Current.FormatWeight(weight, (WeightDisplayFormat) GetValue(DisplayFormatProperty));
    }
}
