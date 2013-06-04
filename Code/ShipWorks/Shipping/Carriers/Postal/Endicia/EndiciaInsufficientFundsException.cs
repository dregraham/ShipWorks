using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Exception thrown when their are insufficient funds for processing in a label server account
    /// </summary>
    public class EndiciaInsufficientFundsException : EndiciaApiException
    {
        EndiciaAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaInsufficientFundsException(EndiciaAccountEntity account, int code, string message)
            : base(code, message)
        {
            this.account = account;
        }

        /// <summary>
        /// The Endicia account that is out of funds
        /// </summary>
        public EndiciaAccountEntity Account
        {
            get { return account; }
        }
    }
}
