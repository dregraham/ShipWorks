using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    public interface IChannelAdvisorWizardPageFactory
    {
        List<WizardPage> GetWizardPages(ILifetimeScope scope);
    }
}
