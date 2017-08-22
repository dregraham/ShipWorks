using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// UserControl used for editing return item information
    /// </summary>
    public partial class ReturnTabControl : UserControl
    {
        // If enableEditing was specified in LoadShipments
        bool enableEditing;

        // The shipments that were called LoadShipments
        List<ShipmentEntity> loadedShipments;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnTabControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The shipments last past to LoadShipments
        /// </summary>
        protected List<ShipmentEntity> LoadedShipments => loadedShipments;

        /// <summary>
        /// The enable editing value last past to LoadShipments
        /// </summary>
        protected bool EnableEditing => enableEditing;
      
        private void Load(IEnumerable<ShipmentEntity> shipments)
        {
            
        }
        
        /// <summary>
        /// Update the ContentWeight for each of the given shipments
        /// </summary>
        private void UpdateContentWeight(IEnumerable<ShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                shipment.ContentWeight = shipment.ShipmentReturnItem.Sum(c => c.Quantity * c.Weight);
            }
        }

        /// <summary>
        /// Delete selected return item
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
          
        }

        /// <summary>
        /// Add a return item 
        /// </summary>
        private void OnAdd(object sender, EventArgs e)
        {

        }
    }
}
