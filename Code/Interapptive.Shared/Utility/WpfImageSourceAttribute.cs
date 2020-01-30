using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Attribute for specifying a WPF image source for an enum value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class WpfImageSourceAttribute : Attribute
    {
        /// <summary>
        /// ctor
        /// </summary>
        public WpfImageSourceAttribute(string imageSource)
        {
            ImageSource = imageSource;
        }

        /// <summary>
        /// Wpf image source
        /// </summary>
        public string ImageSource { get; }
    }
}
