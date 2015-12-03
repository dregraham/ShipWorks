using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ShipWorks.Shipping.UI.ValueConverters;

namespace ShipWorks.Shipping.UI.ShippingPanel.Weight
{
    /// <summary>
    /// Control for editing weight
    /// </summary>
    [TemplatePart(Name = "PART_Entry", Type = typeof(TextBox))]
    public class WeightInput : Control
    {
        static readonly DoubleToWeightStringConverter weightConverter = new DoubleToWeightStringConverter();
        TextBox entry;

        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(double), typeof(WeightInput),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnWeightChanged)));

        /// <summary>
        /// Static constructor
        /// </summary>
        static WeightInput()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WeightInput), new FrameworkPropertyMetadata(typeof(WeightInput)));
        }

        /// <summary>
        /// Weight in fractional lbs
        /// </summary>
        [Bindable(true)]
        public double Weight
        {
            get { return (double) GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        /// <summary>
        /// Apply the template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (entry != null)
            {
                entry.LostKeyboardFocus -= OnEntryLostKeyboardFocus;
            }

            entry = GetTemplateChild("PART_Entry") as TextBox;

            if (entry == null)
            {
                throw new InvalidOperationException("PART_Entry is not available in the template");
            }

            entry.LostKeyboardFocus += OnEntryLostKeyboardFocus;
        }

        /// <summary>
        /// The entry box has lost keyboard focus
        /// </summary>
        private void OnEntryLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            double? weight = weightConverter.ConvertBack(entry.Text, typeof(double), null, null) as double?;

            if (weight.HasValue)
            {
                SetCurrentValue(WeightProperty, weight.Value);
            }
            else
            {
                SetEntryWeightValue(this, Weight);
            }
        }

        /// <summary>
        /// The value of the weight property has changed
        /// </summary>
        private static void OnWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            SetEntryWeightValue(d as WeightInput, (double) e.NewValue);

        /// <summary>
        /// Set the value of the entry box
        /// </summary>
        private static void SetEntryWeightValue(WeightInput input, double weight) =>
            input.entry.Text = weightConverter.Convert(weight, typeof(string), null, null) as string;
    }
}
