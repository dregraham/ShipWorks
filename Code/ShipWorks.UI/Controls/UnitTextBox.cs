using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Text box that displays and converts units
    /// </summary>
    [TemplatePart(Name = "PART_Entry", Type = typeof(TextBox))]
    public class UnitTextBox : TextBox
    {
        private const double MaxValueDefault = 10000D;
        private TextBox entry;
        double? initialBoundValue = 0;

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(UnitTextBox),
                                        new FrameworkPropertyMetadata(0.0,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnValueChanged));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(UnitTextBox),
                                        new FrameworkPropertyMetadata(MaxValueDefault,
                                                                      FrameworkPropertyMetadataOptions.None,
                                                                      OnMaxValueChanged));

        public static readonly DependencyProperty UnitTextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(UnitTextBox),
                                        new FrameworkPropertyMetadata(string.Empty,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnTextChanged,
                                                                      CoerceTextPropertyChanges));

        public static readonly DependencyProperty UnitTypeProperty =
            DependencyProperty.Register("UnitType", typeof(UnitType), typeof(UnitTextBox),
                                        new FrameworkPropertyMetadata(UnitType.Weight,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        static UnitTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UnitTextBox), new FrameworkPropertyMetadata(typeof(UnitTextBox)));
        }

        /// <summary>
        /// The text has changed
        /// </summary>
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // We need to handle text changes to also handle coercion
        }

        /// <summary>
        /// value in inches
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public double Value
        {
            get => (double) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Max value
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public double MaxValue
        {
            get => (double) GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// Unit type
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public UnitType UnitType
        {
            get => (UnitType) GetValue(UnitTypeProperty);
            set => SetValue(UnitTypeProperty, value);
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
                SetEntryValue(this, initialBoundValue ?? 0D);
            }
        }

        /// <summary>
        /// Coerce entered text into text that we expect for value input
        /// </summary>
        private static object CoerceTextPropertyChanges(DependencyObject d, object baseValue)
        {
            return (UnitType) d.GetValue(UnitTypeProperty) == UnitType.Weight ?
                FormatWeight(d, baseValue) :
                FormatLength(d, baseValue);
        }

        /// <summary>
        /// Format the weight value
        /// </summary>
        private static object FormatWeight(DependencyObject d, object baseValue)
        {
            double weight = WeightConverter.Current.ParseWeight(baseValue as string) ?? 0;

            double maxWeight = (double) d.GetValue(MaxValueProperty);
            double clampedWeight = weight.Clamp(0, maxWeight);
            d.SetCurrentValue(ValueProperty, clampedWeight);
            return WeightConverter.Current.FormatWeight(clampedWeight);
        }

        /// <summary>
        /// Format the length value
        /// </summary>
        private static object FormatLength(DependencyObject d, object baseValue)
        {
            LengthConverter lengthConverter = new LengthConverter();

            double length = lengthConverter.ParseLength(baseValue as string) ?? 0;

            double maxLength = (double) d.GetValue(MaxValueProperty);
            double clampedLength = length.Clamp(0, maxLength);
            d.SetCurrentValue(ValueProperty, clampedLength);
            return lengthConverter.FormatLength(clampedLength);
        }

        /// <summary>
        /// The Value property has changed
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            SetEntryValue(d as UnitTextBox, (double) e.NewValue);

        /// <summary>
        /// The max value has changed
        /// </summary>
        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double existingValue = (double) d.GetValue(ValueProperty);
            double newValue = (double) e.NewValue;

            if (newValue < existingValue)
            {
                d.SetCurrentValue(ValueProperty, newValue);
            }
        }

        /// <summary>
        /// Set the value of the entry box
        /// </summary>
        private static void SetEntryValue(UnitTextBox input, double value)
        {
            if (input?.entry != null)
            {
                input.entry.Text = (UnitType) input.GetValue(UnitTypeProperty) == UnitType.Weight ?
                    WeightConverter.Current.FormatWeight(value) :
                    new LengthConverter().FormatLength(value);
            }
            else
            {
                input.initialBoundValue = value;
            }
        }
    }
}