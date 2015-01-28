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
    /// <summary>
    /// UpsPickupLocationControl
    /// </summary>
    public partial class UpsPickupLocationControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPickupLocationControl" /> class.
        /// </summary>
        public UpsPickupLocationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Saves to request.
        /// </summary>
        public void SavePickupInfoToAccountAndRequest(OpenAccountRequest request, UpsAccountEntity upsAccount)
        {
            if (request.PickupAddress == null)
            {
                request.PickupAddress = new PickupAddressType();
            }

            if (request.PickupAddress.Phone == null)
            {
                request.PickupAddress.Phone = new PhoneType();
            }

            if (!pickupLocationPersonControl.ValidateRequiredFields())
            {
                throw new UpsOpenAccountException("Required fields missing.", UpsOpenAccountErrorCode.MissingRequiredFields);
            }

            // Adding to account because address fields can't be accessed directly.
            PersonAdapter personAdapter = new PersonAdapter();
            pickupLocationPersonControl.SaveToEntity(personAdapter);

            PersonAdapter.Copy(personAdapter, new PersonAdapter(upsAccount, ""));

            if (upsAccount.CountryCode != "US")
            {
                throw new UpsOpenAccountException("ShipWorks can only create US accounts. To create an account for another country, please register your new account on the UPS website.");
            }

            request.PickupAddress.City = upsAccount.City;
            request.PickupAddress.CompanyName = upsAccount.Company;
            request.PickupAddress.ContactName = personAdapter.UnparsedName;
            request.PickupAddress.CountryCode = upsAccount.CountryCode;
            request.PickupAddress.EmailAddress = upsAccount.Email;
            request.PickupAddress.Phone.Number = upsAccount.Phone;
            request.PickupAddress.PostalCode = upsAccount.PostalCode;
            request.PickupAddress.StateProvinceCode = upsAccount.StateProvCode;
            request.PickupAddress.StreetAddress = personAdapter.StreetAll.Replace("\r\n", ", ");
        }
    }
}