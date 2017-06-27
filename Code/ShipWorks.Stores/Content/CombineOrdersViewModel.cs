using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// View Model for the combine orders dialog
    /// </summary>
    [Component(Service = typeof(ICombineOrdersViewModel))]
    public class CombineOrdersViewModel : ICombineOrdersViewModel, INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        private string newOrderNumber;
        private IOrderEntity survivingOrder;
        private string addressName;
        private string addressStreet;
        private string addressCityStateZip;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrdersViewModel(IEnumerable<IOrderEntity> orders)
        {
            if (orders?.Any() != true)
            {
                throw new ArgumentException("Orders cannot be null or empty");
            }

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            Orders = orders.ToReadOnly();

            IOrderEntity firstOrder = orders.First();
            NewOrderNumber = firstOrder.OrderNumberComplete + "-C";
            SurvivingOrder = firstOrder;
            AddressName = firstOrder.BillPerson.UnparsedName;
            AddressStreet = firstOrder.BillPerson.Street1;
            AddressCityStateZip = $"{firstOrder.BillPerson.City}, {Geography.GetStateProvName(firstOrder.BillPerson.StateProvCode)} {firstOrder.BillPerson.PostalCode}";
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Order number to use for the new order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string NewOrderNumber
        {
            get { return newOrderNumber; }
            set { handler.Set(nameof(NewOrderNumber), ref newOrderNumber, value); }
        }

        /// <summary>
        /// Order that will be used as the basis for the combined order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderEntity SurvivingOrder
        {
            get { return survivingOrder; }
            set { handler.Set(nameof(SurvivingOrder), ref survivingOrder, value); }
        }

        /// <summary>
        /// Name of the recipient of the currently selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AddressName
        {
            get { return addressName; }
            set { handler.Set(nameof(AddressName), ref addressName, value); }
        }

        /// <summary>
        /// Street of the recipient of the currently selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AddressStreet
        {
            get { return addressStreet; }
            set { handler.Set(nameof(AddressStreet), ref addressStreet, value); }
        }

        /// <summary>
        /// City, state, and zip of the currently selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AddressCityStateZip
        {
            get { return addressCityStateZip; }
            set { handler.Set(nameof(AddressCityStateZip), ref addressCityStateZip, value); }
        }

        /// <summary>
        /// Orders that will be combined
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOrderEntity> Orders { get; set; }

        /// <summary>
        /// Details for combining orders
        /// </summary>
        public Tuple<long, string> Details => Tuple.Create(SurvivingOrder.OrderID, NewOrderNumber);
    }
}
