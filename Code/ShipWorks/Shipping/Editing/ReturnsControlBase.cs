using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Base control for Return shipment configuration
    /// </summary>
    public partial class ReturnsControlBase : UserControl
    {
        private List<ShipmentEntity> loadedShipments;

        /// <summary>
        /// A change occurred that impacts the rate of a shipment.
        /// </summary>
        public event EventHandler RateCriteriaChanged;

        /// <summary>
        /// Shipments currently loaded into the control
        /// </summary>
        protected List<ShipmentEntity> LoadedShipments
        {
            get { return loadedShipments; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnsControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load shipment data to the UI
        /// </summary>
        public virtual void LoadShipments(List<ShipmentEntity> shipments)
        {
            this.loadedShipments = shipments;
        }

        /// <summary>
        /// Save data to the shipments
        /// </summary>
        public virtual void SaveToShipments()
        {

        }

        /// <summary>
        /// Raises the rate criteria changed event.
        /// </summary>
        protected void RaiseRateCriteriaChanged()
        {
            RateCriteriaChanged?.Invoke(this, new EventArgs());
        }
    }
}
