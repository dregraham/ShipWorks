using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFormBatch" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="carrierAccount">The carrier account.</param>
        /// <param name="printer">The printer.</param>
        public ScanFormBatch(IScanFormCarrierAccount carrierAccount, IScanFormBatchPrinter printer)
            : this(carrierAccount, printer, new List<ScanForm>())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFormBatch" /> class.
        /// </summary>
        /// <param name="carrierAccount">The carrier account.</param>
        /// <param name="printer">The printer.</param>
        /// <param name="scanForms">The scan forms.</param>
        public ScanFormBatch(IScanFormCarrierAccount carrierAccount, IScanFormBatchPrinter printer, List<ScanForm> scanForms)
        {
            this.scanForms = scanForms;
            this.printer = printer;
            this.carrierAccount = carrierAccount;
        }

        /// <summary>
        /// Gets or sets the shipment type (Endicia, Stamps.com, etc.).
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
            get { return ScanForms.Sum(f => f.ShipmentCount); }
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
        /// that are created are aded to the ScanForms list.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        public void Create(List<ShipmentEntity> shipments)
        {
            if (shipments != null && shipments.Any())
            {
                // Express 1 cubic shipments are created on a separate Endicia account on the Express 1 backend, so we need 
                // to separate the Express 1 cubic shipments into a separate SCAN form and request a scan form for those separately
                // (we don't want the user to have to split these out)
                List<ShipmentEntity> express1CubicShipments = shipments.Where(selected => (ShipmentTypeCode)selected.ShipmentType == ShipmentTypeCode.PostalExpress1 && (PostalPackagingType)selected.Postal.PackagingType == PostalPackagingType.Cubic).ToList();
                List<ShipmentEntity> allOtherShipments = shipments.Except(express1CubicShipments).ToList();

                // Create batch record
                CreatedDate = DateTime.UtcNow;
                ShipmentType = (ShipmentTypeCode)shipments.First().ShipmentType;

                if (express1CubicShipments.Any())
                {
                    // Notate that this scan form contains shipments with the cubic packaging type
                    scanForms.Add(GenerateScanForm(express1CubicShipments, "Cubic shipments"));
                }

                if (allOtherShipments.Any())
                {
                    // The description for the scan form could be better
                    scanForms.Add(GenerateScanForm(allOtherShipments, "Non-cubic shipments"));
                }

                // We need to set the batch ID which comes from the batch entity (which this 
                // class has no concept of), so we'll defer to  the carrier account to carry
                // out the saving and returning the scan form ID value.
                BatchId = carrierAccount.Save(this);
            }
        }

        /// <summary>
        /// A helper/factory method that generates a scan form for the given set of shipments.
        /// </summary>
        /// <param name="shipmentsForScanForm">The shipments for scan form.</param>
        /// <param name="description">The description.</param>
        /// <returns>A ScanForm object.</returns>
        private ScanForm GenerateScanForm(IEnumerable<ShipmentEntity> shipmentsForScanForm, string description)
        {
            // We have shipments selected, so we'll reach out to the carrier API to get the
            // scan form via the scan form gateway
            ScanForm scanForm = new ScanForm(carrierAccount, BatchId, description, shipmentsForScanForm);
            scanForm.Generate();

            return scanForm;
        }
    }
}
