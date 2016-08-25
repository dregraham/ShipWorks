using Autofac;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public interface ITermsAndConditionsException
    {
        void OpenTermsAndConditionsDlg(ILifetimeScope lifetimeScope);
    }
}