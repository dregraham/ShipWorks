using System;
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
            builder.Append("<div>");

            if (!string.IsNullOrWhiteSpace(Name))
            {
                builder.Append($"<b>{Name}</b><br/>");
            }

            if (!string.IsNullOrWhiteSpace(Barcode))
            {
                builder.Append($"<span class='barcode'>*{Barcode}*</span><br/>");
            }

            if (!string.IsNullOrWhiteSpace(KeyboardHotkey))
            {
                builder.Append($"{KeyboardHotkey}");
            }

            builder.Append("</div>");
            return builder.ToString();
        }

        /// <summary>
        /// Override equals
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is PrintableBarcode)
            {
                return this.Equals((PrintableBarcode) obj);
            }
            return false;
        }

        /// <summary>
        /// Override equals
        /// </summary>
        public bool Equals(PrintableBarcode barcode) =>
            (Name == barcode.Name) &&
            (Barcode == barcode.Barcode) &&
            (KeyboardHotkey == barcode.KeyboardHotkey);

        /// <summary>
        /// Override equals
        /// </summary>
        public static bool operator ==(PrintableBarcode lhs, PrintableBarcode rhs) =>
            lhs.Equals(rhs);

        /// <summary>
        /// Override not equals
        /// </summary>
        public static bool operator !=(PrintableBarcode lhs, PrintableBarcode rhs) =>
            !lhs.Equals(rhs);

        /// <summary>
        /// Override GetHashCode for equals and not equals
        /// </summary>
        public override int GetHashCode() =>
            (Name, Barcode, KeyboardHotkey).GetHashCode();
    }
}
