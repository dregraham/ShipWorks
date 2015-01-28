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
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    /// <summary>
    /// UpsBillingContactInfoControl
    /// </summary>
    public partial class UpsBillingContactInfoControl : UserControl
    {
        private int originalPersonControlHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsBillingContactInfoControl" /> class.
        /// </summary>
        public UpsBillingContactInfoControl()
        {
            InitializeComponent();

            originalPersonControlHeight = billingContactPersonControl.Height;
        }

        /// <summary>
        /// Is the billing address the same as the pickup address
        /// </summary>
        public bool SameAsPickup
        {
            get
            {
                return sameAsPickup.Checked;
            }
        }

        /// <summary>
        /// Saves to request.
        /// </summary>
        public void SaveToAccountAndRequest(OpenAccountRequest request, UpsAccountEntity upsAccount)
        {
            if (!billingContactPersonControl.ValidateRequiredFields())
            {
                throw new UpsOpenAccountException("Required fields missing.", UpsOpenAccountErrorCode.MissingRequiredFields);
            }

            SaveBillingInfoToAccountAndRequest(request, upsAccount);

            if (upsAccount.CountryCode != "US")
            {
                throw new UpsOpenAccountException("ShipWorks can only create US accounts. To create an account for another country, please register your new account on the UPS website.");
            }

            CopyBillingInfoToPickupInfo(request);
        }

        /// <summary>
        /// Saves the billing information to account and request.
        /// </summary>
        private void SaveBillingInfoToAccountAndRequest(OpenAccountRequest request, UpsAccountEntity upsAccount)
        {
            if (request.BillingAddress == null)
            {
                request.BillingAddress = new BillingAddressType();
            }

            if (request.BillingAddress.Phone == null)
            {
                request.BillingAddress.Phone = new PhoneType();
            }

            PersonAdapter personAdapter = new PersonAdapter();
            billingContactPersonControl.SaveToEntity(personAdapter);

            PersonAdapter.Copy(personAdapter, new PersonAdapter(upsAccount, ""));

            request.BillingAddress.City = upsAccount.City;
            request.BillingAddress.CompanyName = upsAccount.Company;
            request.BillingAddress.ContactName = personAdapter.UnparsedName;
            request.BillingAddress.CountryCode = upsAccount.CountryCode;
            request.BillingAddress.EmailAddress = upsAccount.Email;
            request.BillingAddress.Phone.Number = upsAccount.Phone;
            request.BillingAddress.PostalCode = upsAccount.PostalCode;
            request.BillingAddress.StateProvinceCode = upsAccount.StateProvCode;
            request.BillingAddress.StreetAddress = personAdapter.StreetAll.Replace("\r\n", ", ");
        }

        /// <summary>
        /// Copies the billing information to pickup information.
        /// </summary>
        private void CopyBillingInfoToPickupInfo(OpenAccountRequest request)
        {
            if (sameAsPickup.Checked)
            {
                if (request.PickupAddress == null)
                {
                    request.PickupAddress = new PickupAddressType();
                }

                if (request.PickupAddress.Phone == null)
                {
                    request.PickupAddress.Phone = new PhoneType();
                }

                request.PickupAddress.City = request.BillingAddress.City;
                request.PickupAddress.CompanyName = request.BillingAddress.CompanyName;
                request.PickupAddress.ContactName = request.BillingAddress.ContactName;
                request.PickupAddress.CountryCode = request.BillingAddress.CountryCode;
                request.PickupAddress.EmailAddress = request.BillingAddress.EmailAddress;
                request.PickupAddress.Phone.Number = request.BillingAddress.Phone.Number;
                request.PickupAddress.PostalCode = request.BillingAddress.PostalCode;
                request.PickupAddress.StateProvinceCode = request.BillingAddress.StateProvinceCode;
                request.PickupAddress.StreetAddress = request.BillingAddress.StreetAddress;
            }
        }

        /// <summary>
        /// Update the location of controls below the billing contact control when it resizes
        /// </summary>
        private void OnBillingContactPersonControlResize(object sender, EventArgs e)
        {
            sameAsPickup.Top = sameAsPickup.Top - (originalPersonControlHeight - billingContactPersonControl.Height);
        }
    }
}
