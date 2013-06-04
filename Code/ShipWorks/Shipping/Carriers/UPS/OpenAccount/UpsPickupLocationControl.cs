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
    public partial class UpsPickupLocationControl : UserControl
    {
        private UpsAccountEntity upsAccountEntity;

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
        public void SaveToRequest(OpenAccountRequest request)
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

            upsAccountEntity = new UpsAccountEntity();
            PersonAdapter personAdapter = new PersonAdapter(upsAccountEntity, "");
            pickupLocationPersonControl.SaveToEntity(personAdapter);

            request.PickupAddress.City = upsAccountEntity.City;
            request.PickupAddress.CompanyName = upsAccountEntity.Company;
            request.PickupAddress.ContactName = pickupLocationPersonControl.FullName;
            request.PickupAddress.CountryCode = upsAccountEntity.CountryCode;
            request.PickupAddress.EmailAddress = upsAccountEntity.Email;
            request.PickupAddress.Phone.Number = upsAccountEntity.Phone;
            request.PickupAddress.PostalCode = upsAccountEntity.PostalCode;
            request.PickupAddress.StateProvinceCode = upsAccountEntity.StateProvCode;
            request.PickupAddress.StreetAddress = upsAccountEntity.Street1;

            upsAccountEntity.RollbackChanges();
            upsAccountEntity = null;
        }
    }
}