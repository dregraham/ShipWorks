using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Manage the display of a filter control
    /// </summary>
    public class FilterControlDisplayManager : IDisposable
    {
        private readonly Control[] controls;
        private readonly List<Color> originalColors;
        private readonly List<Font> originalFonts;
        private readonly List<DisabledFilterFont> disabledFonts;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controls">Controls that will be managed by this class</param>
        public FilterControlDisplayManager(params Control[] controls)
        {
            this.controls = controls;
            originalColors = controls.Select(x => x.ForeColor).ToList();
            originalFonts = controls.Select(x => x.Font).ToList();
            disabledFonts = controls.Select(x => new DisabledFilterFont(x.Font)).ToList();
        }

        /// <summary>
        /// Toggle the enabled/disabled display
        /// </summary>
        public void ToggleDisplay(bool isEnabled)
        {
            for (int i = 0; i < controls.Count(); i++)
            {
                Control control = controls[i];

                control.ForeColor = isEnabled ? originalColors[0] : disabledFonts[i].TextColor;
                control.Font = isEnabled ? originalFonts[0] : disabledFonts[i].Font;
            }
        }

        /// <summary>
        /// Dispose the object
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
                for (int i = 0; i < controls.Count(); i++)
                {
                    originalFonts[i].Dispose();
                    disabledFonts[i].Dispose();
                }
            }
        }
    }
}