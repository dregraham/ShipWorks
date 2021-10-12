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
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Customs control for UPS
    /// </summary>
    public partial class UpsCustomsControl : CustomsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsCustomsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            EnumHelper.BindComboBox<UpsTermsOfSale>(ciTermsOfSale);
            EnumHelper.BindComboBox<UpsExportReason>(ciPurpose);
        }

        /// <summary>
        /// Load the shipment data into the control
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

            documentsOnly.CheckedChanged -= new EventHandler(OnChangeDocumentsOnly);

            // Hide commercial invoice if SurePost.
            sectionInvoicing.Visible = (!LoadedShipments.Any(s => s.Ups != null && UpsUtility.IsUpsSurePostService((UpsServiceType) s.Ups.Service)));

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    if (shipment.Ups == null)
                    {
                        throw new NullReferenceException("shipment.Ups cannot be null.");
                    }

                    documentsOnly.ApplyMultiCheck(shipment.Ups.CustomsDocumentsOnly);
                    extraDocuments.ApplyMultiCheck(shipment.Ups.PaperlessAdditionalDocumentation);
                    descriptionOfGoods.ApplyMultiText(shipment.Ups.CustomsDescription);
                    customsRecipientTIN.ApplyMultiText(shipment.Ups.CustomsRecipientTIN);

                    usePaperlessInvoice.ApplyMultiCheck(shipment.Ups.CommercialPaperlessInvoice);
                    ciTermsOfSale.ApplyMultiValue((UpsTermsOfSale) shipment.Ups.CommercialInvoiceTermsOfSale);
                    ciPurpose.ApplyMultiValue((UpsExportReason) shipment.Ups.CommercialInvoicePurpose);
                    ciComments.ApplyMultiText(shipment.Ups.CommercialInvoiceComments);
                    ciFreight.ApplyMultiAmount(shipment.Ups.CommercialInvoiceFreight);
                    ciInsurance.ApplyMultiAmount(shipment.Ups.CommercialInvoiceInsurance);
                    ciAdditional.ApplyMultiAmount(shipment.Ups.CommercialInvoiceOther);
                }
            }

            // Update the Documents\Products visibiity
            OnChangeDocumentsOnly(documentsOnly, EventArgs.Empty);

            documentsOnly.CheckedChanged += new EventHandler(OnChangeDocumentsOnly);

            // Update the use paperless invoicing controls' enabled state
            OnUsePaperlessInvoiceCheckedChanged(usePaperlessInvoice, EventArgs.Empty);
        }

        /// <summary>
        /// Whether documents or products are avaiable for the shipment
        /// </summary>
        void OnChangeDocumentsOnly(object sender, EventArgs e)
        {
            UpdateDocumentsProductsVisibility(documentsOnly.CheckState != CheckState.Checked);
        }

        /// <summary>
        /// Update the visibility of the editors for showing document entry vs. product entry
        /// </summary>
        private void UpdateDocumentsProductsVisibility(bool showProducts)
        {
            sectionContents.Visible = showProducts;
        }

        /// <summary>
        /// Save the data in the control to the shipments
        /// </summary>
        public override void SaveToShipments()
        {
            base.SaveToShipments();

            // Save the 
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                documentsOnly.ReadMultiCheck(c => shipment.Ups.CustomsDocumentsOnly = c);
                extraDocuments.ReadMultiCheck(c => shipment.Ups.PaperlessAdditionalDocumentation = c);
                descriptionOfGoods.ReadMultiText(t => shipment.Ups.CustomsDescription = t);
                customsRecipientTIN.ReadMultiText(t => shipment.Ups.CustomsRecipientTIN = t);

                usePaperlessInvoice.ReadMultiCheck(c => shipment.Ups.CommercialPaperlessInvoice = c);
                ciTermsOfSale.ReadMultiValue(v => shipment.Ups.CommercialInvoiceTermsOfSale = (int) v);
                ciPurpose.ReadMultiValue(v => shipment.Ups.CommercialInvoicePurpose = (int) v);
                ciComments.ReadMultiText(t => shipment.Ups.CommercialInvoiceComments = t);
                ciFreight.ReadMultiAmount(a => shipment.Ups.CommercialInvoiceFreight = a);
                ciInsurance.ReadMultiAmount(a => shipment.Ups.CommercialInvoiceInsurance = a);
                ciAdditional.ReadMultiAmount(a => shipment.Ups.CommercialInvoiceOther = a);
            }
        }

        /// <summary>
        /// Fires when the invoice use paperless invoice check box changes state.  Updates it's sub children's enabled state.
        /// </summary>
        private void OnUsePaperlessInvoiceCheckedChanged(object sender, EventArgs e)
        {
            extraDocuments.Enabled = usePaperlessInvoice.Checked;
            ciTermsOfSale.Enabled = usePaperlessInvoice.Checked;
            ciPurpose.Enabled = usePaperlessInvoice.Checked;
            ciComments.Enabled = usePaperlessInvoice.Checked;
            ciFreight.Enabled = usePaperlessInvoice.Checked;
            ciInsurance.Enabled = usePaperlessInvoice.Checked;
            ciAdditional.Enabled = usePaperlessInvoice.Checked;
        }

    }
}
