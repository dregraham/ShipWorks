using ShipWorks.Data.Model.EntityClasses;
using System.ComponentModel;

namespace ShipWorks.Stores.UI.Platforms.SparkPay.WizardPages
{
    /// <summary>
    /// View model for the SparkPayAccount control
    /// </summary>
    public interface ISparkPayAccountViewModel
    {
        /// <summary>
        /// The store Token
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// The store Url
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Property changed handler
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Saves the store with the url and token
        /// </summary>
        bool Save(SparkPayStoreEntity store);
        
        /// <summary>
        /// Loads the store url and token
        /// </summary>
        /// <param name="store"></param>
        void Load(SparkPayStoreEntity store);
    }
}