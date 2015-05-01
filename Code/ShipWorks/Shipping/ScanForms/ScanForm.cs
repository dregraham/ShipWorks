using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// Class for generating and printing individual USPS SCAN forms.
    /// </summary>
    public class ScanForm
    {
        private const long InvalidScanFormId = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanForm" /> class.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="batchId">The batch id.</param>
        /// <param name="description">The description.</param>
        public ScanForm(IScanFormCarrierAccount account, long batchId, string description)
            : this(account, batchId, description, new List<ShipmentEntity>(), null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanForm" /> class. This version of the
        /// constructor is intended to be used when the SCAN form needs to be generated. The
        /// shipment count will be populated from the collection of shipments provided.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="batchId">The batch ID.</param>
        /// <param name="description">The description.</param>
        /// <param name="shipments">The shipments.</param>
        /// <param name="scanFormEntity">Actual entity associated with this scan form.</param>
        public ScanForm(IScanFormCarrierAccount account, long batchId, string description, IEnumerable<ShipmentEntity> shipments, IEntity2 scanFormEntity)
            : this(account, InvalidScanFormId, batchId, description, DateTime.MinValue)
        {
            Shipments = shipments;
            ScanFormEntity = scanFormEntity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanForm" /> class. This is intended to be used after
        /// the SCAN form has already been generated and the object is just being re-hydrated from a data source
        /// as the collection of shipments is not specified in this constructor.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="batchId">The batch ID.</param>
        /// <param name="description">The description.</param>
        /// <param name="scanFormId">The scan form ID.</param>
        /// <param name="createdDate">The created date.</param>
        public ScanForm(IScanFormCarrierAccount account, long scanFormId, long batchId, string description, DateTime createdDate)
        {
            CarrierAccount = account;
            
            BatchId = batchId;
            Description = description;

            ScanFormId = scanFormId;
            CreatedDate = createdDate;
        }

        /// <summary>
        /// Gets or sets the batch ID that can be used to group a set of scan forms that 
        /// were generated in one batch.
        /// </summary>
        /// <value>The batch ID.</value>
        public long BatchId { get; private set; }

        /// <summary>
        /// Gets or sets the name of the batch the scan form is associated with.
        /// </summary>
        /// <value>The name of the batch.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Gets the scan form ID.
        /// </summary>
        public long ScanFormId { get; set; }

        /// <summary>
        /// Gets or sets the images that make up the actual forms.
        /// </summary>
        /// <value>The image.</value>
        public List<byte[]> Images { get; set; }

        /// <summary>
        /// Gets the shipments.
        /// </summary>
        /// <value>The shipments.</value>
        public IEnumerable<ShipmentEntity> Shipments { get; private set; }

        /// <summary>
        /// Gets the account.
        /// </summary>
        public IScanFormCarrierAccount CarrierAccount { get; private set; }

        /// <summary>
        /// Gets the scan form entity.
        /// </summary>
        public IEntity2 ScanFormEntity { get; private set; }

        /// <summary>
        /// Prints the scan form.
        /// </summary>
        /// <param name="owner">The IWin32Window object that a print dialog will below to if needed.</param>
        /// <returns>A Boolean value where [true] indicates printing was successful; otherwise [false] is returned.</returns>
        public bool Print(IWin32Window owner)
        {
            if (CarrierAccount == null)
            {
                throw new ShippingException("ShipWorks was unable to print the SCAN form. The shipping carrier could not be determined.");
            }

            IScanFormPrinter printer = CarrierAccount.GetPrinter();            
            return printer.Print(owner, this);
        }
    }
}
