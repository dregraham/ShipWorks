using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Etsy
{
    [Component]
    public class EtsyDaysBackViewModel : ViewModelBase, IEtsyDaysBackViewModel
    {
        private int initialDownloadDays;
        private string errorMessage;

        public EtsyDaysBackViewModel()
        {
            InitialDownloadDays = 6;
        }

        /// <summary>
        /// Number of days back to start downloading
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int InitialDownloadDays
        {
            get => initialDownloadDays;
            set => Set(ref initialDownloadDays, value);
        }

        /// <summary>
        /// Validation error message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorMessage
        {
            get => errorMessage;
            private set => Set(ref errorMessage, value);
        }

        /// <summary>
        /// Save to the order source
        /// </summary>
        public bool Save(EtsyStoreEntity store)
        {
            if (initialDownloadDays >= 0 && initialDownloadDays <= 30)
            {
                store.InitialDownloadDays = initialDownloadDays;
                return true;
            }
            ErrorMessage = "Number of days back must be between 0 and 30";
            return false;
        }
    }
}