using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.Stores.UI.Orders.Split
{
    /// <summary>
    /// Interaction logic for OrderSplitItem.xaml
    /// </summary>
    public partial class OrderSplitItem : UserControl
    {
        [Obfuscation(Exclude = true)]
        public static readonly DependencyProperty ShowDecimalsProperty =
            DependencyProperty.Register("ShowDecimals", typeof(bool), typeof(OrderSplitItem),
                new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Weight in fractional lbs
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public bool ShowDecimals
        {
            get => (bool) GetValue(ShowDecimalsProperty);
            set => SetValue(ShowDecimalsProperty, value);
        }
    }
}
