using System;
using System.Drawing;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Encapsulates the stylings to use when showing disabled filters.
    /// </summary>
    public class DisabledFilterFont : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisabledFilterFont"/> class.
        /// </summary>
        /// <param name="font">The font.</param>
        public DisabledFilterFont(Font font)
        {
            Font = new Font(font, FontStyle.Strikeout);
        }

        /// <summary>
        /// Gets the font to use to represent disabled filters.
        /// </summary>
        public Font Font { get; private set; }

        /// <summary>
        /// Gets the color of the text.
        /// </summary>
        public Color TextColor { get { return Color.DimGray; } }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Font.Dispose();
            }
        }
    }
}
