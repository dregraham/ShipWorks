using System;
using System.Collections.ObjectModel;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior
{
    /// <summary>
    /// Channel limit behavior for the ODBC
    /// </summary>
    public class OdbcBehavior : IChannelLimitBehavior
    {
        /// <summary>
        /// Populates the channels.
        /// </summary>
        public void PopulateChannels(ObservableCollection<StoreTypeCode> channels,
            StoreTypeCode? channelToAdd)
        {
            channels.Clear();

            channels.Add(StoreTypeCode.Odbc);
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.Odbc;

        /// <summary>
        /// Title
        /// </summary>
        public string Title => "ODBC not allowed";
    }
}