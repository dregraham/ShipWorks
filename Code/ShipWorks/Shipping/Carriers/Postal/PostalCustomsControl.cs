using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// USPS specific customs stuff
    /// </summary>
    public partial class PostalCustomsControl : CustomsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalCustomsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            EnumHelper.BindComboBox<PostalCustomsContentType>(contentType);
        }

        /// <summary>
        /// Load the shipments into the controls
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing)
        {   
            // A null reference error was being thrown.  Discoverred by Crash Reports.
            // Let's figure out what is null....
            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            base.LoadShipments(shipments, enableEditing);

            contentType.SelectedIndexChanged -= this.OnChangeContentType;

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    if (shipment.Postal == null)
                    {
                        ShippingManager.EnsureShipmentLoaded(shipment);
                    }

                    if (shipment.Postal == null)
                    {
                        throw new NullReferenceException("shipment.Postal cannot be null.");
                    }

                    contentType.ApplyMultiValue((PostalCustomsContentType) shipment.Postal.CustomsContentType);
                    otherDetail.ApplyMultiText(shipment.Postal.CustomsContentDescription);
                }
            }

            contentType.SelectedIndexChanged += new EventHandler(OnChangeContentType);

            UpdateOtherDetailVisibility();
        }

        /// <summary>
        /// Content type has changed
        /// </summary>
        void OnChangeContentType(object sender, EventArgs e)
        {
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                contentType.ReadMultiValue(v => shipment.Postal.CustomsContentType = (int) (PostalCustomsContentType) v);
            }

            UpdateOtherDetailVisibility();
        }

        /// <summary>
        /// Update the visibility of the detail section for "other"
        /// </summary>
        private void UpdateOtherDetailVisibility()
        {
            bool showOther = LoadedShipments.Any(s => s.Postal.CustomsContentType == (int) PostalCustomsContentType.Other);

            labelOtherDetail.Visible = showOther;
            otherDetail.Visible = showOther;
        }

        /// <summary>
        /// Save the data in the control to the loaded shipments
        /// </summary>
        public override void SaveToShipments()
        {
            base.SaveToShipments();

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                contentType.ReadMultiValue(v => shipment.Postal.CustomsContentType = (int) (PostalCustomsContentType) v);
                otherDetail.ReadMultiText(s => shipment.Postal.CustomsContentDescription = s);
            }
        }
    }
}
