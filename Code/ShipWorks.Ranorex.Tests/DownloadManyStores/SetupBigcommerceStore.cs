﻿///////////////////////////////////////////////////////////////////////////////
//
// This file was automatically generated by RANOREX.
// DO NOT MODIFY THIS FILE! It is regenerated by the designer.
// All your modifications will be lost!
// http://www.ranorex.com
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using Ranorex.Core.Repository;

namespace DownloadManyStores
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The SetupBigcommerceStore recording.
    /// </summary>
    [TestModule("1a53de60-3fce-469d-83cb-049285c3e7f9", ModuleType.Recording, 1)]
    public partial class SetupBigcommerceStore : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupBigcommerceStore instance = new SetupBigcommerceStore();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupBigcommerceStore()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupBigcommerceStore Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
        public static void Start()
        {
            TestModuleRunner.Run(Instance);
        }

        /// <summary>
        /// Performs the playback of actions in this recording.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(0));
            repo.MainForm.Manage.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(1));
            repo.MainForm.Manage.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.Stores' at Center.", repo.ShipWorksSa.StoresInfo, new RecordItemIndex(2));
            repo.ShipWorksSa.Stores.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksSa.Stores' at Center.", repo.ShipWorksSa.StoresInfo, new RecordItemIndex(3));
            repo.ShipWorksSa.Stores.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'StoreManagerDlg.AddStore' at Center.", repo.StoreManagerDlg.AddStoreInfo, new RecordItemIndex(4));
            repo.StoreManagerDlg.AddStore.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'StoreManagerDlg.AddStore' at Center.", repo.StoreManagerDlg.AddStoreInfo, new RecordItemIndex(5));
            repo.StoreManagerDlg.AddStore.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'AddStoreWizard.MainPanel.ComboStoreType' at Center.", repo.AddStoreWizard.MainPanel.ComboStoreTypeInfo, new RecordItemIndex(6));
            repo.AddStoreWizard.MainPanel.ComboStoreType.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AddStoreWizard.MainPanel.ComboStoreType' at Center.", repo.AddStoreWizard.MainPanel.ComboStoreTypeInfo, new RecordItemIndex(7));
            repo.AddStoreWizard.MainPanel.ComboStoreType.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'PopupWindow.BigCommerce' at Center.", repo.PopupWindow.BigCommerceInfo, new RecordItemIndex(8));
            repo.PopupWindow.BigCommerce.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'PopupWindow.BigCommerce' at Center.", repo.PopupWindow.BigCommerceInfo, new RecordItemIndex(9));
            repo.PopupWindow.BigCommerce.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(10));
            repo.AddStoreWizard.Next.MoveTo();
            Delay.Milliseconds(200);
            
            // Click next on the "What platform do you sell on" page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the \"What platform do you sell on\" page\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(11));
            repo.AddStoreWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'AddStoreWizard.MainPanel.BigcommerceApiPath' at Center.", repo.AddStoreWizard.MainPanel.BigcommerceApiPathInfo, new RecordItemIndex(12));
            repo.AddStoreWizard.MainPanel.BigcommerceApiPath.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AddStoreWizard.MainPanel.BigcommerceApiPath' at Center.", repo.AddStoreWizard.MainPanel.BigcommerceApiPathInfo, new RecordItemIndex(13));
            repo.AddStoreWizard.MainPanel.BigcommerceApiPath.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'https://api.bigcommerce.com/stores/cee1f/v3/' with focus on 'AddStoreWizard.MainPanel.BigcommerceApiPath'.", repo.AddStoreWizard.MainPanel.BigcommerceApiPathInfo, new RecordItemIndex(14));
            repo.AddStoreWizard.MainPanel.BigcommerceApiPath.PressKeys("https://api.bigcommerce.com/stores/cee1f/v3/");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab Down}{Tab Up}3nb3fi3seko1m5ikp46i7tgo3i2665f'.", new RecordItemIndex(15));
            Keyboard.Press("{Tab Down}{Tab Up}3nb3fi3seko1m5ikp46i7tgo3i2665f");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab Down}{Tab Up}9kaxovd9q83d9qkdv7oayp60vg8gjj0'.", new RecordItemIndex(16));
            Keyboard.Press("{Tab Down}{Tab Up}9kaxovd9q83d9qkdv7oayp60vg8gjj0");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse None Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(17));
            repo.AddStoreWizard.Next.MoveTo();
            Delay.Milliseconds(200);
            
            // Click next on the "Enter your Bigcommerce Issued credentials" page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the \"Enter your Bigcommerce Issued credentials\" page\r\nMouse Left Click item 'AddStoreWizard.Next' at UpperRight.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(18));
            repo.AddStoreWizard.Next.Click(Location.UpperRight);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse None Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(19));
            repo.AddStoreWizard.Next.MoveTo();
            Delay.Milliseconds(200);
            
            // Click next on the "Number of days back to check" ui
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the \"Number of days back to check\" ui\r\nMouse Left Click item 'AddStoreWizard.Next' at UpperRight.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(20));
            repo.AddStoreWizard.Next.Click(Location.UpperRight);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.MainPanel.StoreName' at Center.", repo.AddStoreWizard.MainPanel.StoreNameInfo, new RecordItemIndex(21));
            repo.AddStoreWizard.MainPanel.StoreName.MoveTo();
            Delay.Milliseconds(200);
            
            // Click: Store Name
            Report.Log(ReportLevel.Info, "Mouse", "Click: Store Name\r\nMouse Left Click item 'AddStoreWizard.MainPanel.StoreName' at Center.", repo.AddStoreWizard.MainPanel.StoreNameInfo, new RecordItemIndex(22));
            repo.AddStoreWizard.MainPanel.StoreName.Click();
            Delay.Milliseconds(200);
            
            // Input Store Name: Amazon
            Report.Log(ReportLevel.Info, "Keyboard", "Input Store Name: Amazon\r\nKey sequence '{RControlKey down}{Akey}{RControlKey up}Bigcommerce' with focus on 'AddStoreWizard.MainPanel.StoreName'.", repo.AddStoreWizard.MainPanel.StoreNameInfo, new RecordItemIndex(23));
            repo.AddStoreWizard.MainPanel.StoreName.PressKeys("{RControlKey down}{Akey}{RControlKey up}Bigcommerce");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}'.", new RecordItemIndex(24));
            Keyboard.Press("{Tab}");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            //Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.MainPanel.Company' at Center.", repo.AddStoreWizard.MainPanel.CompanyInfo, new RecordItemIndex(25));
            //repo.AddStoreWizard.MainPanel.Company.MoveTo();
            //Delay.Milliseconds(200);
            
            // Click: Company
            Report.Log(ReportLevel.Info, "Mouse", "Click: Company\r\nMouse Left Click item 'AddStoreWizard.MainPanel.Company' at Center.", repo.AddStoreWizard.MainPanel.CompanyInfo, new RecordItemIndex(26));
            repo.AddStoreWizard.MainPanel.Company.Click();
            Delay.Milliseconds(200);
            
            // Input Company: ShipWorks
            Report.Log(ReportLevel.Info, "Keyboard", "Input Company: ShipWorks\r\nKey sequence 'ShipWorks' with focus on 'AddStoreWizard.MainPanel.Company'.", repo.AddStoreWizard.MainPanel.CompanyInfo, new RecordItemIndex(27));
            repo.AddStoreWizard.MainPanel.Company.PressKeys("ShipWorks");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            //Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.MainPanel.Street' at Center.", repo.AddStoreWizard.MainPanel.StreetInfo, new RecordItemIndex(28));
            //repo.AddStoreWizard.MainPanel.Street.MoveTo();
            //Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}'.", new RecordItemIndex(29));
            Keyboard.Press("{Tab}");
            Delay.Milliseconds(0);
            
            // Click: Street
            Report.Log(ReportLevel.Info, "Mouse", "Click: Street\r\nMouse Left Click item 'AddStoreWizard.MainPanel.Street' at Center.", repo.AddStoreWizard.MainPanel.StreetInfo, new RecordItemIndex(30));
            repo.AddStoreWizard.MainPanel.Street.Click();
            Delay.Milliseconds(200);
            
            // Input Street: 1 Memorial Drive #2000
            Report.Log(ReportLevel.Info, "Keyboard", "Input Street: 1 Memorial Drive #2000\r\nKey sequence '1 Memorial Drive #2000' with focus on 'AddStoreWizard.MainPanel.Street'.", repo.AddStoreWizard.MainPanel.StreetInfo, new RecordItemIndex(31));
            repo.AddStoreWizard.MainPanel.Street.PressKeys("1 Memorial Drive #2000");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}'.", new RecordItemIndex(32));
            Keyboard.Press("{Tab}");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Click item 'AddStoreWizard.MainPanel.City' at Center.", repo.AddStoreWizard.MainPanel.CityInfo, new RecordItemIndex(33));
            repo.AddStoreWizard.MainPanel.City.Click();
            Delay.Milliseconds(200);
            
            // Click: City
            //Report.Log(ReportLevel.Info, "Mouse", "Click: City\r\nMouse Left Click item 'AddStoreWizard.MainPanel.City' at Center.", repo.AddStoreWizard.MainPanel.CityInfo, new RecordItemIndex(34));
            //repo.AddStoreWizard.MainPanel.City.Click();
            //Delay.Milliseconds(200);
            
            // Input City: Saint Louis
            Report.Log(ReportLevel.Info, "Keyboard", "Input City: Saint Louis\r\nKey sequence 'Saint Louis' with focus on 'AddStoreWizard.MainPanel.City'.", repo.AddStoreWizard.MainPanel.CityInfo, new RecordItemIndex(35));
            repo.AddStoreWizard.MainPanel.City.PressKeys("Saint Louis");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.MainPanel.State' at Center.", repo.AddStoreWizard.MainPanel.StateInfo, new RecordItemIndex(36));
            repo.AddStoreWizard.MainPanel.State.MoveTo();
            Delay.Milliseconds(200);
            
            // Click: State
            Report.Log(ReportLevel.Info, "Mouse", "Click: State\r\nMouse Left Click item 'AddStoreWizard.MainPanel.State' at Center.", repo.AddStoreWizard.MainPanel.StateInfo, new RecordItemIndex(37));
            repo.AddStoreWizard.MainPanel.State.Click();
            Delay.Milliseconds(200);
            
            // Input State: Missouri
            Report.Log(ReportLevel.Info, "Keyboard", "Input State: Missouri\r\nKey sequence 'Missouri' with focus on 'AddStoreWizard.MainPanel.State'.", repo.AddStoreWizard.MainPanel.StateInfo, new RecordItemIndex(38));
            repo.AddStoreWizard.MainPanel.State.PressKeys("Missouri");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.MainPanel.Postal_Code' at Center.", repo.AddStoreWizard.MainPanel.Postal_CodeInfo, new RecordItemIndex(39));
            repo.AddStoreWizard.MainPanel.Postal_Code.MoveTo();
            Delay.Milliseconds(200);
            
            // Click: Postal Code
            Report.Log(ReportLevel.Info, "Mouse", "Click: Postal Code\r\nMouse Left Click item 'AddStoreWizard.MainPanel.Postal_Code' at Center.", repo.AddStoreWizard.MainPanel.Postal_CodeInfo, new RecordItemIndex(40));
            repo.AddStoreWizard.MainPanel.Postal_Code.Click();
            Delay.Milliseconds(200);
            
            // Input Postal Code: 63102
            Report.Log(ReportLevel.Info, "Keyboard", "Input Postal Code: 63102\r\nKey sequence '63102' with focus on 'AddStoreWizard.MainPanel.Postal_Code'.", repo.AddStoreWizard.MainPanel.Postal_CodeInfo, new RecordItemIndex(41));
            repo.AddStoreWizard.MainPanel.Postal_Code.PressKeys("63102");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(42));
            repo.AddStoreWizard.Next.MoveTo();
            Delay.Milliseconds(200);
            
            // Click next on the Store Information page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Store Information page\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(43));
            repo.AddStoreWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(44));
            repo.AddStoreWizard.Next.MoveTo();
            Delay.Milliseconds(200);
            
            // Click next on the Contact Information page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Contact Information page\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(45));
            repo.AddStoreWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'AddStoreWizard.MainPanel.SetStatus' at Center.", repo.AddStoreWizard.MainPanel.SetStatusInfo, new RecordItemIndex(46));
            repo.AddStoreWizard.MainPanel.SetStatus.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AddStoreWizard.MainPanel.SetStatus' at Center.", repo.AddStoreWizard.MainPanel.SetStatusInfo, new RecordItemIndex(47));
            repo.AddStoreWizard.MainPanel.SetStatus.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(48));
            repo.AddStoreWizard.Next.MoveTo();
            Delay.Milliseconds(200);
            
            // Click next on the Download Orders page (it is not named anything specific)
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Download Orders page (it is not named anything specific)\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(49));
            repo.AddStoreWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'AddStoreWizard.Finish' at Center.", repo.AddStoreWizard.FinishInfo, new RecordItemIndex(50));
            repo.AddStoreWizard.Finish.MoveTo();
            Delay.Milliseconds(200);
            
            // Click finish on the Shipworks is ready! page
            Report.Log(ReportLevel.Info, "Mouse", "Click finish on the Shipworks is ready! page\r\nMouse Left Click item 'AddStoreWizard.Finish' at Center.", repo.AddStoreWizard.FinishInfo, new RecordItemIndex(51));
            repo.AddStoreWizard.Finish.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'StoreManagerDlg.Cancel' at Center.", repo.StoreManagerDlg.CancelInfo, new RecordItemIndex(52));
            repo.StoreManagerDlg.Cancel.Click();
            Delay.Milliseconds(200);
            
            // Wait for up to 30s for the ShipWorks download modal to disappear
            Report.Log(ReportLevel.Info, "Wait", "Wait for up to 30s for the ShipWorks download modal to disappear\r\nWaiting 30s to not exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(30000), new RecordItemIndex(53));
            repo.ProgressDlg.SelfInfo.WaitForNotExists(30000);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
