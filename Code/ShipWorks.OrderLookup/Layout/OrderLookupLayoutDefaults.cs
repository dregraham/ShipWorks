using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using ShipWorks.OrderLookup.Controls.Customs;
using ShipWorks.OrderLookup.Controls.EmailNotifications;
using ShipWorks.OrderLookup.Controls.From;
using ShipWorks.OrderLookup.Controls.LabelOptions;
using ShipWorks.OrderLookup.Controls.OrderItems;
using ShipWorks.OrderLookup.Controls.Rating;
using ShipWorks.OrderLookup.Controls.Reference;
using ShipWorks.OrderLookup.Controls.ShipmentDetails;
using ShipWorks.OrderLookup.Controls.To;

namespace ShipWorks.OrderLookup.Layout
{
    /// <summary>
    /// Defaults for the order lookup mode layout
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
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
                new PanelInfo(nameof(IFromViewModel), false),
                new PanelInfo(nameof(IToViewModel), true),
                new PanelInfo(nameof(IOrderItemsViewModel), true)
            },
            new List<PanelInfo>
            {
                new PanelInfo(nameof(IDetailsViewModel), true),
                new PanelInfo(nameof(ILabelOptionsViewModel), false),
                new PanelInfo(nameof(IReferenceViewModel), false),
                new PanelInfo(nameof(IEmailNotificationsViewModel), false)
            },
            new List<PanelInfo>
            {
                new PanelInfo(nameof(ICustomsViewModel), true),
                new PanelInfo(nameof(IRatingViewModel), true)
            }
        };
    }
}