using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ActiproSoftware.SyntaxEditor;

namespace ShipWorks.Templates.Controls.XslEditing
{
    /// <summary>
    /// Indicator used for showing results from a Find All operation
    /// </summary>
    public class FindAllIndicator : SpanIndicator
    {
        Color borderColor;

        /// <summary>
        /// Initializes a new instance of the <c>FindAllIndicator</c> class.
        /// </summary>
        public FindAllIndicator()
            : this(Color.Red)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <c>FindAllIndicator</c> class.
        /// </summary>
        public FindAllIndicator(Color borderColor) :
            base("Custom")
        {
            this.borderColor = borderColor;
        }

        /// <summary>
        /// Applies the adornments of the span indicator to the specified <see cref="HighlightingStyleResolver"/>.
        /// </summary>
        protected override void ApplyHighlightingStyleAdornments(HighlightingStyleResolver resolver)
        {
            resolver.ApplyBorder(borderColor, HighlightingStyleLineStyle.Dot, HighlightingStyleBorderCornerStyle.Square);
        }

        /// <summary>
        /// Applies the foreground and background colors of the span indicator to the specified <see cref="HighlightingStyleResolver"/>.
        /// </summary>
        protected override void ApplyHighlightingStyleColors(HighlightingStyleResolver resolver)
        {
            resolver.SetBackColor(Color.FromArgb(200, Color.Thistle));
            resolver.SetForeColor(Color.DarkMagenta);
        }

        /// <summary>
        /// We do not change the font
        /// </summary>
        public override bool HasFontChange
        {
            get { return false; }
        }
    }
}
