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
    ///The ChooseUPS10KGBox recording.
    /// </summary>
    [TestModule("aa3eca8a-1809-4b05-a8b0-58f9869cbd49", ModuleType.Recording, 1)]
    public partial class ChooseUPS10KGBox : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ShippingCarrierRegressionRepository repository.
        /// </summary>
        public static ShippingCarrierRegressionRepository repo = ShippingCarrierRegressionRepository.Instance;

        static ChooseUPS10KGBox instance = new ChooseUPS10KGBox();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ChooseUPS10KGBox()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ChooseUPS10KGBox Instance
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

            // Focus on package type
            Report.Log(ReportLevel.Info, "Invoke Action", "Focus on package type\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.PackagingType'.", repo.ShippingDlg.SplitContainer.PackagingTypeInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.PackagingType.Focus();
            Delay.Milliseconds(0);
            
            // Move to package type
            Report.Log(ReportLevel.Info, "Mouse", "Move to package type\r\nMouse Left Move item 'ShippingDlg.SplitContainer.PackagingType' at Center.", repo.ShippingDlg.SplitContainer.PackagingTypeInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.PackagingType.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Packaging: FedEx Large Box
            Report.Log(ReportLevel.Info, "Set Value", "Set Packaging: FedEx Large Box\r\nSetting attribute SelectedItemText to 'UPS 10 KG Box®' on item 'ShippingDlg.SplitContainer.PackagingType'.", repo.ShippingDlg.SplitContainer.PackagingTypeInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.PackagingType.Element.SetAttributeValue("SelectedItemText", "UPS 10 KG Box®");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
