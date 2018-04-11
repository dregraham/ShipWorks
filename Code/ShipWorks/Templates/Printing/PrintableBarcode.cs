using System.Text;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// A printable barcode
    /// </summary>
    public struct PrintableBarcode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrintableBarcode(string name, string barcode, string keyboardHotkey)
        {
            Name = name;
            Barcode = barcode;
            KeyboardHotkey = keyboardHotkey;
        }

        /// <summary>
        /// The barcodes name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The barcode
        /// </summary>
        public string Barcode { get; }

        /// <summary>
        /// The KeyboardHotkey
        /// </summary>
        public string KeyboardHotkey { get; }

        /// <summary>
        /// Get the barcodes html block
        /// </summary>
        public string GetHtmlBlock()
        {
            if (string.IsNullOrWhiteSpace(Barcode) && string.IsNullOrWhiteSpace(KeyboardHotkey))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<div>");

            if (!string.IsNullOrWhiteSpace(Name))
            {
                builder.AppendLine($"<b>{Name}</b><br/>");
            }

            if (!string.IsNullOrWhiteSpace(Barcode))
            {
                builder.AppendLine($"<span class='barcode'>*{Barcode}*</span><br/>");
            }

            if (!string.IsNullOrWhiteSpace(KeyboardHotkey))
            {
                builder.AppendLine($"{KeyboardHotkey}");
            }

            builder.AppendLine("</div>");
            

            return builder.ToString();
        }
    }
}
