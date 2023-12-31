﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace ShipWorksPerformanceTestSuite
{

    [TestModule("17A4D054-F29C-4977-9E00-C9788508E533", ModuleType.UserCode, 1)]
    public class RestoreDatabase : ITestModule
    {
    	public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;

        public RestoreDatabase()
        {
        }
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
			// RESTORE DATABASE SETUP
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SimpleDatabaseSetupWizard.GetSetUp' at Center.", repo.SimpleDatabaseSetupWizard.GetSetUpInfo, new RecordItemIndex(5));
			repo.SimpleDatabaseSetupWizard.GetSetUp.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.ShowAdvancedOptions' at Center.", repo.DetailedDatabaseSetupWizard.ShowAdvancedOptionsInfo, new RecordItemIndex(4));
            repo.DetailedDatabaseSetupWizard.ShowAdvancedOptions.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.ShowAdvancedOptions' at Center.", repo.DetailedDatabaseSetupWizard.ShowAdvancedOptionsInfo, new RecordItemIndex(5));
            repo.DetailedDatabaseSetupWizard.ShowAdvancedOptions.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.MainPanel.RadioChooseRestore2012' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.RadioChooseRestore2012Info, new RecordItemIndex(7));
            repo.DetailedDatabaseSetupWizard.MainPanel.RadioChooseRestore2012.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.MainPanel.RadioChooseRestore2012' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.RadioChooseRestore2012Info, new RecordItemIndex(8));
            repo.DetailedDatabaseSetupWizard.MainPanel.RadioChooseRestore2012.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(9));
            repo.DetailedDatabaseSetupWizard.Next.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(10));
            repo.DetailedDatabaseSetupWizard.Next.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(11));
            repo.DetailedDatabaseSetupWizard.Next.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(12));
            repo.DetailedDatabaseSetupWizard.Next.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.MainPanel.RadioSqlServerRunning' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.RadioSqlServerRunningInfo, new RecordItemIndex(13));
            repo.DetailedDatabaseSetupWizard.MainPanel.RadioSqlServerRunning.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.MainPanel.RadioSqlServerRunning' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.RadioSqlServerRunningInfo, new RecordItemIndex(14));
            repo.DetailedDatabaseSetupWizard.MainPanel.RadioSqlServerRunning.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(15));
            repo.DetailedDatabaseSetupWizard.Next.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(16));
            repo.DetailedDatabaseSetupWizard.Next.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.SQLInstance' at Center.", repo.DetailedDatabaseSetupWizard.SQLInstanceInfo, new RecordItemIndex(17));
            repo.DetailedDatabaseSetupWizard.SQLInstance.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.SQLInstance' at Center.", repo.DetailedDatabaseSetupWizard.SQLInstanceInfo, new RecordItemIndex(18));
            repo.DetailedDatabaseSetupWizard.SQLInstance.Click();
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key 'Ctrl+A' Press with focus on 'DetailedDatabaseSetupWizard.SQLInstance'.", repo.DetailedDatabaseSetupWizard.SQLInstanceInfo, new RecordItemIndex(19));
            Keyboard.PrepareFocus(repo.DetailedDatabaseSetupWizard.SQLInstance);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            
            Keyboard.Press("localhost\ranorex");
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(21));
            repo.DetailedDatabaseSetupWizard.Next.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(22));
            repo.DetailedDatabaseSetupWizard.Next.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.MainPanel.RawTextEdit' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.RawTextEditInfo, new RecordItemIndex(23));
            repo.DetailedDatabaseSetupWizard.MainPanel.RawTextEdit.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.MainPanel.RawTextEdit' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.RawTextEditInfo, new RecordItemIndex(24));
            repo.DetailedDatabaseSetupWizard.MainPanel.RawTextEdit.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.MainPanel.DatabaseName1' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.DatabaseName1Info, new RecordItemIndex(25));
            repo.DetailedDatabaseSetupWizard.MainPanel.DatabaseName1.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.MainPanel.DatabaseName1' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.DatabaseName1Info, new RecordItemIndex(26));
            repo.DetailedDatabaseSetupWizard.MainPanel.DatabaseName1.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(28));
            repo.DetailedDatabaseSetupWizard.Next.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(29));
            repo.DetailedDatabaseSetupWizard.Next.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.MainPanel.BrowseForBackupFile' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.BrowseForBackupFileInfo, new RecordItemIndex(30));
            repo.DetailedDatabaseSetupWizard.MainPanel.BrowseForBackupFile.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.MainPanel.BrowseForBackupFile' at Center.", repo.DetailedDatabaseSetupWizard.MainPanel.BrowseForBackupFileInfo, new RecordItemIndex(31));
            repo.DetailedDatabaseSetupWizard.MainPanel.BrowseForBackupFile.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Cursor moves to Desktop\r\nMouse Left Move item 'Open.Desktop' at Center.", repo.Open.DesktopInfo, new RecordItemIndex(32));
            repo.Open.Desktop.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Click Desktop\r\nMouse Left Click item 'Open.Desktop' at Center.", repo.Open.DesktopInfo, new RecordItemIndex(33));
            repo.Open.Desktop.Click();
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LMenu down}{Dkey}{LMenu up}'.", new RecordItemIndex(34));
            Keyboard.Press("{LMenu down}{Dkey}{LMenu up}");
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key in path Desktop\\Generic Performance\r\nKey sequence 'Desktop\\Generic Performance\\'.", new RecordItemIndex(35));
            Keyboard.Press("Desktop\\Generic Performance\\");
            
            Report.Log(ReportLevel.Info, "Keyboard", "Enter on the keyboard\r\nKey sequence '{Return}'.", new RecordItemIndex(36));
            Keyboard.Press("{Return}");
            
            Report.Log(ReportLevel.Info, "Mouse", "Cursor moves to the File Name Field\r\nMouse Left Move item 'Open.ComboBox1148' at Center.", repo.Open.ComboBox1148Info, new RecordItemIndex(37));
            repo.Open.ComboBox1148.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Click File Name Field\r\nMouse Left Click item 'Open.ComboBox1148' at Center.", repo.Open.ComboBox1148Info, new RecordItemIndex(38));
            repo.Open.ComboBox1148.Click();
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key in text\r\nKey sequence 'Empty.swb'.", new RecordItemIndex(39));
            Keyboard.Press("Empty.swb");
            
            Report.Log(ReportLevel.Info, "Keyboard", "Press Enter on the keyboard > Window closes\r\nKey sequence '{Return}'.", new RecordItemIndex(40));
            Keyboard.Press("{Return}");
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(42));
            repo.DetailedDatabaseSetupWizard.Next.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(43));
            repo.DetailedDatabaseSetupWizard.Next.Click();
            
            Report.Log(ReportLevel.Info, "Delay", "Delay - Restore ShipWorks\r\nWaiting for 14ms.", new RecordItemIndex(44));
            repo.ProgressDlg.ButtonOkInfo.WaitForAttributeEqual(60000, "Enabled", "True");
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ProgressDlg.ButtonOk' at Center.", repo.ProgressDlg.ButtonOkInfo, new RecordItemIndex(45));
            repo.ProgressDlg.ButtonOk.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ProgressDlg.ButtonOk' at Center.", repo.ProgressDlg.ButtonOkInfo, new RecordItemIndex(46));
            repo.ProgressDlg.ButtonOk.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(47));
            repo.DetailedDatabaseSetupWizard.Next.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DetailedDatabaseSetupWizard.Next' at Center.", repo.DetailedDatabaseSetupWizard.NextInfo, new RecordItemIndex(48));
            repo.DetailedDatabaseSetupWizard.Next.Click();
            
            if(repo.DatabaseUpdateWizard.SelfInfo.Exists(30000))
            {            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DatabaseUpdateWizard.Next1' at Center.", repo.DatabaseUpdateWizard.Next1Info, new RecordItemIndex(51));
	            repo.DatabaseUpdateWizard.Next1.MoveTo();
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DatabaseUpdateWizard.Next1' at Center.", repo.DatabaseUpdateWizard.Next1Info, new RecordItemIndex(52));
	            repo.DatabaseUpdateWizard.Next1.Click();
	            
	            Report.Log(ReportLevel.Info, "Keyboard", "Entering credentials\r\nKey sequence 'ssmempires@email.com'.", new RecordItemIndex(53));
	            Keyboard.Press("ssmempires@email.com");
	            
	            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}'.", new RecordItemIndex(54));
	            Keyboard.Press("{Tab}");
	            
	            Report.Log(ReportLevel.Info, "Keyboard", "Enterin credentials\r\nKey sequence 'password1'.", new RecordItemIndex(55));
	            Keyboard.Press("password1");
	            
				Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DatabaseUpdateWizard.Next1' at Center.", repo.DatabaseUpdateWizard.Next1Info, new RecordItemIndex(52));
	            repo.DatabaseUpdateWizard.Next1.Click();
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DatabaseUpdateWizard.Next1' at Center.", repo.DatabaseUpdateWizard.Next1Info, new RecordItemIndex(52));
	            repo.DatabaseUpdateWizard.Next1.Click();
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DatabaseUpdateWizard.Next1' at Center.", repo.DatabaseUpdateWizard.Next1Info, new RecordItemIndex(52));
	            repo.DatabaseUpdateWizard.Next1.Click();
	            
	            Report.Log(ReportLevel.Info, "Wait", "Waiting 5s for the attribute 'Enabled' to equal the specified value 'True'. Associated repository item: 'ProgressDlg.ButtonOk'", repo.ProgressDlg.ButtonOkInfo, new RecordItemIndex(7));
	            repo.ProgressDlg.ButtonOkInfo.WaitForAttributeEqual(60000, "Enabled", "True");
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ProgressDlg.ButtonOk' at Center.", repo.ProgressDlg.ButtonOkInfo, new RecordItemIndex(1));
	            repo.ProgressDlg.ButtonOk.MoveTo();
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ProgressDlg.ButtonOk' at Center.", repo.ProgressDlg.ButtonOkInfo, new RecordItemIndex(1));
	            repo.ProgressDlg.ButtonOk.Click();
	            
	            repo.DatabaseUpdateWizard.Next1Info.WaitForAttributeEqual(60000, "Enabled", "True");
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'DatabaseUpdateWizard.Next1' at Center.", repo.DatabaseUpdateWizard.Next1Info, new RecordItemIndex(51));
	            repo.DatabaseUpdateWizard.Next1.MoveTo();
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'DatabaseUpdateWizard.Next1' at Center.", repo.DatabaseUpdateWizard.Next1Info, new RecordItemIndex(52));
	            repo.DatabaseUpdateWizard.Next1.Click();
            }
            
            repo.LogonDlg.Password.Click();
            
            Keyboard.Press("password1");
            
            repo.LogonDlg.Logon.Click();

            repo.MainForm.Self.Maximize();
        }
    }
}
