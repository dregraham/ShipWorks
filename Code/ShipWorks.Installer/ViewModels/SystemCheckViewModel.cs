using System.Timers;
using System.Windows;
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

            //Wait for a second because this can happen so fast that the screen will flash
            Timer t = new Timer()
            {
                Interval = 1000,
                AutoReset = false,
            };

            t.Elapsed += (object sender, ElapsedEventArgs e) => Application.Current.Dispatcher.Invoke(() => ProcessResult(result));
            t.Start();
        }

        private void ProcessResult(SystemCheckResult result)
        {
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
