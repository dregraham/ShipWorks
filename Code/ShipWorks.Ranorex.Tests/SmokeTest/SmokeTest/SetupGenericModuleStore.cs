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

namespace SmokeTest
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The SetupGenericModuleStore recording.
    /// </summary>
    [TestModule("baeed6b5-a403-4a0a-a54e-7c68d41a72d8", ModuleType.Recording, 1)]
    public partial class SetupGenericModuleStore : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupGenericModuleStore instance = new SetupGenericModuleStore();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupGenericModuleStore()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupGenericModuleStore Instance
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
            Mouse.DefaultMoveTime = 1000;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // Move mouse to click the drop down menu
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to click the drop down menu\r\nMouse Left Move item 'AddStoreWizard.MainPanel.ComboStoreType' at Center.", repo.AddStoreWizard.MainPanel.ComboStoreTypeInfo, new RecordItemIndex(0));
            repo.AddStoreWizard.MainPanel.ComboStoreType.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click to select the store
            Report.Log(ReportLevel.Info, "Mouse", "Click to select the store\r\nMouse Left Click item 'AddStoreWizard.MainPanel.ComboStoreType' at Center.", repo.AddStoreWizard.MainPanel.ComboStoreTypeInfo, new RecordItemIndex(1));
            repo.AddStoreWizard.MainPanel.ComboStoreType.Click(300);
            Delay.Milliseconds(200);
            
            // move mouse to select generic module
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to select generic module\r\nMouse Left Move item 'PopupWindow.GenericModule' at Center.", repo.PopupWindow.GenericModuleInfo, new RecordItemIndex(2));
            repo.PopupWindow.GenericModule.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click generic module
            Report.Log(ReportLevel.Info, "Mouse", "click generic module\r\nMouse Left Click item 'PopupWindow.GenericModule' at Center.", repo.PopupWindow.GenericModuleInfo, new RecordItemIndex(3));
            repo.PopupWindow.GenericModule.Click(300);
            Delay.Milliseconds(200);
            
            // move to next
            Report.Log(ReportLevel.Info, "Mouse", "move to next\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(4));
            repo.AddStoreWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click next
            Report.Log(ReportLevel.Info, "Mouse", "click next\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(5));
            repo.AddStoreWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // move mouse to enter username
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to enter username\r\nMouse Left Move item 'ShipWorksSa.MainPanel.Password' at Center.", repo.ShipWorksSa.MainPanel.PasswordInfo, new RecordItemIndex(6));
            repo.ShipWorksSa.MainPanel.Password.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click to select the field
            Report.Log(ReportLevel.Info, "Mouse", "click to select the field\r\nMouse Left Click item 'ShipWorksSa.MainPanel.Password' at Center.", repo.ShipWorksSa.MainPanel.PasswordInfo, new RecordItemIndex(7));
            repo.ShipWorksSa.MainPanel.Password.Click(300);
            Delay.Milliseconds(200);
            
            // enter the username
            Report.Log(ReportLevel.Info, "Keyboard", "enter the username\r\nKey sequence 'Ranorex' with focus on 'ShipWorksSa.MainPanel.Password'.", repo.ShipWorksSa.MainPanel.PasswordInfo, new RecordItemIndex(8));
            repo.ShipWorksSa.MainPanel.Password.PressKeys("Ranorex");
            Delay.Milliseconds(0);
            
            // move mouse to enter password
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to enter password\r\nMouse Left Move item 'ShipWorksSa.MainPanel.Text' at Center.", repo.ShipWorksSa.MainPanel.TextInfo, new RecordItemIndex(9));
            repo.ShipWorksSa.MainPanel.Text.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click to select the field
            Report.Log(ReportLevel.Info, "Mouse", "click to select the field\r\nMouse Left Click item 'ShipWorksSa.MainPanel.Text' at Center.", repo.ShipWorksSa.MainPanel.TextInfo, new RecordItemIndex(10));
            repo.ShipWorksSa.MainPanel.Text.Click(300);
            Delay.Milliseconds(200);
            
            // enter the password
            Report.Log(ReportLevel.Info, "Keyboard", "enter the password\r\nKey sequence 'ranorex' with focus on 'ShipWorksSa.MainPanel.Text'.", repo.ShipWorksSa.MainPanel.TextInfo, new RecordItemIndex(11));
            repo.ShipWorksSa.MainPanel.Text.PressKeys("ranorex");
            Delay.Milliseconds(0);
            
            // move mouse to enter module url
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to enter module url\r\nMouse Left Move item 'ShipWorksSa.MainPanel.ModuleURL' at Center.", repo.ShipWorksSa.MainPanel.ModuleURLInfo, new RecordItemIndex(12));
            repo.ShipWorksSa.MainPanel.ModuleURL.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click to select the field
            Report.Log(ReportLevel.Info, "Mouse", "click to select the field\r\nMouse Left Click item 'ShipWorksSa.MainPanel.ModuleURL' at Center.", repo.ShipWorksSa.MainPanel.ModuleURLInfo, new RecordItemIndex(13));
            repo.ShipWorksSa.MainPanel.ModuleURL.Click(300);
            Delay.Milliseconds(200);
            
            // enter the url
            Report.Log(ReportLevel.Info, "Keyboard", "enter the url\r\nKey sequence 'http://devsandbox:8880/api/ranorex_store'.", new RecordItemIndex(14));
            Keyboard.Press("http://devsandbox:8880/api/ranorex_store");
            Delay.Milliseconds(0);
            
            // move mouse to next button
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to next button\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(15));
            repo.AddStoreWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click next
            Report.Log(ReportLevel.Info, "Mouse", "click next\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(16));
            repo.AddStoreWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // move mouse to next button
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to next button\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(17));
            repo.AddStoreWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click next
            Report.Log(ReportLevel.Info, "Mouse", "click next\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(18));
            repo.AddStoreWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // move mouse to next button
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to next button\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(19));
            repo.AddStoreWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click next
            Report.Log(ReportLevel.Info, "Mouse", "click next\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(20));
            repo.AddStoreWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // move mouse to next button
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to next button\r\nMouse Left Move item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(21));
            repo.AddStoreWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click next
            Report.Log(ReportLevel.Info, "Mouse", "click next\r\nMouse Left Click item 'AddStoreWizard.Next' at Center.", repo.AddStoreWizard.NextInfo, new RecordItemIndex(22));
            repo.AddStoreWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // move mouse to next finish
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to next finish\r\nMouse Left Move item 'AddStoreWizard.Finish' at Center.", repo.AddStoreWizard.FinishInfo, new RecordItemIndex(23));
            repo.AddStoreWizard.Finish.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click finish
            Report.Log(ReportLevel.Info, "Mouse", "click finish\r\nMouse Left Click item 'AddStoreWizard.Finish' at Center.", repo.AddStoreWizard.FinishInfo, new RecordItemIndex(24));
            repo.AddStoreWizard.Finish.Click(300);
            Delay.Milliseconds(200);
            
            // move to start using shipworks
            Report.Log(ReportLevel.Info, "Mouse", "move to start using shipworks\r\nMouse Left Move item 'WhatsNext.Image' at Center.", repo.WhatsNext.ImageInfo, new RecordItemIndex(25));
            repo.WhatsNext.Image.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click start using shipworks
            Report.Log(ReportLevel.Info, "Mouse", "click start using shipworks\r\nMouse Left Click item 'WhatsNext.Image' at Center.", repo.WhatsNext.ImageInfo, new RecordItemIndex(26));
            repo.WhatsNext.Image.Click(300);
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
