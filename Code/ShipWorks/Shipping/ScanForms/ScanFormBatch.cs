using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// Class that represents a group of ScanForm objects and handles printing and creation
    /// of ScanForm objects from a list of shipments.
    /// </summary>
    public class ScanFormBatch
    {
        private readonly List<ScanForm> scanForms;
        private readonly IScanFormBatchPrinter printer;
        private readonly IScanFormCarrierAccount carrierAccount;
        private readonly IScanFormBatchShipmentRepository shipmentRepository;

        private int shipmentCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFormBatch" /> class.
        /// </summary>
        /// <param name="carrierAccount">The carrier account.</param>
        /// <param name="printer">The printer.</param>
        /// <param name="shipmentRepository">The shipment repository to use for finding shipments related to this batch.</param>
        public ScanFormBatch(IScanFormCarrierAccount carrierAccount, IScanFormBatchPrinter printer, IScanFormBatchShipmentRepository shipmentRepository)
            : this(carrierAccount, printer, new List<ScanForm>(), shipmentRepository)
        { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFormBatch" /> class.
        /// </summary>
        /// <param name="carrierAccount">The carrier account.</param>
        /// <param name="printer">The printer.</param>
        /// <param name="scanForms">The scan forms.</param>
        /// <param name="shipmentRepository">The shipment repository to use for finding shipments related to this batch.</param>
        public ScanFormBatch(IScanFormCarrierAccount carrierAccount, IScanFormBatchPrinter printer, List<ScanForm> scanForms, IScanFormBatchShipmentRepository shipmentRepository)
        {
            this.scanForms = scanForms;
            this.printer = printer;
            this.carrierAccount = carrierAccount;
            this.shipmentRepository = shipmentRepository;

            shipmentCount = 0;
        }

        /// <summary>
        /// Gets or sets the shipment type (Endicia, USPS, etc.).
        /// </summary>
        /// <value>The type of the shipment.</value>
        public ShipmentTypeCode ShipmentType { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Gets the batch ID.
        /// </summary>
        /// <value>The batch ID.</value>
        public long BatchId { get; set; }

        /// <summary>
        /// Gets the total number of shipments across all the scan forms in this batch.
        /// </summary>
        /// <value>The shipment count.</value>
        public int ShipmentCount 
        {
            get
            {
                if (shipmentCount == 0)
                {
                    // Double check with the repository to make sure the shipment count is 
                    // actually zero. Having zero shipments should never occur in practice.
                    // The only way the shipment count could be zero is if this batch was
                    // populated from a previously persisted state where the scan form objects
                    // no longer have a handle to the shipments they correspond with.
                    shipmentCount = shipmentRepository.GetShipmentCount(this);
                }

                return shipmentCount;
            }
            private set { shipmentCount = value; }
        }

        /// <summary>
        /// Gets the scan forms.
        /// </summary>
        /// <value>The scan forms.</value>
        public IEnumerable<ScanForm> ScanForms
        {
            get { return scanForms; }
        }

        /// <summary>
        /// Gets the account entity for the batch
        /// </summary>
        public IEntity2 AccountEntity
        {
            get { return carrierAccount.GetAccountEntity(); }
        }

        /// <summary>
        /// Prints the the scan forms in the batch. Since a printer dialog will be displayed, the window
        /// that will "own" the dialog needs to be provided.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns>True if the printing was successful, otherwise false.</returns>
        public bool Print(IWin32Window owner)
        {
            bool allPrinted = true;

            if (scanForms.Count > 1)
            {
                // We have a batch of scan forms, so use the batch printer
                allPrinted = printer.Print(owner, this);
            }
            else if (scanForms.Count == 1)
            {
                // Just one - defer to the scan form to print itself
                allPrinted = scanForms.First().Print(owner);
            }

            return allPrinted;
        }

        /// <summary>
        /// Creates the SCAN form object(s) for the given list of shipments. The SCAN forms
        /// that are created are added to the ScanForms list.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        public void Create(List<ShipmentEntity> shipments)
        {
            if (shipments != null && shipments.Any())
            {
                // Create batch record
                CreatedDate = DateTime.UtcNow;
                ShipmentType = (ShipmentTypeCode)shipments.First().ShipmentType;
                ShipmentCount = shipments.Count;

                // Process SCAN forms in batches to try and avoid connection timeouts.
                IEnumerable<IEntity2> scanFormEntities = shipments
                    .GroupBy(s => string.Format("{0},{1}", s.OriginPostalCode, ShipmentTypeManager.IsDhl((PostalServiceType)s.Postal.Service)))
                    .SelectMany(shipmentsByZip =>
                        shipmentsByZip
                            .SplitIntoChunksOf(1000)
                            .SelectMany(shipmentChunk =>
                                carrierAccount.GetGateway().CreateScanForms(this, shipmentChunk)))
                    .ToList();

                if (!scanFormEntities.Any())
                {
                    throw new ShippingException(string.Format("ShipWorks was unable to create a SCAN form through {0} at this time. Please try again later.", carrierAccount.ShippingCarrierName));
                }

                // We need to set the batch ID which comes from the batch entity (which this 
                // class has no concept of), so we'll defer to  the carrier account to carry
                // out the saving and returning the scan form ID value.
                BatchId = carrierAccount.Save(this);
            }
        }

        /// <summary>
        /// Creates a scan form object that is managed by the batch
        /// </summary>
        /// <param name="description">Description of the shipment type</param>
        /// <param name="shipments">Collection of shipments that are associated with this SCAN form</param>
        /// <param name="entity">The entity.</param>
        /// <param name="images">Images that make up the actual SCAN form that gets printed.</param>
        /// <returns>A ScanForm object.</returns>
        public ScanForm CreateScanForm(string description, IEnumerable<ShipmentEntity> shipments, IEntity2 entity, List<byte[]> images)
        {
            ScanForm scanForm = new ScanForm(carrierAccount, BatchId, description, shipments, entity)
                {
                    CreatedDate = CreatedDate,
                    Images = images
                };

            scanForms.Add(scanForm);
            return scanForm;
        }
    }
}
