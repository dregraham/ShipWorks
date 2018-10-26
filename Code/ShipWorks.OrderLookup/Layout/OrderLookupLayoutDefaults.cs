using System.Collections.Generic;
using System.Windows;

namespace ShipWorks.OrderLookup.Layout
{
    /// <summary>
    /// Defaults for the order lookup mode layout
    /// </summary>
    public class OrderLookupLayoutDefaults 
    {
        /// <summary>
        /// Left Column Width
        /// </summary>
        public GridLength LeftColumnWidth => new GridLength(1, GridUnitType.Star);

        /// <summary>
        /// Middle Column Width
        /// </summary>
        public GridLength RightColumnWidth => new GridLength(1, GridUnitType.Star);

        /// <summary>
        /// Get default layout
        /// </summary>
        public IEnumerable<IEnumerable<PanelInfo>> GetDefaults() => new List<List<PanelInfo>>
        {
            new List<PanelInfo>
            {
                new PanelInfo("IFromViewModel", false),
                new PanelInfo("IToViewModel", true)    
            },
            new List<PanelInfo>
            {
                new PanelInfo("IDetailsViewModel", true),
                new PanelInfo("ILabelOptionsViewModel", false),
                new PanelInfo("IReferenceViewModel", false),
                new PanelInfo("IEmailNotificationsViewModel", false)    
            },
            new List<PanelInfo>
            {
                new PanelInfo("ICustomsViewModel", true),
                new PanelInfo("IRatingViewModel", true)    
            }
        };
    }
}