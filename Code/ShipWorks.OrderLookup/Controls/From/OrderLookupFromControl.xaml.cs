using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// Interaction logic for OrderLookupFromControl.xaml
    /// </summary>
    public partial class OrderLookupFromControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFromControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dependency property to hold the account description
        /// </summary>
        public static readonly DependencyProperty AccountDescriptionProperty = DependencyProperty.RegisterAttached(
            "AccountDescription",
            typeof(string),
            typeof(OrderLookupFromControl),
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// The Account Description
        /// </summary>
        public string AccountDescription
        {
            get => (string) GetValue(AccountDescriptionProperty);
            set => SetValue(AccountDescriptionProperty, value);
        }

        /// <summary>
        /// Dependency property to hold the origin description
        /// </summary>
        public static readonly DependencyProperty OriginDescriptionProperty = DependencyProperty.RegisterAttached(
            "OriginDescription",
            typeof(string),
            typeof(OrderLookupFromControl),
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// The origin description
        /// </summary>
        public string OriginDescription
        {
            get => (string) GetValue(OriginDescriptionProperty);
            set => SetValue(OriginDescriptionProperty, value);
        }

        /// <summary>
        /// Set the origin description
        /// </summary>
        private void UpdateOriginDescription(object sender, SelectionChangedEventArgs e)
        {
            object originObject = (sender as ComboBox).SelectedItem;

            if (originObject != null && originObject.GetType() == typeof(KeyValuePair<string, long>))
            {
                KeyValuePair<string, long> origin = (KeyValuePair<string, long>) originObject;
                OriginDescription = origin.Key;
            }
            else
            {
                OriginDescription = string.Empty;
            }
            UpdateExpanderHeader();
        }

        /// <summary>
        /// Set the account description property
        /// </summary>
        private void UpdateAccountDescription(object sender, SelectionChangedEventArgs e)
        {
            ICarrierAccount account = (sender as ComboBox).SelectedItem as ICarrierAccount;
            AccountDescription = account?.AccountDescription ?? string.Empty;
            UpdateExpanderHeader();
        }

        /// <summary>
        /// Update the expander header
        /// </summary>
        private void UpdateExpanderHeader()
        {
            bool rateShop = (DataContext as OrderLookupFromViewModel)?.Orchestrator.ShipmentAdapter.SupportsRateShopping ?? false;

            string headerAccountText = rateShop ? "(Rate Shopping)" : AccountDescription;
            Expander.Header = "From Account: " + headerAccountText + ", " + OriginDescription;
        }

        /// <summary>
        /// Update the expander header when changing rate shopping
        /// </summary>
        private void RateShoppingClicked(object sender, RoutedEventArgs e)
        {
            UpdateExpanderHeader();
        }
    }
}
