using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    public class OneBalanceAutoFundControlViewModel : ViewModelBase
    {
        private decimal minimumBalance;
        private decimal autoFundAmount;
        private bool isAutoFund;

        /// <summary>
        /// The account balance that triggers the auto fund
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal MinimumBalance
        {
            get => minimumBalance;
            set => Set(ref minimumBalance, value);
        }

        /// <summary>
        /// The amount to add to the account balance when auto funding
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal AutoFundAmount
        {
            get => autoFundAmount;
            set => Set(ref autoFundAmount, value);
        }

        /// <summary>
        /// A value indicating if auto funding is turned on
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsAutoFund
        {
            get => isAutoFund;
            set => Set(ref isAutoFund, value);
        }
    }
}
