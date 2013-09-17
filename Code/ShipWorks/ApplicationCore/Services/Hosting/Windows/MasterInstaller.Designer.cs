namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
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
            ShipWorks.ApplicationCore.Services.Hosting.Windows.WindowsServiceInstaller schedulerInstaller;
            serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            schedulerInstaller = new ShipWorks.ApplicationCore.Services.Hosting.Windows.WindowsServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;
            // 
            // schedulerInstaller
            // 
            schedulerInstaller.Description = "Provides support for running ShipWorks actions when the ShipWorks application is " +
    "not open.";
            schedulerInstaller.ServiceType = ShipWorks.ApplicationCore.Services.ShipWorksServiceType.Scheduler;
            schedulerInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // MasterInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            serviceProcessInstaller,
            schedulerInstaller});

        }

        #endregion
    }
}