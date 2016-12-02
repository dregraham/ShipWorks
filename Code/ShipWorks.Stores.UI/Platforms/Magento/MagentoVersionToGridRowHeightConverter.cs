using System;
using System.Reflection;
using System.Windows;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.UI.Controls.Design;
using ShipWorks.UI.ValueConverters;

namespace ShipWorks.Stores.UI.Platforms.Magento
{
    /// <summary>
    /// If Magento 1, return *, 0 for false.
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class MagentoVersionToGridHeightConverter : BooleanLambdaConverter<MagentoVersion, GridLength>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoVersionToGridHeightConverter():this(DesignModeDetector.IsDesignerHosted())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoVersionToGridHeightConverter(bool isDesignerHosted) : 
            base(IsMagentoOne, new GridLength(1, GridUnitType.Star), new GridLength(0), isDesignerHosted)
        {
        }

        /// <summary>
        /// Returns true if magentoVersion represents a Magento 1 version - else false.
        /// </summary>
        private static bool IsMagentoOne(MagentoVersion magentoVersion)
        {
            return magentoVersion == MagentoVersion.MagentoConnect || 
                magentoVersion == MagentoVersion.PhpFile;
        }
    }
}
