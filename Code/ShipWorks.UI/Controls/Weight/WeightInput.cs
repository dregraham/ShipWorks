using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.Controls.Weight
{
    /// <summary>
    /// Control for editing weight
    /// </summary>
    [TemplatePart(Name = "PART_Entry", Type = typeof(TextBox))]
    public class WeightInput : Control
    {
        public const double MaxWeightDefault = 10000D;
        TextBox entry;

        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(double), typeof(WeightInput),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnWeightChanged)));

        public static readonly DependencyProperty MaxWeightProperty =
            DependencyProperty.Register("MaxWeight", typeof(double), typeof(WeightInput),
                new FrameworkPropertyMetadata(MaxWeightDefault,
                    FrameworkPropertyMetadataOptions.None,
                    new PropertyChangedCallback(OnMaxWeightChanged)));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WeightInput),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnTextChanged),
                    new CoerceValueCallback(CoerceTextPropertyChanges)));

        /// <summary>
        /// The text has changed
        /// </summary>
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // We need to handle text changes to also handle coercion
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static WeightInput()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WeightInput), new FrameworkPropertyMetadata(typeof(WeightInput)));
            WeightControl.ErrorMessageProperty.AddOwner(typeof(WeightInput));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WeightInput()
        {
            AddHandler(ScaleButton.ScaleReadEvent, new RoutedEventHandler(OnScaleRead));
        }

        /// <summary>
        /// Textual representation of the weight
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
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
        /// Max weight
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
            get { return (string) GetValue(WeightControl.ErrorMessageProperty); }
        }

        /// <summary>
        /// Apply the template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            entry = GetTemplateChild("PART_Entry") as TextBox;

            if (entry == null)
            {
                throw new InvalidOperationException("PART_Entry is not available in the template");
            }

            if (!DesignModeDetector.IsDesignerHosted())
            {
                SetEntryWeightValue(this, 0D);
            }
        }

        /// <summary>
        /// Coerce entered text into text that we expect for weight input
        /// </summary>
        private static object CoerceTextPropertyChanges(DependencyObject d, object baseValue)
        {
            d.SetValue(WeightControl.ErrorMessageProperty, string.Empty);

            double? weight = WeightConverter.Current.ParseWeight(baseValue as string);

            if (weight.HasValue)
            {
                double maxWeight = (double) d.GetValue(MaxWeightProperty);
                double clampedWeight = weight.Value.Clamp(0, maxWeight);
                d.SetCurrentValue(WeightProperty, clampedWeight);
                return WeightConverter.Current.FormatWeight(clampedWeight);
            }
            else
            {
                d.SetValue(WeightControl.ErrorMessageProperty, "The input was not valid.");
                return WeightConverter.Current.FormatWeight((double) d.GetValue(WeightProperty));
            }
        }

        /// <summary>
        /// The value of the weight property has changed
        /// </summary>
        private static void OnWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            SetEntryWeightValue(d as WeightInput, (double) e.NewValue);

        /// <summary>
        /// The max value has changed
        /// </summary>
        private static void OnMaxWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double existingWeight = (double) d.GetValue(WeightProperty);
            double newValue = (double) e.NewValue;

            if (newValue < existingWeight)
            {
                d.SetCurrentValue(WeightProperty, newValue);
            }
        }

        /// <summary>
        /// Set the value of the entry box
        /// </summary>
        private static void SetEntryWeightValue(WeightInput input, double weight)
        {
            if (input?.entry != null)
            {
                input.entry.Text = WeightConverter.Current.FormatWeight(weight);
            }
        }

        /// <summary>
        /// Handle the scale read event
        /// </summary>
        private void OnScaleRead(object sender, RoutedEventArgs e)
        {
            entry.Focus();
            entry.Select(entry.Text.Length, 0);
        }
    }
}
