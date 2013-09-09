using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    public partial class UpsBillingContactInfoControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsBillingContactInfoControl" /> class.
        /// </summary>
        public UpsBillingContactInfoControl()
        {
            InitializeComponent();
        }
         
        /// <summary>
        /// Saves to request.
        /// </summary>
        public void SaveToAccountAndRequest(OpenAccountRequest request, UpsAccountEntity upsAccount)
        {
            if (request.BillingAddress == null)
            {
                request.BillingAddress = new BillingAddressType();
            }

            if (request.BillingAddress.Phone == null)
            {
                request.BillingAddress.Phone = new PhoneType();
            }

            if (!billingContactPersonControl.ValidateRequiredFields())
            {
                throw new UpsOpenAccountException("Required fields missing.", UpsOpenAccountErrorCode.MissingRequiredFields);
            }

            PersonAdapter personAdapter = new PersonAdapter(upsAccount, "");
            billingContactPersonControl.SaveToEntity(personAdapter);

            request.BillingAddress.City = upsAccount.City;
            request.BillingAddress.CompanyName = upsAccount.Company;
            request.BillingAddress.ContactName = billingContactPersonControl.FullName;
            request.BillingAddress.CountryCode = upsAccount.CountryCode;
            request.BillingAddress.EmailAddress = upsAccount.Email;
            request.BillingAddress.Phone.Number = upsAccount.Phone;
            request.BillingAddress.PostalCode = upsAccount.PostalCode;
            request.BillingAddress.StateProvinceCode = upsAccount.StateProvCode;
            request.BillingAddress.StreetAddress = upsAccount.Street1;
        }
    }
}
