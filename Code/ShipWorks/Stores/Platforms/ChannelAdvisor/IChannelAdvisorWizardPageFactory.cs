using System.Collections.Generic;
using Autofac;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Create wizard pages for Channel advisor
    /// </summary>
    public interface IChannelAdvisorWizardPageFactory
    {
        /// <summary>
        /// Get the wizard pages.
        /// </summary>
        List<WizardPage> GetWizardPages();
    }
}
