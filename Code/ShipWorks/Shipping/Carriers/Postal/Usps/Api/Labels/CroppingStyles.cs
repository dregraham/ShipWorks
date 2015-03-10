using System.Drawing;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Labels
{
    /// <summary>
    /// Encapuslates all of the cropping styles/sizes to use when manipulating USPS labels.
    /// </summary>
    public static class CroppingStyles
    {
        /// <summary>
        /// Gets the cropping style for the case where the label does not need cropping. This is 
        /// useful for thermal labels and where a blank page/label is needed.
        /// </summary>
        public static Rectangle None
        {
            get { return Rectangle.Empty;}
        }

        /// <summary>
        /// Gets the cropping style for the primary portion of a label.
        /// </summary>
        public static Rectangle PrimaryCrop
        {
            get { return new Rectangle(0, 0, 1600, 1010); }
        }

        /// <summary>
        /// Gets the cropping style for the continuation portion of a label.
        /// </summary>
        public static Rectangle ContinuationCrop
        {
            get { return new Rectangle(0, 1075, 1600, 1010); }
        }

        /// <summary>
        /// Gets the cropping style for international shipments where there is only
        /// one label sent from USPS such as the CN22.
        /// </summary>
        public static Rectangle SingleInternationalCrop
        {
            get { return new Rectangle(198, 123, 1202, 772); }
        }

        /// <summary>
        /// Gets the cropping style for a military primary crop portion of a label.
        /// </summary>
        public static Rectangle MilitaryPrimaryCrop
        {
            get { return new Rectangle(0, 0, 1597, 1005); }
        }

        /// <summary>
        /// Gets the cropping style for a military continuation portion of a label.
        /// </summary>
        public static Rectangle MilitaryContinuationCrop
        {
            get { return new Rectangle(0, 1030, 1597, 1005); }
        }
    }
}
