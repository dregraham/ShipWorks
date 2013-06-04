using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor;
using ShipWorks.Actions.Tasks.Common;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions
{
    /// <summary>
    /// Base class for MarketplaceAdvisor change flags tasks
    /// </summary>
    public abstract class MarketplaceAdvisorChangeFlagsTaskBase : StoreInstanceTaskBase
    {
        MarketplaceAdvisorOmsFlagTypes flagsOn = MarketplaceAdvisorOmsFlagTypes.None;
        MarketplaceAdvisorOmsFlagTypes flagsOff = MarketplaceAdvisorOmsFlagTypes.None;

        /// <summary>
        /// The flags that the task will turn on
        /// </summary>
        public MarketplaceAdvisorOmsFlagTypes FlagsOn
        {
            get { return flagsOn; }
            set { flagsOn = value; }
        }

        /// <summary>
        /// The flags that the task will turn off
        /// </summary>
        public MarketplaceAdvisorOmsFlagTypes FlagsOff
        {
            get { return flagsOff; }
            set { flagsOff = value; }
        }
    }
}
