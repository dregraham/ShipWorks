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

namespace ShippingCarrierRegression
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The CreateNewShipment_ProviderUSPSNoPostage recording.
    /// </summary>
    [TestModule("e855fe84-a593-4412-96f5-5f96cbfd9694", ModuleType.Recording, 1)]
    public partial class CreateNewShipment_ProviderUSPSNoPostage : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTest.SmokeTestRepository repository.
        /// </summary>
        public static SmokeTest.SmokeTestRepository repo = SmokeTest.SmokeTestRepository.Instance;

        static CreateNewShipment_ProviderUSPSNoPostage instance = new CreateNewShipment_ProviderUSPSNoPostage();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CreateNewShipment_ProviderUSPSNoPostage()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static CreateNewShipment_ProviderUSPSNoPostage Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "7.0")]
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
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "7.0")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CreateNew' at Center.", repo.ShippingDlg.SplitContainer.CreateNewInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.CreateNew.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Click Create New to create new shipment
            Report.Log(ReportLevel.Info, "Mouse", "Click Create New to create new shipment\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CreateNew' at Center.", repo.ShippingDlg.SplitContainer.CreateNewInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.CreateNew.Click();
            Delay.Milliseconds(200);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.MoveTo(1000);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.Click();
            Delay.Milliseconds(200);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'List1000.USPSWOPostage' at Center.", repo.List1000.USPSWOPostageInfo, new RecordItemIndex(4));
            repo.List1000.USPSWOPostage.MoveTo(1000);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'List1000.USPSWOPostage' at 149;9.", repo.List1000.USPSWOPostageInfo, new RecordItemIndex(5));
            repo.List1000.USPSWOPostage.Click("149;9");
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
