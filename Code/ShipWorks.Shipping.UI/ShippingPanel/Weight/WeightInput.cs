﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.ShippingPanel.Weight
{
    /// <summary>
    /// Control for editing weight
    /// </summary>
    [TemplatePart(Name = "PART_Entry", Type = typeof(TextBox))]
    public class WeightInput : Control
    {
        TextBox entry;

        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(double), typeof(WeightInput),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnWeightChanged)));

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

            entry = GetTemplateChild("PART_Entry") as TextBox;

            if (entry == null)
            {
                throw new InvalidOperationException("PART_Entry is not available in the template");
            }

            Binding textBinding = new Binding();
            textBinding.Source = this;
            textBinding.Path = new PropertyPath(nameof(Text));
            textBinding.Mode = BindingMode.TwoWay;
            entry.SetBinding(TextBox.TextProperty, textBinding);
        }

        /// <summary>
        /// Coerce entered text into text that we expect for weight input
        /// </summary>
        private static object CoerceTextPropertyChanges(DependencyObject d, object baseValue)
        {
            double? weight = WeightConverter.Current.ParseWeight(baseValue as string);

            if (weight.HasValue)
            {
                d.SetCurrentValue(WeightProperty, weight.Value);
                return WeightConverter.Current.FormatWeight(weight.Value);
            }
            else
            {
                return WeightConverter.Current.FormatWeight((double) d.GetValue(WeightProperty));
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
        private static void SetEntryWeightValue(WeightInput input, double weight)
        {
            if (input?.entry != null)
            {
                input.entry.Text = WeightConverter.Current.FormatWeight(weight);
            }
        }
    }
}
