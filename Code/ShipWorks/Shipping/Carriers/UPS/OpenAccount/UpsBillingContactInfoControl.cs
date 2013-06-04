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
        private UpsAccountEntity upsAccountEntity;

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
        public void SaveToRequest(OpenAccountRequest request)
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

            upsAccountEntity = new UpsAccountEntity();
            PersonAdapter personAdapter = new PersonAdapter(upsAccountEntity, "");
            billingContactPersonControl.SaveToEntity(personAdapter);

            request.BillingAddress.City = upsAccountEntity.City;
            request.BillingAddress.CompanyName = upsAccountEntity.Company;
            request.BillingAddress.ContactName = billingContactPersonControl.FullName;
            request.BillingAddress.CountryCode = upsAccountEntity.CountryCode;
            request.BillingAddress.EmailAddress = upsAccountEntity.Email;
            request.BillingAddress.Phone.Number = upsAccountEntity.Phone;
            request.BillingAddress.PostalCode = upsAccountEntity.PostalCode;
            request.BillingAddress.StateProvinceCode = upsAccountEntity.StateProvCode;
            request.BillingAddress.StreetAddress = upsAccountEntity.Street1;

            upsAccountEntity.RollbackChanges();
            upsAccountEntity = null;
        }
    }
}
