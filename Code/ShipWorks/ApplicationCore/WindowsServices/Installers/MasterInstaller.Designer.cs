﻿namespace ShipWorks.ApplicationCore.WindowsServices.Installers
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
            ShipWorks.ApplicationCore.WindowsServices.Installers.ShipWorksServiceInstaller schedulerInstaller;
            serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            schedulerInstaller = new ShipWorks.ApplicationCore.WindowsServices.Installers.ShipWorksServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;
            // 
            // schedulerInstaller
            // 
            schedulerInstaller.Description = "Processes ShipWorks scheduled actions.";
            schedulerInstaller.ServiceType = ShipWorks.ApplicationCore.WindowsServices.ShipWorksServiceType.Scheduler;
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