using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    public interface IExpress1SettingsFacade
    {
        bool UseExpress1 { get; set; }
        long Express1Account { get; set; }
        ShipmentType ShipmentType { get; set; }

        ICollection<KeyValuePair<string, long>> GetAccountList();

    }
}
