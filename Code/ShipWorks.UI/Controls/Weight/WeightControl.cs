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
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnWeightChanged)));

        public static readonly DependencyProperty MaxWeightProperty =
            DependencyProperty.Register("MaxWeight", typeof(double), typeof(WeightControl),
                new FrameworkPropertyMetadata(WeightInput.MaxWeightDefault));

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage",
                typeof(string),
                typeof(WeightControl),
                new PropertyMetadata(string.Empty));

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

            AddErrorMessageValueChangedHandler(scaleButton);
            AddErrorMessageValueChangedHandler(entry);
        }

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
