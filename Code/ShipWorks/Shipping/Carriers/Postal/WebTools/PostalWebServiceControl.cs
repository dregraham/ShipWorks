using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Service control for settings specific to USPS web tools
    /// </summary>
    public partial class PostalWebServiceControl : PostalServiceControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostalWebServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public PostalWebServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.PostalWebTools, rateControl)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.PostalWebTools);
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            sectionFrom.ExtraText = originControl.OriginDescription;
        }

        /// <summary>
        /// Load the shipment data into the ui
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            originControl.DestinationChanged -= OnOriginDestinationChanged;
            base.LoadShipments(shipments, enableEditing, enableShippingAddress);
            originControl.DestinationChanged += OnOriginDestinationChanged;

            // Load the origin
            originControl.LoadShipments(shipments);
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();

            // Save the origin
            originControl.SaveToEntities();

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }
    }
}
