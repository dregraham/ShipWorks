using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.UI.Controls.Weight
{
    /// <summary>
    /// Control for editing weight
    /// </summary>
    [TemplatePart(Name = "PART_Entry", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ScaleButton", Type = typeof(ScaleButton))]
    public class WeightControl : Control
    {
        WeightInput entry;

        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(double), typeof(WeightControl),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnWeightChanged)));

        public static readonly DependencyProperty MaxWeightProperty =
            DependencyProperty.Register("MaxWeight", typeof(double), typeof(WeightControl),
                new FrameworkPropertyMetadata(WeightInput.MaxWeightDefault));

        public static readonly DependencyProperty AcceptApplyWeightKeyboardShortcutProperty =
            DependencyProperty.Register("AcceptApplyWeightKeyboardShortcut", typeof(bool), typeof(WeightControl),
                new FrameworkPropertyMetadata(ScaleButton.AcceptApplyWeightKeyboardShortcutDefault));

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage",
                typeof(string),
                typeof(WeightControl),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TelemetrySourceProperty =
            DependencyProperty.Register("TelemetrySource",
                typeof(string),
                typeof(WeightControl));

        private ScaleButton scaleButton;

        /// <summary>
        /// Static constructor
        /// </summary>
        static WeightControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WeightControl), new FrameworkPropertyMetadata(typeof(WeightControl)));
        }

        /// <summary>
        /// Weight in fractional lbs
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public double Weight
        {
            get { return (double) GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        /// <summary>
        /// Maximum weight
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public double MaxWeight
        {
            get { return (double) GetValue(MaxWeightProperty); }
            set { SetValue(MaxWeightProperty, value); }
        }

        /// <summary>
        /// Most recent error message
        /// </summary>
        public string ErrorMessage
        {
            get { return (string) GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        /// <summary>
        /// Maximum weight
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
        /// Apply the template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            entry = GetTemplateChild("PART_Entry") as WeightInput;
            scaleButton = GetTemplateChild("PART_ScaleButton") as ScaleButton;

            if (entry == null)
            {
                throw new InvalidOperationException("PART_Entry is not available in the template");
            }

            if (scaleButton == null)
            {
                throw new InvalidOperationException("PART_ScaleButton is not available in the template");
            }

            // Remove any existing handlers before adding another
            scaleButton.ScaleRead -= OnScaleButtonScaleRead;
            scaleButton.ScaleRead += OnScaleButtonScaleRead;

            AddErrorMessageValueChangedHandler(scaleButton);
            AddErrorMessageValueChangedHandler(entry);
        }

        /// <summary>
        /// The scale was read
        /// </summary>
        private void OnScaleButtonScaleRead(object sender, RoutedEventArgs e) =>
            entry.RaiseEvent(e);

        /// <summary>
        /// Add an error message value changed handler for a control
        /// </summary>
        private void AddErrorMessageValueChangedHandler(DependencyObject control)
        {
            DependencyPropertyDescriptor descriptor =
                DependencyPropertyDescriptor.FromProperty(ErrorMessageProperty, control.GetType());
            descriptor.AddValueChanged(control, new EventHandler(OnErrorChanged));
        }

        /// <summary>
        /// Handle the error message changing
        /// </summary>
        private void OnErrorChanged(object sender, EventArgs e)
        {
            DependencyObject d = sender as DependencyObject;
            if (d == null)
            {
                return;
            }

            string message = d.GetValue(ErrorMessageProperty) as string;
            SetCurrentValue(ErrorMessageProperty, message);
        }

        /// <summary>
        /// Handle the weight changed
        /// </summary>
        private static void OnWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(ErrorMessageProperty, string.Empty);
        }
    }
}
