using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Factory for creating ChannelAdvisorWizardPages in the correct order
    /// </summary>
    [Component]
    public class ChannelAdvisorWizardPageFactory : IChannelAdvisorWizardPageFactory
    {
        private readonly ChannelAdvisorStoreSetupControlHost storeSetupControlHost;
        private readonly ChannelAdvisorExcludeFbaWizardPage fbaWizardPage;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorWizardPageFactory(ChannelAdvisorStoreSetupControlHost storeSetupControlHost, ChannelAdvisorExcludeFbaWizardPage fbaWizardPage)
        {
            this.storeSetupControlHost = storeSetupControlHost;
            this.fbaWizardPage = fbaWizardPage;
        }

        /// <summary>
        /// Returns a list of wizard pages
        /// </summary>
        public List<WizardPage> GetWizardPages()
        {
            return new List<WizardPage>
            {
                storeSetupControlHost,
                fbaWizardPage
            };
        }
    }
}
