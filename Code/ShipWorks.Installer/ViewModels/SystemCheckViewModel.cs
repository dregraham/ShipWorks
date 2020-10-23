using FontAwesome5;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class SystemCheckViewModel : InstallerViewModelBase
    {
        public SystemCheckViewModel(MainViewModel mainViewModel, 
            INavigationService<NavigationPageType> navigationService, 
            ISystemCheckService systemCheckService) :
            base(mainViewModel, navigationService, NavigationPageType.Eula)
        {
            var result = systemCheckService.CheckSystem();
            mainViewModel.InstallSettings.CheckSystemResult = result;

            if (result.CpuMeetsRequirement &&
                result.HddMeetsRequirement &&
                result.OsMeetsRequirement &&
                result.RamMeetsRequirement)
            {
                MoveNext();
            }
            else
            {
                mainViewModel.InstallSettings.CheckSystemResult = result;
                MoveToWarn();
            }
        }

        private void MoveNext()
        {
            mainViewModel.SystemCheckIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        private void MoveToWarn()
        {
            mainViewModel.WarningIcon = EFontAwesomeIcon.Solid_ExclamationTriangle;
            navigationService.NavigateTo(NavigationPageType.Warning);
        }

        protected override void NextExecute()
        {
            MoveNext();
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
