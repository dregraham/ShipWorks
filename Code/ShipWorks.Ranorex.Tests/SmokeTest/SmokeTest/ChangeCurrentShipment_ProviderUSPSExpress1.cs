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
    ///The ChangeCurrentShipment_ProviderUSPSExpress1 recording.
    /// </summary>
    [TestModule("f20599ee-ff69-4d67-aba1-b081da870a75", ModuleType.Recording, 1)]
    public partial class ChangeCurrentShipment_ProviderUSPSExpress1 : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static ChangeCurrentShipment_ProviderUSPSExpress1 instance = new ChangeCurrentShipment_ProviderUSPSExpress1();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ChangeCurrentShipment_ProviderUSPSExpress1()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ChangeCurrentShipment_ProviderUSPSExpress1 Instance
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

            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Provider Dropdown
            Report.Log(ReportLevel.Info, "Mouse", "Click Provider Dropdown\r\nMouse Left Click item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'List1000.USPSExpress1' at Center.", repo.List1000.USPSExpress1Info, new RecordItemIndex(2));
            repo.List1000.USPSExpress1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click USPS (Express 1) in provider dropdown
            Report.Log(ReportLevel.Info, "Mouse", "Click USPS (Express 1) in provider dropdown\r\nMouse Left Click item 'List1000.USPSExpress1' at Center.", repo.List1000.USPSExpress1Info, new RecordItemIndex(3));
            repo.List1000.USPSExpress1.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
