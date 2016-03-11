using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MaxWeightProperty =
            DependencyProperty.Register("MaxWeight", typeof(double), typeof(WeightControl),
                new FrameworkPropertyMetadata(WeightInput.MaxWeightDefault));

        public static readonly DependencyPropertyKey ErrorMessagePropertyKey =
            DependencyProperty.RegisterReadOnly("ErrorMessage",
                typeof(string),
                typeof(WeightControl),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ErrorMessageProperty = ErrorMessagePropertyKey.DependencyProperty;
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

            Binding textBinding = new Binding();
            textBinding.Source = this;
            textBinding.Path = new PropertyPath(nameof(Weight));
            textBinding.Mode = BindingMode.TwoWay;
            entry.SetBinding(WeightInput.WeightProperty, textBinding);

            DependencyPropertyDescriptor scaleButtonDescriptor =
                    DependencyPropertyDescriptor.FromProperty(ScaleButton.ErrorMessageProperty, typeof(ScaleButton));
            scaleButtonDescriptor.AddValueChanged(scaleButton, new EventHandler(OnErrorChanged));

            DependencyPropertyDescriptor entryDescriptor =
                    DependencyPropertyDescriptor.FromProperty(WeightInput.ErrorMessageProperty, typeof(WeightInput));
            entryDescriptor.AddValueChanged(entry, new EventHandler(OnErrorChanged));
        }

        private void OnErrorChanged(object sender, EventArgs e)
        {
            string message = string.Empty;

            if (sender == entry)
            {
                message = entry.GetValue(WeightInput.ErrorMessageProperty) as string;
            }

            if (sender == scaleButton)
            {
                message = scaleButton.GetValue(ScaleButton.ErrorMessageProperty) as string;
            }

            SetValue(ErrorMessagePropertyKey, message);
        }
    }
}
