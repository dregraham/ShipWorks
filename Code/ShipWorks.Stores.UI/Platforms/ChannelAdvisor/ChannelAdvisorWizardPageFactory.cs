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
    [Component]
    public class ChannelAdvisorWizardPageFactory : IChannelAdvisorWizardPageFactory
    {
        public List<WizardPage> GetWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
            {
                scope.Resolve<ChannelAdvisorStoreSetupControlHost>(),
                scope.Resolve<ChannelAdvisorExcludeFbaControl>()
            };
        }
    }
}
