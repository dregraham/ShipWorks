using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// User control for editing a store's physical address
    /// </summary>
    public partial class StoreAddressControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreAddressControl()
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
            if (string.IsNullOrEmpty(stateProv))
            {
                // If the text was empty, clear the selection or else the first item in the list will be selected by default.
                // This was causing issues when stores sent an empty state, yet Alabama was selected by default
                state.SelectedIndex = -1;
            }
            else
            {
                state.Text = stateProv;   
            }
        }
    }
}
