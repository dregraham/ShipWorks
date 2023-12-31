﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// A page of barcodes
    /// </summary>
    public class BarcodePage
    {
        private readonly string title;
        private const string HTMLContent = "<html><head><title></title><style>body {{font-family:Arial; text-align:center;}}table {{margin-bottom:40px;}} td {{text-align:center;}} .barcode {{font-family:'Free 3 of 9 Extended';font-size:36pt;}} </style></head><body>{0}</body></html>";

        /// <summary>
        /// Constructor
        /// </summary>
        public BarcodePage(string title, IEnumerable<PrintableBarcode> barcodes)
        {
            Barcodes = barcodes;
            this.title = title;
        }

        /// <summary>
        /// The barcodes on this page
        /// </summary>
        public IEnumerable<PrintableBarcode> Barcodes { get; }

        /// <summary>
        /// Get the pages html content
        /// </summary>
        public TemplateResult GetTemplateResult()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            PrintableBarcode lastBarcode = Barcodes.LastOrDefault();
            foreach (PrintableBarcode barcode in Barcodes)
            {
                string barcodeBlock = barcode.GetHtmlBlock();

                if (!string.IsNullOrWhiteSpace(barcodeBlock))
                {
                    // dont start out with a line break
                    if (htmlBuilder.Length > 0)
                    {
                        htmlBuilder.Append("<br/>");
                    }

                    htmlBuilder.Append(barcodeBlock);

                    // dont end with a line break
                    if (barcode != lastBarcode)
                    {
                        htmlBuilder.Append("<br/>");
                    }
                }
            }

            if (htmlBuilder.Length > 0 && !string.IsNullOrWhiteSpace(title))
            {
                htmlBuilder.Insert(0, $"<h1>{title}</h1>");
            }

            return new TemplateResult(null, string.Format(HTMLContent, htmlBuilder.ToString()));
        }
    }
}
