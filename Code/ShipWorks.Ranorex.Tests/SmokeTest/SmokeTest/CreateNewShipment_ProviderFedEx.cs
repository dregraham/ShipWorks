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
    ///The CreateNewShipment_ProviderFedEx recording.
    /// </summary>
    [TestModule("3abfd772-9a86-4cce-af3b-d6c68431f012", ModuleType.Recording, 1)]
    public partial class CreateNewShipment_ProviderFedEx : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static CreateNewShipment_ProviderFedEx instance = new CreateNewShipment_ProviderFedEx();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CreateNewShipment_ProviderFedEx()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static CreateNewShipment_ProviderFedEx Instance
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

            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CreateNew' at Center.", repo.ShippingDlg.SplitContainer.CreateNewInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.CreateNew.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Create New to create new shipment
            Report.Log(ReportLevel.Info, "Mouse", "Click Create New to create new shipment\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CreateNew' at Center.", repo.ShippingDlg.SplitContainer.CreateNewInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.CreateNew.Click(300);
            Delay.Milliseconds(200);
            
            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Set Provider: FedEx
            Report.Log(ReportLevel.Info, "Set value", "Set Provider: FedEx\r\nSetting attribute SelectedItemText to 'FedEx' on item 'ShippingDlg.SplitContainer.ComboShipmentType'.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.Element.SetAttributeValue("SelectedItemText", "FedEx");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
