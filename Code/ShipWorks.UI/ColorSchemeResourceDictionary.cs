using System;
using System.Windows;
using ShipWorks.ApplicationCore.Appearance;

namespace ShipWorks.UI
{
    /// <summary>
    /// Resource dictionary that can handle ShipWorks skinning
    /// </summary>
    public class ColorSchemeResourceDictionary : ResourceDictionary
    {
        private Uri silverSource;
        private Uri blueSource;
        private Uri blackSource;

        /// <summary>
        /// Source of the black color scheme
        /// </summary>
        public Uri BlackSource
        {
            get { return blackSource; }
            set
            {
                blackSource = value;
                UpdateSource();
            }
        }

        /// <summary>
        /// Source of the blue color scheme
        /// </summary>
        public Uri BlueSource
        {
            get { return blueSource; }
            set
            {
                blueSource = value;
                UpdateSource();
            }
        }

        /// <summary>
        /// Source of the silver color scheme
        /// </summary>
        public Uri SilverSource
        {
            get { return silverSource; }
            set
            {
                silverSource = value;
                UpdateSource();
            }
        }

        /// <summary>
        /// Update the source based on the current scheme
        /// </summary>
        public void UpdateSource()
        {
            var val = GetColorSchemeUri(ShipWorksDisplay.ColorScheme);

            if (val != null && base.Source != val)
            {
                base.Source = val;
            }
        }

        /// <summary>
        /// Get the URI of the scheme based on the selected color scheme
        /// </summary>
        private Uri GetColorSchemeUri(ColorScheme colorScheme)
        {
            switch (colorScheme)
            {
                case ColorScheme.Black:
                    return BlackSource;
                case ColorScheme.Silver:
                    return SilverSource;
                default:
                    return BlueSource;
            }
        }
    }
}
