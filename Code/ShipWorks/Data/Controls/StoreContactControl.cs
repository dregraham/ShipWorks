using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;

namespace ShipWorks.Data.Controls
{
    /// <summary>
    /// User control for editing the contact details of a store
    /// </summary>
    partial class StoreContactControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreContactControl()
        {
            InitializeComponent();

            country.DataSource = Geography.Countries;
        }

        /// <summary>
        /// Load the contact information from the given store
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            fullName.Text = store.StoreName;
            company.Text = store.Company;

            street.Line1 = store.Street1;
            street.Line2 = store.Street2;
            street.Line3 = store.Street3;

            city.Text = store.City;
            state.Text = Geography.GetStateProvName(store.StateProvCode, store.CountryCode);
            postalCode.Text = store.PostalCode;
            country.Text = Geography.GetCountryName(store.CountryCode);

            email.Text = store.Email;
            phone.Text = store.Phone;
            fax.Text = store.Fax;
            website.Text = store.Website;
        }

        /// <summary>
        /// Save the contact information for the given store
        /// </summary>
        public void SaveToEntity(StoreEntity store)
        {
            store.StoreName = fullName.Text.Trim();
            store.Company = company.Text;

            store.Street1 = street.Line1;
            store.Street2 = street.Line2;
            store.Street3 = street.Line3;

            store.City = city.Text;
            store.StateProvCode = Geography.GetStateProvCode(state.Text);
            store.PostalCode = postalCode.Text;
            store.CountryCode = Geography.GetCountryCode(country.Text);

            store.Email = email.Text;
            store.Phone = phone.Text;
            store.Fax = fax.Text;
            store.Website = website.Text;
        }

        /// <summary>
        /// The country selection has changed
        /// </summary>
        private void OnCountryChanged(object sender, EventArgs e)
        {
            // Save the text that is in there now
            string stateProv = state.Text;

            // Clear current state\prov
            state.DataSource = null;

            // Get the country
            string countryCode = Geography.GetCountryCode(country.Text);

            if (countryCode == "US")
            {
                state.DataSource = Geography.States;
            }
            else if (countryCode == "CA")
            {
                state.DataSource = Geography.Provinces;
            }

            // Set the text back
            state.Text = stateProv;
        }
    }
}
