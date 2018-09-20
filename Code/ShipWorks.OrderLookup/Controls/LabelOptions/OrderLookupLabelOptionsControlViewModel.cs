using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.LabelOptions
{
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.LabelOptions)]
    public class OrderLookupLabelOptionsControlViewModel : INotifyPropertyChanged
    {
        private readonly IOrderLookupMessageBus messageBus;
        private readonly PropertyChangedHandler handler;
        private DateTime shipDate;
        private bool stealth;
        private bool noPostage;
        private string requestedLabelFormat;

        public event PropertyChangedEventHandler PropertyChanged;

        public OrderLookupLabelOptionsControlViewModel(IOrderLookupMessageBus messageBus)
        {
            this.messageBus = messageBus;
            this.messageBus.PropertyChanged += MessageBusPropertyChanged;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);            
        }

        [Obfuscation(Exclude = true)]
        public DateTime ShipDate
        {
            get => shipDate;            
            set { handler.Set(nameof(ShipDate), ref shipDate, value); }            
        }

        [Obfuscation(Exclude = true)]
        public bool Stealth
        {
            get => stealth;
            set { handler.Set(nameof(Stealth), ref stealth, value); }
        }

        [Obfuscation(Exclude = true)]
        public bool NoPostage
        {
            get => noPostage;
            set { handler.Set(nameof(NoPostage), ref noPostage, value); }
        }

        [Obfuscation(Exclude = true)]
        public string RequestedLabelFormat
        {
            get => requestedLabelFormat;
            set { handler.Set(nameof(RequestedLabelFormat), ref requestedLabelFormat, value); }
        }

        [Obfuscation(Exclude = true)]
        public List<string> LabelFormats { get; set; }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && messageBus.Order != null)
            {

            }
        }
    }
}
