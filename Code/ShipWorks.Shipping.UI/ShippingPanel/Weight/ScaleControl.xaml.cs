using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.UI.Controls;
using System.Reflection;

namespace ShipWorks.Shipping.UI.ShippingPanel.Weight
{
    /// <summary>
    /// Interaction logic for ScaleControl.xaml
    /// </summary>
    public partial class ScaleControl : UserControl
    {
        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight",
                typeof(string),
                typeof(ScaleControl));

        public static readonly DependencyProperty DisplayFormatProperty =
            DependencyProperty.Register("DisplayFormat",
                typeof(WeightDisplayFormat),
                typeof(ScaleControl),
                new PropertyMetadata(WeightDisplayFormat.FractionalPounds));

        private IDisposable weightSubscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScaleControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Weight from the scale
        /// </summary>
        public string Weight
        {
            get { return (string)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        /// <summary>
        /// Handle dependency property changes
        /// </summary>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property != IsVisibleProperty)
            {
                return;
            }

            weightSubscription?.Dispose();

            if ((bool)e.NewValue)
            {
                weightSubscription = ScaleReader.ReadEvents
                    .ObserveOn(DispatcherScheduler.Current)
                    .Subscribe(DisplayWeight);
            }
        }

        /// <summary>
        /// Display the weight read from the scale
        /// </summary>
        private void DisplayWeight(ScaleReadResult readResult)
        {
            if (readResult.Status == ScaleReadStatus.Success && readResult.Weight >= 0)
            {
                Display.Visibility = Visibility.Visible;
                Display.Text = FormatWeight(readResult.Weight);
            }
            else
            {
                Display.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Read scale into the weight property
        /// </summary>
        [Obfuscation]
        private async void OnWeighButtonClick(object sender, RoutedEventArgs e)
        {
            ScaleReadResult result = await ScaleReader.ReadScale();

            if (result.Status != ScaleReadStatus.Success)
            {
                return;
            }

            SetValue(WeightProperty, FormatWeight(result.Weight));

            BindingExpression binding = BindingOperations.GetBindingExpression(this, WeightProperty);
            binding?.UpdateSource();
        }

        /// <summary>
        /// Format the weight
        /// </summary>
        private string FormatWeight(double weight) =>
            WeightControl.FormatWeight(weight, (WeightDisplayFormat) GetValue(DisplayFormatProperty));
    }
}
