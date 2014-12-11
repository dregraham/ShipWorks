using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon.WebServices.SellerCentral;

namespace ShipWorks.Data.Controls
{
    /// <summary>
    /// Person control that will allow address to be auto-filled from store addresses
    /// </summary>
    public partial class AutofillPersonControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutofillPersonControl()
        {
            InitializeComponent();

            Load += OnLoad;
        }

        /// <summary>
        /// Load the stores when the control is loaded
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Checking DesignMode was not working. I don't like this either...
            if (DesignMode || Process.GetCurrentProcess().ProcessName == "devenv")
            {
                return;
            }

            List<StoreEntity> stores = StoreManager.GetAllStores();
            if (stores.Count == 1)
            {
                LoadStoreAddresIntoPersonControl(stores.First());

                storeSelectorPanel.Visible = false;

                Height = Height - personControl.Top;
                personControl.Location = new Point(0, 0);
            }

            storeAddressLink.TabStop = false;
        }

        /// <summary>
        /// Set which fields are available for editing
        /// </summary>
        [DefaultValue(PersonFields.All)]
        public PersonFields AvailableFields
        {
            get
            {
                return personControl.AvailableFields;
            }
            set
            {
                personControl.AvailableFields = value;
            }
        }

        /// <summary>
        /// The Maximum number of lines in the street field. (Between 1-3)
        /// </summary>
        [DefaultValue(3)]
        [Range(1, 3)]
        [Browsable(true)]
        [Category("Misc")]
        public int MaxStreetLines
        {
            get
            {
                return personControl.MaxStreetLines;
            }
            set
            {
                personControl.MaxStreetLines = value;
            }
        }

        /// <summary>
        /// Fields to Validate as not empty when ValidateRequiredFields is called.
        /// IMPORTANT: This should only be used when not in MultiValued mode.
        /// </summary>
        [DefaultValue(PersonFields.None)]
        [Category("Misc")]
        public PersonFields RequiredFields
        {
            get
            {
                return personControl.RequiredFields;
            }
            set
            {
                personControl.RequiredFields = value;
            }
        }

        /// <summary>
        /// Validate that RequiredFields have data entered.
        /// IMPORTANT: This should only be used when not in MultiValued mode.
        /// </summary>
        public bool ValidateRequiredFields()
        {
            return personControl.ValidateRequiredFields();
        }

        /// <summary>
        /// Load the given entity into the control
        /// </summary>
        public void LoadEntity(PersonAdapter person)
        {
            personControl.LoadEntity(person);
        }

        /// <summary>
        /// Save the data from the controls into the given entity. The original list of loaded people is
        /// left alone.
        /// </summary>
        public void SaveToEntity(PersonAdapter personAdapter)
        {
            personControl.SaveToEntity(personAdapter);
        }

        /// <summary>
        /// Store address link clicked
        /// </summary>
        private void OnStoreAddressLinkClick(object sender, EventArgs e)
        {
            ContextMenu menu = BuildAddressMenu();
            menu.Show(storeAddressLink, new Point(0, ((Control)sender).Height));
        }

        /// <summary>
        /// Build the address menu
        /// </summary>
        private ContextMenu BuildAddressMenu()
        {
            ContextMenu menu = new ContextMenu();
            menu.MenuItems.AddRange(StoreManager.GetAllStores().Select(CreateMenuItem).ToArray());
            return menu;
        }

        /// <summary>
        /// Create a menu item for the given store
        /// </summary>
        private MenuItem CreateMenuItem(StoreEntity store)
        {
            return new MenuItem(store.StoreName, (sender, args) => LoadStoreAddresIntoPersonControl(store));
        }

        /// <summary>
        /// Load the address of the store into the person control
        /// </summary>
        private void LoadStoreAddresIntoPersonControl(StoreEntity store)
        {
            string originalName = personControl.FullName;

            // Create a copy of the address so that any changes cannot be accidentally saved back into the store
            PersonAdapter addressCopy = new PersonAdapter();
            PersonAdapter.Copy(new PersonAdapter(store, string.Empty), addressCopy);

            LoadEntity(addressCopy);

            personControl.FullName = originalName;
        }
    }
}
