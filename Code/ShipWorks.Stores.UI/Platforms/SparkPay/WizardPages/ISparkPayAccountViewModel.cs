using ShipWorks.Data.Model.EntityClasses;
using System.ComponentModel;

namespace ShipWorks.Stores.UI.Platforms.SparkPay.WizardPages
{
    public interface ISparkPayAccountViewModel
    {
        string Token { get; set; }
        string Url { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        bool Save(SparkPayStoreEntity store);
    }
}