using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using Interapptive.Shared;

namespace ShipWorks.UI.Controls.Html.Glyphs
{
	/// <summary>
	/// Manages HTML glyphs
	/// </summary>
	public static class GlyphManager
	{
        // The glyph table strint to be passed to IE
        static string glyphTable = null;

        // Base location of all of our glyph images
        static string imageBase = "";//@"res://C:\Documents and Settings\Brian\Desktop\Netrix\CS\bin\Debug\Comzept.Genesis.NetRix.Resources.dll/GIFS/";

        /// <summary>
        /// Returns the glyph table string to be passed to IE
        /// </summary>
        public static string GlyphTable
        {
            get
            {
                if (glyphTable == null)
                {
                    glyphTable = BuildGlyphTable();
                }

                return glyphTable;
            }
        }

        /// <summary>
        /// Creates the glpyh table string
        /// </summary>
        private static string BuildGlyphTable()
        {
            StringBuilder glyphTable = new StringBuilder();

            // Get all the glyph entries
            ArrayList glyphs = GetGlyphs();

            // Add them each to the table
            foreach (Glyph glyph in glyphs)
            {
                glyphTable.Append(glyph.GlyphTableRow);
            }

            return glyphTable.ToString();
        }

        /// <summary>
        /// Get all of the Glyph objects to be displayed
        /// </summary>
        [NDependIgnoreLongMethod]
        private static ArrayList GetGlyphs()
        {
            ArrayList list = new ArrayList();

            list.Add(new Glyph("a", GlyphType.CloseTag, imageBase + "A-CLOSE", 22, 17));
            list.Add(new Glyph("a", GlyphType.OpenTag, imageBase + "A-OPEN", 38, 17));
            list.Add(new Glyph("address", GlyphType.CloseTag, imageBase + "ADDRESS-CLOSE", 58, 17));
            list.Add(new Glyph("address", GlyphType.OpenTag, imageBase + "ADDRESS-OPEN", 58, 17));
            list.Add(new Glyph("applet", GlyphType.CloseTag, imageBase + "APPLET-CLOSE", 58, 17));
            list.Add(new Glyph("applet", GlyphType.OpenTag, imageBase + "APPLET-OPEN", 58, 17));
            list.Add(new Glyph("b", GlyphType.CloseTag, imageBase + "B-CLOSE", 22, 17));
            list.Add(new Glyph("b", GlyphType.OpenTag, imageBase + "B-OPEN", 22, 17));
            list.Add(new Glyph("big", GlyphType.CloseTag, imageBase + "BIG-CLOSE", 28, 17));
            list.Add(new Glyph("big", GlyphType.OpenTag, imageBase + "BIG-OPEN", 28, 17));
            list.Add(new Glyph("blockquote", GlyphType.CloseTag, imageBase + "BLOCKQ-CLOSE", 62, 17));
            list.Add(new Glyph("blockquote", GlyphType.OpenTag, imageBase + "BLOCKQ-OPEN", 62, 17));
            list.Add(new Glyph("br", GlyphType.OpenTag, imageBase + "BR", 0x12, 17));
            list.Add(new Glyph("caption", GlyphType.CloseTag, imageBase + "CAPTION-CLOSE", 62, 17));
            list.Add(new Glyph("caption", GlyphType.OpenTag, imageBase + "CAPTION-OPEN", 62, 17));
            list.Add(new Glyph("center", GlyphType.CloseTag, imageBase + "CENTER-CLOSE", 44, 17));
            list.Add(new Glyph("center", GlyphType.OpenTag, imageBase + "CENTER-OPEN", 44, 17));
            list.Add(new Glyph("cite", GlyphType.CloseTag, imageBase + "CITE-CLOSE", 44, 17));
            list.Add(new Glyph("cite", GlyphType.OpenTag, imageBase + "CITE-OPEN", 44, 17));
            list.Add(new Glyph("code", GlyphType.CloseTag, imageBase + "CODE-CLOSE", 44, 17));
            list.Add(new Glyph("code", GlyphType.OpenTag, imageBase + "CODE-OPEN", 44, 17));
            list.Add(new Glyph("dd", GlyphType.CloseTag, imageBase + "DD-CLOSE", 28, 17));
            list.Add(new Glyph("dd", GlyphType.OpenTag, imageBase + "DD-OPEN", 28, 17));
            list.Add(new Glyph("dfn", GlyphType.CloseTag, imageBase + "DFN-CLOSE", 28, 17));
            list.Add(new Glyph("dfn", GlyphType.OpenTag, imageBase + "DFN-OPEN", 28, 17));
            list.Add(new Glyph("dir", GlyphType.CloseTag, imageBase + "DIR-CLOSE", 28, 17));
            list.Add(new Glyph("dir", GlyphType.OpenTag, imageBase + "DIR-OPEN", 28, 17));
            list.Add(new Glyph("div", GlyphType.CloseTag, imageBase + "DIV-CLOSE", 0x20, 17));
            list.Add(new Glyph("div", GlyphType.OpenTag, imageBase + "DIV-OPEN", 50, 17));
            list.Add(new Glyph("dl", GlyphType.CloseTag, imageBase + "DL-CLOSE", 28, 17));
            list.Add(new Glyph("dl", GlyphType.OpenTag, imageBase + "DL-OPEN", 28, 17));
            list.Add(new Glyph("dt", GlyphType.CloseTag, imageBase + "DT-CLOSE", 28, 17));
            list.Add(new Glyph("dt", GlyphType.OpenTag, imageBase + "DT-OPEN", 28, 17));
            list.Add(new Glyph("em", GlyphType.CloseTag, imageBase + "EM-CLOSE", 28, 17));
            list.Add(new Glyph("em", GlyphType.OpenTag, imageBase + "EM-OPEN", 28, 17));
            list.Add(new Glyph("font", GlyphType.CloseTag, imageBase + "FONT-CLOSE", 44, 17));
            list.Add(new Glyph("font", GlyphType.OpenTag, imageBase + "FONT-OPEN", 44, 17));
            list.Add(new Glyph("form", GlyphType.CloseTag, imageBase + "FORM-CLOSE", 40, 17));
            list.Add(new Glyph("form", GlyphType.OpenTag, imageBase + "FORM-OPEN", 62, 17));
            list.Add(new Glyph("h1", GlyphType.CloseTag, imageBase + "H1-CLOSE", 28, 17));
            list.Add(new Glyph("h1", GlyphType.OpenTag, imageBase + "H1-OPEN", 28, 17));
            list.Add(new Glyph("h2", GlyphType.CloseTag, imageBase + "H2-CLOSE", 28, 17));
            list.Add(new Glyph("h2", GlyphType.OpenTag, imageBase + "H2-OPEN", 28, 17));
            list.Add(new Glyph("h3", GlyphType.CloseTag, imageBase + "H3-CLOSE", 28, 17));
            list.Add(new Glyph("h3", GlyphType.OpenTag, imageBase + "H3-OPEN", 28, 17));
            list.Add(new Glyph("h4", GlyphType.CloseTag, imageBase + "H4-CLOSE", 28, 17));
            list.Add(new Glyph("h4", GlyphType.OpenTag, imageBase + "H4-OPEN", 28, 17));
            list.Add(new Glyph("h5", GlyphType.CloseTag, imageBase + "H5-CLOSE", 28, 17));
            list.Add(new Glyph("h5", GlyphType.OpenTag, imageBase + "H5-OPEN", 28, 17));
            list.Add(new Glyph("h6", GlyphType.CloseTag, imageBase + "H6-CLOSE", 28, 17));
            list.Add(new Glyph("h6", GlyphType.OpenTag, imageBase + "H6-OPEN", 28, 17));
            list.Add(new Glyph("hr", GlyphType.OpenTag, imageBase + "HR", 38, 17));
            list.Add(new Glyph("i", GlyphType.CloseTag, imageBase + "I-CLOSE", 22, 17));
            list.Add(new Glyph("i", GlyphType.OpenTag, imageBase + "I-OPEN", 22, 17));
            list.Add(new Glyph("img", GlyphType.BothTags, imageBase + "IMG", 44, 17));
            list.Add(new Glyph("kbd", GlyphType.CloseTag, imageBase + "KBD-CLOSE", 28, 17));
            list.Add(new Glyph("kbd", GlyphType.OpenTag, imageBase + "KBD-OPEN", 28, 17));
            list.Add(new Glyph("li", GlyphType.CloseTag, imageBase + "LI-CLOSE", 20, 17));
            list.Add(new Glyph("li", GlyphType.OpenTag, imageBase + "LI-OPEN", 20, 17));
            list.Add(new Glyph("map", GlyphType.CloseTag, imageBase + "MAP-CLOSE", 40, 17));
            list.Add(new Glyph("map", GlyphType.OpenTag, imageBase + "MAP-OPEN", 62, 17));
            list.Add(new Glyph("menu", GlyphType.CloseTag, imageBase + "MENU-CLOSE", 44, 17));
            list.Add(new Glyph("menu", GlyphType.OpenTag, imageBase + "MENU-OPEN", 44, 17));
            list.Add(new Glyph("ol", GlyphType.CloseTag, imageBase + "OL-CLOSE", 0x18, 17));
            list.Add(new Glyph("ol", GlyphType.OpenTag, imageBase + "OL-OPEN", 38, 17));
            list.Add(new Glyph("p", GlyphType.CloseTag, imageBase + "P-CLOSE", 23, 17));
            list.Add(new Glyph("p", GlyphType.OpenTag, imageBase + "P-OPEN", 23, 17));
            list.Add(new Glyph("param", GlyphType.CloseTag, imageBase + "PARAM-CLOSE", 58, 17));
            list.Add(new Glyph("param", GlyphType.OpenTag, imageBase + "PARAM-OPEN", 58, 17));
            list.Add(new Glyph("pre", GlyphType.CloseTag, imageBase + "PRE-CLOSE", 28, 17));
            list.Add(new Glyph("pre", GlyphType.OpenTag, imageBase + "PRE-OPEN", 28, 17));
            list.Add(new Glyph("samp", GlyphType.CloseTag, imageBase + "SAMP-CLOSE", 44, 17));
            list.Add(new Glyph("samp", GlyphType.OpenTag, imageBase + "SAMP-OPEN", 44, 17));
            list.Add(new Glyph("select", GlyphType.CloseTag, imageBase + "SELECT-CLOSE", 62, 17));
            list.Add(new Glyph("script", GlyphType.CloseTag, imageBase + "SCRIPT", 19, 16));
            list.Add(new Glyph("select", GlyphType.OpenTag, imageBase + "SELECT-OPEN", 62, 17));
            list.Add(new Glyph("small", GlyphType.CloseTag, imageBase + "SMALL-CLOSE", 44, 17));
            list.Add(new Glyph("small", GlyphType.OpenTag, imageBase + "SMALL-OPEN", 44, 17));
            list.Add(new Glyph("strike", GlyphType.CloseTag, imageBase + "STRIKE-CLOSE", 58, 17));
            list.Add(new Glyph("strike", GlyphType.OpenTag, imageBase + "STRIKE-OPEN", 58, 17));
            list.Add(new Glyph("strong", GlyphType.CloseTag, imageBase + "STRONG-CLOSE", 58, 17));
            list.Add(new Glyph("strong", GlyphType.OpenTag, imageBase + "STRONG-OPEN", 58, 17));
            list.Add(new Glyph("style", GlyphType.CloseTag, imageBase + "STYLE", 22, 17));
            list.Add(new Glyph("span", GlyphType.CloseTag, imageBase + "SPAN-CLOSE", 40, 17));
            list.Add(new Glyph("span", GlyphType.OpenTag, imageBase + "SPAN-OPEN", 62, 17));
            list.Add(new Glyph("sub", GlyphType.CloseTag, imageBase + "SUB-CLOSE", 28, 17));
            list.Add(new Glyph("sub", GlyphType.OpenTag, imageBase + "SUB-OPEN", 28, 17));
            list.Add(new Glyph("sup", GlyphType.CloseTag, imageBase + "SUP-CLOSE", 28, 17));
            list.Add(new Glyph("sup", GlyphType.OpenTag, imageBase + "SUP-OPEN", 28, 17));
            list.Add(new Glyph("table", GlyphType.OpenTag, imageBase + "TABLE", 48, 17));
            list.Add(new Glyph("textarea", GlyphType.CloseTag, imageBase + "TEXTAREA-CLOSE", 62, 17));
            list.Add(new Glyph("textarea", GlyphType.OpenTag, imageBase + "TEXTAREA-OPEN", 62, 17));
            list.Add(new Glyph("tt", GlyphType.CloseTag, imageBase + "TT-CLOSE", 28, 17));
            list.Add(new Glyph("tt", GlyphType.OpenTag, imageBase + "TT-OPEN", 28, 17));
            list.Add(new Glyph("u", GlyphType.CloseTag, imageBase + "U-CLOSE", 22, 17));
            list.Add(new Glyph("u", GlyphType.OpenTag, imageBase + "U-OPEN", 22, 17));
            list.Add(new Glyph("ul", GlyphType.CloseTag, imageBase + "UL-CLOSE", 28, 17));
            list.Add(new Glyph("ul", GlyphType.OpenTag, imageBase + "UL-OPEN", 28, 17));
            list.Add(new Glyph("var", GlyphType.CloseTag, imageBase + "VAR-CLOSE", 28, 17));
            list.Add(new Glyph("var", GlyphType.OpenTag, imageBase + "VAR-OPEN", 28, 17));

            return list;
        }
	}
}
