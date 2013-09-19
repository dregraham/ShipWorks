using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    public class Express1StampsSettingsFacade : IExpress1SettingsFacade
    {
        public bool UseExpress1 { get; set; }
        public long Express1Account { get; set; }
        public ShipmentType ShipmentType { get; set; }

        public ICollection<KeyValuePair<string, long>> GetAccountList()
        {
            throw new NotImplementedException();
        }
    }
}
