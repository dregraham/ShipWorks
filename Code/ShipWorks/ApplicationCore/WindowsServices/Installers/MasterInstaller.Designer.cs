namespace ShipWorks.ApplicationCore.WindowsServices.Installers
{
    partial class MasterInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
            ShipWorks.ApplicationCore.WindowsServices.Installers.ShipWorksServiceInstaller shipWorksSchedulerInstaller;
            serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            shipWorksSchedulerInstaller = new ShipWorks.ApplicationCore.WindowsServices.Installers.ShipWorksServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;
            // 
            // shipWorksSchedulerInstaller
            // 
            shipWorksSchedulerInstaller.Description = "Processes ShipWorks scheduled actions.";
            shipWorksSchedulerInstaller.DisplayName = "ShipWorks Scheduler";
            shipWorksSchedulerInstaller.ServiceName = "ShipWorksScheduler";
            shipWorksSchedulerInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // AssemblyInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            serviceProcessInstaller,
            shipWorksSchedulerInstaller});

        }

        #endregion
    }
}