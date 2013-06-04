using System;
using System.Text;
using System.Diagnostics;

namespace ShipWorks.UI.Controls.Html.Glyphs
{
	/// <summary>
	/// Represents a single HTML glyph
	/// </summary>
	public class Glyph
	{
        // The HTML tag of the glyph. Such as "h1"
        string tag;

        // If the glyph is for closing, opening, or both tags
        GlyphType type;

        // The path to the image to be displayed be IE
        string imageUrl;

        // Dimensions
        int width;
        int height;

        /// <summary>
        /// Constructor
        /// </summary>
		public Glyph(string tag, GlyphType type, string imageUrl, int width, int height)
		{
			this.tag = tag;
            this.type = type;
            this.imageUrl = imageUrl;
            this.width = width;
            this.height = height;
		}

        /// <summary>
        /// Builds a string that represents this glyph in the IE glyph table
        /// </summary>
        public string GlyphTableRow
        {
            get
            {
                StringBuilder tableRow = new StringBuilder();

                tableRow.AppendFormat("%%{0}^^", tag);
                tableRow.AppendFormat("%%{0}^^", imageUrl);
                tableRow.AppendFormat("%%{0}^^", (int) type);
                tableRow.Append("%%3^^");
                tableRow.Append("%%3^^");
                tableRow.Append("%%4^^");
                tableRow.AppendFormat("%%{0}^^", width);
                tableRow.AppendFormat("%%{0}^^", height);
                tableRow.AppendFormat("%%{0}^^", width);
                tableRow.AppendFormat("%%{0}^^", height);
                tableRow.Append("**");

                return tableRow.ToString();
            }
        }
	}

    /// <summary>
    /// If the glyph is for opening, closing, or both tags.
    /// </summary>
    public enum GlyphType
    {
        OpenTag,
        CloseTag,
        BothTags
    }
}
