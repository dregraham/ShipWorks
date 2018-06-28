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
using System.Drawing;
using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace SoakPerformanceTest
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    /// The class representing the SoakPerformanceTestRepository element repository.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Ranorex", "5.4.5")]
    [RepositoryFolder("ae0c01ae-9da3-4f2c-95c5-6e24580e0462")]
    public partial class SoakPerformanceTestRepository : RepoGenBaseFolder
    {
        static SoakPerformanceTestRepository instance = new SoakPerformanceTestRepository();
        SoakPerformanceTestRepositoryFolders.MainFormAppFolder _mainform;
        SoakPerformanceTestRepositoryFolders.ShipWorksSaAppFolder _shipworkssa;
        SoakPerformanceTestRepositoryFolders.ShippingDlgAppFolder _shippingdlg;
        SoakPerformanceTestRepositoryFolders.ProcessingShipmentsAppFolder _processingshipments;
        SoakPerformanceTestRepositoryFolders.ShipmentVoidConfirmDlgAppFolder _shipmentvoidconfirmdlg;
        SoakPerformanceTestRepositoryFolders.ShipOrders1AppFolder _shiporders1;

        /// <summary>
        /// Gets the singleton class instance representing the SoakPerformanceTestRepository element repository.
        /// </summary>
        [RepositoryFolder("ae0c01ae-9da3-4f2c-95c5-6e24580e0462")]
        public static SoakPerformanceTestRepository Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Repository class constructor.
        /// </summary>
        public SoakPerformanceTestRepository() 
            : base("SoakPerformanceTestRepository", "/", null, 0, false, "ae0c01ae-9da3-4f2c-95c5-6e24580e0462", ".\\RepositoryImages\\SoakPerformanceTestRepositoryae0c01ae.rximgres")
        {
            _mainform = new SoakPerformanceTestRepositoryFolders.MainFormAppFolder(this);
            _shipworkssa = new SoakPerformanceTestRepositoryFolders.ShipWorksSaAppFolder(this);
            _shippingdlg = new SoakPerformanceTestRepositoryFolders.ShippingDlgAppFolder(this);
            _processingshipments = new SoakPerformanceTestRepositoryFolders.ProcessingShipmentsAppFolder(this);
            _shipmentvoidconfirmdlg = new SoakPerformanceTestRepositoryFolders.ShipmentVoidConfirmDlgAppFolder(this);
            _shiporders1 = new SoakPerformanceTestRepositoryFolders.ShipOrders1AppFolder(this);
        }

#region Variables

#endregion

        /// <summary>
        /// The Self item info.
        /// </summary>
        [RepositoryItemInfo("ae0c01ae-9da3-4f2c-95c5-6e24580e0462")]
        public virtual RepoItemInfo SelfInfo
        {
            get
            {
                return _selfInfo;
            }
        }

        /// <summary>
        /// The MainForm folder.
        /// </summary>
        [RepositoryFolder("71f1d0dd-546a-432a-ac42-88cfd0006fb8")]
        public virtual SoakPerformanceTestRepositoryFolders.MainFormAppFolder MainForm
        {
            get { return _mainform; }
        }

        /// <summary>
        /// The ShipWorksSa folder.
        /// </summary>
        [RepositoryFolder("aeb53ba1-63cb-471b-9d4a-56f30723084d")]
        public virtual SoakPerformanceTestRepositoryFolders.ShipWorksSaAppFolder ShipWorksSa
        {
            get { return _shipworkssa; }
        }

        /// <summary>
        /// The ShippingDlg folder.
        /// </summary>
        [RepositoryFolder("3a1c1565-c514-4ede-8bc5-43f2092a0b0f")]
        public virtual SoakPerformanceTestRepositoryFolders.ShippingDlgAppFolder ShippingDlg
        {
            get { return _shippingdlg; }
        }

        /// <summary>
        /// The ProcessingShipments folder.
        /// </summary>
        [RepositoryFolder("e3fe3159-df93-4283-8ab8-9ac88cc694c8")]
        public virtual SoakPerformanceTestRepositoryFolders.ProcessingShipmentsAppFolder ProcessingShipments
        {
            get { return _processingshipments; }
        }

        /// <summary>
        /// The ShipmentVoidConfirmDlg folder.
        /// </summary>
        [RepositoryFolder("317a58e1-9f54-44ee-81d3-cf8a28f54a0c")]
        public virtual SoakPerformanceTestRepositoryFolders.ShipmentVoidConfirmDlgAppFolder ShipmentVoidConfirmDlg
        {
            get { return _shipmentvoidconfirmdlg; }
        }

        /// <summary>
        /// The ShipOrders1 folder.
        /// </summary>
        [RepositoryFolder("475f83f3-fda2-444a-af63-31ff8426836c")]
        public virtual SoakPerformanceTestRepositoryFolders.ShipOrders1AppFolder ShipOrders1
        {
            get { return _shiporders1; }
        }
    }

    /// <summary>
    /// Inner folder classes.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Ranorex", "5.4.5")]
    public partial class SoakPerformanceTestRepositoryFolders
    {
        /// <summary>
        /// The MainFormAppFolder folder.
        /// </summary>
        [RepositoryFolder("71f1d0dd-546a-432a-ac42-88cfd0006fb8")]
        public partial class MainFormAppFolder : RepoGenBaseFolder
        {
            RepoItemInfo _homeInfo;
            RepoItemInfo _manageInfo;

            /// <summary>
            /// Creates a new MainForm  folder.
            /// </summary>
            public MainFormAppFolder(RepoGenBaseFolder parentFolder) :
                    base("MainForm", "/form[@controlname='MainForm']", parentFolder, 30000, null, true, "71f1d0dd-546a-432a-ac42-88cfd0006fb8", "")
            {
                _homeInfo = new RepoItemInfo(this, "Home", "?/?/rawtext", 30000, null, "d424a31b-6c02-46dd-9a6b-889d444bc468");
                _manageInfo = new RepoItemInfo(this, "Manage", "?/?/rawtext[@rawtext='Manage']", 30000, null, "e3bd119c-0b4a-4512-898d-f804eb8dc2a0");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("71f1d0dd-546a-432a-ac42-88cfd0006fb8")]
            public virtual Ranorex.Form Self
            {
                get
                {
                    return _selfInfo.CreateAdapter<Ranorex.Form>(true);
                }
            }

            /// <summary>
            /// The Self item info.
            /// </summary>
            [RepositoryItemInfo("71f1d0dd-546a-432a-ac42-88cfd0006fb8")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The Home item.
            /// </summary>
            [RepositoryItem("d424a31b-6c02-46dd-9a6b-889d444bc468")]
            public virtual Ranorex.RawText Home
            {
                get
                {
                    return _homeInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The Home item info.
            /// </summary>
            [RepositoryItemInfo("d424a31b-6c02-46dd-9a6b-889d444bc468")]
            public virtual RepoItemInfo HomeInfo
            {
                get
                {
                    return _homeInfo;
                }
            }

            /// <summary>
            /// The Manage item.
            /// </summary>
            [RepositoryItem("e3bd119c-0b4a-4512-898d-f804eb8dc2a0")]
            public virtual Ranorex.RawText Manage
            {
                get
                {
                    return _manageInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The Manage item info.
            /// </summary>
            [RepositoryItemInfo("e3bd119c-0b4a-4512-898d-f804eb8dc2a0")]
            public virtual RepoItemInfo ManageInfo
            {
                get
                {
                    return _manageInfo;
                }
            }
        }

        /// <summary>
        /// The ShipWorksSaAppFolder folder.
        /// </summary>
        [RepositoryFolder("aeb53ba1-63cb-471b-9d4a-56f30723084d")]
        public partial class ShipWorksSaAppFolder : RepoGenBaseFolder
        {
            RepoItemInfo _optionsInfo;
            RepoItemInfo _interapptiveInfo;
            RepoItemInfo _upsonlinetoolsInfo;
            RepoItemInfo _buttonokInfo;
            RepoItemInfo _someerrorsoccurredduringprocessingInfo;
            RepoItemInfo _buttonok1Info;
            RepoItemInfo _agreeInfo;

            /// <summary>
            /// Creates a new ShipWorksSa  folder.
            /// </summary>
            public ShipWorksSaAppFolder(RepoGenBaseFolder parentFolder) :
                    base("ShipWorksSa", "/form[@title~'ShipWorks']", parentFolder, 30000, null, true, "aeb53ba1-63cb-471b-9d4a-56f30723084d", "")
            {
                _optionsInfo = new RepoItemInfo(this, "Options", "?/?/element[@controlname='ribbonTabAdmin']/rawtext[@rawtext='Options']", 30000, null, "33ee4d4a-bd8d-45c7-ae4c-96728d7f6cd7");
                _interapptiveInfo = new RepoItemInfo(this, "Interapptive", "list[@controlname='menuList']/?/?/listitem[4]", 30000, null, "149125f8-0c2b-4b88-baee-aead158d19bb");
                _upsonlinetoolsInfo = new RepoItemInfo(this, "UpsOnLineTools", "container[@controlname='sectionContainer']/?/?/checkbox[@controlname='upsOnLineTools']", 30000, null, "acae23e1-e515-4c13-8eae-431387f0d31d");
                _buttonokInfo = new RepoItemInfo(this, "ButtonOk", "button[@controlname='ok']", 30000, null, "3234fa52-fa07-4acf-beb1-95d8dd1ff50c");
                _someerrorsoccurredduringprocessingInfo = new RepoItemInfo(this, "SomeErrorsOccurredDuringProcessing", "?/?/rawtext[@rawtext='Some errors occurred during processing.Order 4-M: The message with Action ''http://tempuri.org/IRateService/GetInternationalRateZoneWithPostCode'' cannot be processed at the receiver, due to a ContractFilter mismatch at the EndpointDispatcher. This may be because of either a contract mismatch (mismatched Actions between sender and receiver) or a binding/security mismatch between the sender and the receiver.  Check that sender and receiver have the same contract and the same binding (including security requirements, e.g. Message, Transport, None).']", 30000, null, "85f2efe5-3a5b-4802-a83f-83b211019b65");
                _buttonok1Info = new RepoItemInfo(this, "ButtonOK1", "button[@text='OK']", 30000, null, "2a5bd0f8-658e-455f-afc5-62338ee86eea");
                _agreeInfo = new RepoItemInfo(this, "Agree", "checkbox[@controlname='agree']", 30000, null, "0abc6c33-e0e7-45a6-ad5a-7773294ac9a1");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("aeb53ba1-63cb-471b-9d4a-56f30723084d")]
            public virtual Ranorex.Form Self
            {
                get
                {
                    return _selfInfo.CreateAdapter<Ranorex.Form>(true);
                }
            }

            /// <summary>
            /// The Self item info.
            /// </summary>
            [RepositoryItemInfo("aeb53ba1-63cb-471b-9d4a-56f30723084d")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The Options item.
            /// </summary>
            [RepositoryItem("33ee4d4a-bd8d-45c7-ae4c-96728d7f6cd7")]
            public virtual Ranorex.RawText Options
            {
                get
                {
                    return _optionsInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The Options item info.
            /// </summary>
            [RepositoryItemInfo("33ee4d4a-bd8d-45c7-ae4c-96728d7f6cd7")]
            public virtual RepoItemInfo OptionsInfo
            {
                get
                {
                    return _optionsInfo;
                }
            }

            /// <summary>
            /// The Interapptive item.
            /// </summary>
            [RepositoryItem("149125f8-0c2b-4b88-baee-aead158d19bb")]
            public virtual Ranorex.ListItem Interapptive
            {
                get
                {
                    return _interapptiveInfo.CreateAdapter<Ranorex.ListItem>(true);
                }
            }

            /// <summary>
            /// The Interapptive item info.
            /// </summary>
            [RepositoryItemInfo("149125f8-0c2b-4b88-baee-aead158d19bb")]
            public virtual RepoItemInfo InterapptiveInfo
            {
                get
                {
                    return _interapptiveInfo;
                }
            }

            /// <summary>
            /// The UpsOnLineTools item.
            /// </summary>
            [RepositoryItem("acae23e1-e515-4c13-8eae-431387f0d31d")]
            public virtual Ranorex.CheckBox UpsOnLineTools
            {
                get
                {
                    return _upsonlinetoolsInfo.CreateAdapter<Ranorex.CheckBox>(true);
                }
            }

            /// <summary>
            /// The UpsOnLineTools item info.
            /// </summary>
            [RepositoryItemInfo("acae23e1-e515-4c13-8eae-431387f0d31d")]
            public virtual RepoItemInfo UpsOnLineToolsInfo
            {
                get
                {
                    return _upsonlinetoolsInfo;
                }
            }

            /// <summary>
            /// The ButtonOk item.
            /// </summary>
            [RepositoryItem("3234fa52-fa07-4acf-beb1-95d8dd1ff50c")]
            public virtual Ranorex.Button ButtonOk
            {
                get
                {
                    return _buttonokInfo.CreateAdapter<Ranorex.Button>(true);
                }
            }

            /// <summary>
            /// The ButtonOk item info.
            /// </summary>
            [RepositoryItemInfo("3234fa52-fa07-4acf-beb1-95d8dd1ff50c")]
            public virtual RepoItemInfo ButtonOkInfo
            {
                get
                {
                    return _buttonokInfo;
                }
            }

            /// <summary>
            /// The SomeErrorsOccurredDuringProcessing item.
            /// </summary>
            [RepositoryItem("85f2efe5-3a5b-4802-a83f-83b211019b65")]
            public virtual Ranorex.RawText SomeErrorsOccurredDuringProcessing
            {
                get
                {
                    return _someerrorsoccurredduringprocessingInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The SomeErrorsOccurredDuringProcessing item info.
            /// </summary>
            [RepositoryItemInfo("85f2efe5-3a5b-4802-a83f-83b211019b65")]
            public virtual RepoItemInfo SomeErrorsOccurredDuringProcessingInfo
            {
                get
                {
                    return _someerrorsoccurredduringprocessingInfo;
                }
            }

            /// <summary>
            /// The ButtonOK1 item.
            /// </summary>
            [RepositoryItem("2a5bd0f8-658e-455f-afc5-62338ee86eea")]
            public virtual Ranorex.Button ButtonOK1
            {
                get
                {
                    return _buttonok1Info.CreateAdapter<Ranorex.Button>(true);
                }
            }

            /// <summary>
            /// The ButtonOK1 item info.
            /// </summary>
            [RepositoryItemInfo("2a5bd0f8-658e-455f-afc5-62338ee86eea")]
            public virtual RepoItemInfo ButtonOK1Info
            {
                get
                {
                    return _buttonok1Info;
                }
            }

            /// <summary>
            /// The Agree item.
            /// </summary>
            [RepositoryItem("0abc6c33-e0e7-45a6-ad5a-7773294ac9a1")]
            public virtual Ranorex.CheckBox Agree
            {
                get
                {
                    return _agreeInfo.CreateAdapter<Ranorex.CheckBox>(true);
                }
            }

            /// <summary>
            /// The Agree item info.
            /// </summary>
            [RepositoryItemInfo("0abc6c33-e0e7-45a6-ad5a-7773294ac9a1")]
            public virtual RepoItemInfo AgreeInfo
            {
                get
                {
                    return _agreeInfo;
                }
            }
        }

        /// <summary>
        /// The ShippingDlgAppFolder folder.
        /// </summary>
        [RepositoryFolder("3a1c1565-c514-4ede-8bc5-43f2092a0b0f")]
        public partial class ShippingDlgAppFolder : RepoGenBaseFolder
        {
            RepoItemInfo _createlabelInfo;
            RepoItemInfo _voidselectedInfo;
            RepoItemInfo _closeInfo;

            /// <summary>
            /// Creates a new ShippingDlg  folder.
            /// </summary>
            public ShippingDlgAppFolder(RepoGenBaseFolder parentFolder) :
                    base("ShippingDlg", "/form[@controlname='ShippingDlg']", parentFolder, 30000, null, true, "3a1c1565-c514-4ede-8bc5-43f2092a0b0f", "")
            {
                _createlabelInfo = new RepoItemInfo(this, "CreateLabel", "?/?/button[@controlname='processDropDownButton']/rawtext[@rawtext='Create Label']", 30000, null, "28994b08-f9e2-4fb6-b17d-0d2dcac53f13");
                _voidselectedInfo = new RepoItemInfo(this, "VoidSelected", "?/?/button[@controlname='voidSelected']", 30000, null, "c7deec03-acbc-41df-ab75-ee66434d60b1");
                _closeInfo = new RepoItemInfo(this, "Close", "button[@controlname='close']", 30000, null, "4df7c55c-256c-4aa4-bfbd-290cf80709aa");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("3a1c1565-c514-4ede-8bc5-43f2092a0b0f")]
            public virtual Ranorex.Form Self
            {
                get
                {
                    return _selfInfo.CreateAdapter<Ranorex.Form>(true);
                }
            }

            /// <summary>
            /// The Self item info.
            /// </summary>
            [RepositoryItemInfo("3a1c1565-c514-4ede-8bc5-43f2092a0b0f")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The CreateLabel item.
            /// </summary>
            [RepositoryItem("28994b08-f9e2-4fb6-b17d-0d2dcac53f13")]
            public virtual Ranorex.RawText CreateLabel
            {
                get
                {
                    return _createlabelInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The CreateLabel item info.
            /// </summary>
            [RepositoryItemInfo("28994b08-f9e2-4fb6-b17d-0d2dcac53f13")]
            public virtual RepoItemInfo CreateLabelInfo
            {
                get
                {
                    return _createlabelInfo;
                }
            }

            /// <summary>
            /// The VoidSelected item.
            /// </summary>
            [RepositoryItem("c7deec03-acbc-41df-ab75-ee66434d60b1")]
            public virtual Ranorex.Button VoidSelected
            {
                get
                {
                    return _voidselectedInfo.CreateAdapter<Ranorex.Button>(true);
                }
            }

            /// <summary>
            /// The VoidSelected item info.
            /// </summary>
            [RepositoryItemInfo("c7deec03-acbc-41df-ab75-ee66434d60b1")]
            public virtual RepoItemInfo VoidSelectedInfo
            {
                get
                {
                    return _voidselectedInfo;
                }
            }

            /// <summary>
            /// The Close item.
            /// </summary>
            [RepositoryItem("4df7c55c-256c-4aa4-bfbd-290cf80709aa")]
            public virtual Ranorex.Button Close
            {
                get
                {
                    return _closeInfo.CreateAdapter<Ranorex.Button>(true);
                }
            }

            /// <summary>
            /// The Close item info.
            /// </summary>
            [RepositoryItemInfo("4df7c55c-256c-4aa4-bfbd-290cf80709aa")]
            public virtual RepoItemInfo CloseInfo
            {
                get
                {
                    return _closeInfo;
                }
            }
        }

        /// <summary>
        /// The ProcessingShipmentsAppFolder folder.
        /// </summary>
        [RepositoryFolder("e3fe3159-df93-4283-8ab8-9ac88cc694c8")]
        public partial class ProcessingShipmentsAppFolder : RepoGenBaseFolder
        {
            RepoItemInfo _processingshipmentsInfo;
            RepoItemInfo _voidingshipmentsInfo;

            /// <summary>
            /// Creates a new ProcessingShipments  folder.
            /// </summary>
            public ProcessingShipmentsAppFolder(RepoGenBaseFolder parentFolder) :
                    base("ProcessingShipments", "/form[@title=' ']", parentFolder, 30000, null, true, "e3fe3159-df93-4283-8ab8-9ac88cc694c8", "")
            {
                _processingshipmentsInfo = new RepoItemInfo(this, "ProcessingShipments", "container[@controlname='headerPanel']/?/?/rawtext[@rawtext='Processing Shipments']", 30000, null, "1eeab451-063b-4d2e-bd8c-94563bb3ea8f");
                _voidingshipmentsInfo = new RepoItemInfo(this, "VoidingShipments", "container[@controlname='headerPanel']/?/?/rawtext[@rawtext='Voiding Shipments']", 30000, null, "17128e3b-af49-4aac-993a-5e8dc65f3afd");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("e3fe3159-df93-4283-8ab8-9ac88cc694c8")]
            public virtual Ranorex.Form Self
            {
                get
                {
                    return _selfInfo.CreateAdapter<Ranorex.Form>(true);
                }
            }

            /// <summary>
            /// The Self item info.
            /// </summary>
            [RepositoryItemInfo("e3fe3159-df93-4283-8ab8-9ac88cc694c8")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The ProcessingShipments item.
            /// </summary>
            [RepositoryItem("1eeab451-063b-4d2e-bd8c-94563bb3ea8f")]
            public virtual Ranorex.RawText ProcessingShipments
            {
                get
                {
                    return _processingshipmentsInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The ProcessingShipments item info.
            /// </summary>
            [RepositoryItemInfo("1eeab451-063b-4d2e-bd8c-94563bb3ea8f")]
            public virtual RepoItemInfo ProcessingShipmentsInfo
            {
                get
                {
                    return _processingshipmentsInfo;
                }
            }

            /// <summary>
            /// The VoidingShipments item.
            /// </summary>
            [RepositoryItem("17128e3b-af49-4aac-993a-5e8dc65f3afd")]
            public virtual Ranorex.RawText VoidingShipments
            {
                get
                {
                    return _voidingshipmentsInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The VoidingShipments item info.
            /// </summary>
            [RepositoryItemInfo("17128e3b-af49-4aac-993a-5e8dc65f3afd")]
            public virtual RepoItemInfo VoidingShipmentsInfo
            {
                get
                {
                    return _voidingshipmentsInfo;
                }
            }
        }

        /// <summary>
        /// The ShipmentVoidConfirmDlgAppFolder folder.
        /// </summary>
        [RepositoryFolder("317a58e1-9f54-44ee-81d3-cf8a28f54a0c")]
        public partial class ShipmentVoidConfirmDlgAppFolder : RepoGenBaseFolder
        {
            RepoItemInfo _buttonokInfo;

            /// <summary>
            /// Creates a new ShipmentVoidConfirmDlg  folder.
            /// </summary>
            public ShipmentVoidConfirmDlgAppFolder(RepoGenBaseFolder parentFolder) :
                    base("ShipmentVoidConfirmDlg", "/form[@controlname='ShipmentVoidConfirmDlg']", parentFolder, 30000, null, true, "317a58e1-9f54-44ee-81d3-cf8a28f54a0c", "")
            {
                _buttonokInfo = new RepoItemInfo(this, "ButtonOk", "button[@controlname='ok']", 30000, null, "7045aaaf-3f69-4ffe-b27f-8c2c05d9190f");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("317a58e1-9f54-44ee-81d3-cf8a28f54a0c")]
            public virtual Ranorex.Form Self
            {
                get
                {
                    return _selfInfo.CreateAdapter<Ranorex.Form>(true);
                }
            }

            /// <summary>
            /// The Self item info.
            /// </summary>
            [RepositoryItemInfo("317a58e1-9f54-44ee-81d3-cf8a28f54a0c")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The ButtonOk item.
            /// </summary>
            [RepositoryItem("7045aaaf-3f69-4ffe-b27f-8c2c05d9190f")]
            public virtual Ranorex.Button ButtonOk
            {
                get
                {
                    return _buttonokInfo.CreateAdapter<Ranorex.Button>(true);
                }
            }

            /// <summary>
            /// The ButtonOk item info.
            /// </summary>
            [RepositoryItemInfo("7045aaaf-3f69-4ffe-b27f-8c2c05d9190f")]
            public virtual RepoItemInfo ButtonOkInfo
            {
                get
                {
                    return _buttonokInfo;
                }
            }
        }

        /// <summary>
        /// The ShipOrders1AppFolder folder.
        /// </summary>
        [RepositoryFolder("475f83f3-fda2-444a-af63-31ff8426836c")]
        public partial class ShipOrders1AppFolder : RepoGenBaseFolder
        {
            SoakPerformanceTestRepositoryFolders.SplitContainerFolder _splitcontainer;

            /// <summary>
            /// Creates a new ShipOrders1  folder.
            /// </summary>
            public ShipOrders1AppFolder(RepoGenBaseFolder parentFolder) :
                    base("ShipOrders1", "/form[@title='Ship Orders']", parentFolder, 30000, null, true, "475f83f3-fda2-444a-af63-31ff8426836c", "")
            {
                _splitcontainer = new SoakPerformanceTestRepositoryFolders.SplitContainerFolder(this);
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("475f83f3-fda2-444a-af63-31ff8426836c")]
            public virtual Ranorex.Form Self
            {
                get
                {
                    return _selfInfo.CreateAdapter<Ranorex.Form>(true);
                }
            }

            /// <summary>
            /// The Self item info.
            /// </summary>
            [RepositoryItemInfo("475f83f3-fda2-444a-af63-31ff8426836c")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The SplitContainer folder.
            /// </summary>
            [RepositoryFolder("b9ae4bfa-cc8a-4316-8ae4-efea8c187b05")]
            public virtual SoakPerformanceTestRepositoryFolders.SplitContainerFolder SplitContainer
            {
                get { return _splitcontainer; }
            }
        }

        /// <summary>
        /// The SplitContainerFolder folder.
        /// </summary>
        [RepositoryFolder("b9ae4bfa-cc8a-4316-8ae4-efea8c187b05")]
        public partial class SplitContainerFolder : RepoGenBaseFolder
        {
            RepoItemInfo _carrierInfo;
            RepoItemInfo _service1Info;
            RepoItemInfo _weightotherInfo;
            RepoItemInfo _costInfo;
            RepoItemInfo _trackinghashInfo;
            RepoItemInfo _useinsuranceInfo;
            RepoItemInfo _insuredvalueInfo;

            /// <summary>
            /// Creates a new SplitContainer  folder.
            /// </summary>
            public SplitContainerFolder(RepoGenBaseFolder parentFolder) :
                    base("SplitContainer", "container[@controlname='splitContainer']", parentFolder, 30000, null, false, "b9ae4bfa-cc8a-4316-8ae4-efea8c187b05", "")
            {
                _carrierInfo = new RepoItemInfo(this, "Carrier", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/container[@controlname='OtherServiceControl']/container[@controlname='sectionShipment']//text[@controlname='carrier']", 30000, null, "b92f79a9-0339-45ff-8fc1-fbb908f1a6aa");
                _service1Info = new RepoItemInfo(this, "Service1", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/container[@controlname='OtherServiceControl']/container[@controlname='sectionShipment']//text[@controlname='service']", 30000, null, "aebd4219-3f0d-449f-a528-215787c2b72a");
                _weightotherInfo = new RepoItemInfo(this, "WeightOther", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/container[@controlname='OtherServiceControl']/container[@controlname='sectionShipment']//container[@controlname='weight']/text[@controlname='textBox']", 30000, null, "b511ec5c-c40b-4a1a-a15b-59817391caf0");
                _costInfo = new RepoItemInfo(this, "Cost", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/container[@controlname='OtherServiceControl']/container[@controlname='sectionShipment']//text[@controlname='cost']", 30000, null, "8cdb8673-4082-474b-949a-d54a4b913abb");
                _trackinghashInfo = new RepoItemInfo(this, "TrackingHash", "?/?/container[@controlname='ratesSplitContainer']/container[@controlname='panel1']/?/?/tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/?/container[@controlname='sectionShipment']//text[@controlname='tracking']/text[@accessiblename='Tracking #:']", 30000, null, "8e83cd91-1699-43c7-83dd-271f30857ab2");
                _useinsuranceInfo = new RepoItemInfo(this, "UseInsurance", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/container[@controlname='OtherServiceControl']/container[@controlname='sectionShipment']//container[@controlname='insuranceControl']/checkbox[@controlname='useInsurance']", 30000, null, "c8a53428-cb4c-4d98-957a-5c4a69601c13");
                _insuredvalueInfo = new RepoItemInfo(this, "InsuredValue", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/?/container[@controlname='sectionShipment']//container[@controlname='insuranceControl']/text[@controlname='insuredValue']/text[@accessiblename='Insured value:']", 30000, null, "9032f7f4-d19e-4fed-ae81-9055d43143f9");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("b9ae4bfa-cc8a-4316-8ae4-efea8c187b05")]
            public virtual Ranorex.Container Self
            {
                get
                {
                    return _selfInfo.CreateAdapter<Ranorex.Container>(true);
                }
            }

            /// <summary>
            /// The Self item info.
            /// </summary>
            [RepositoryItemInfo("b9ae4bfa-cc8a-4316-8ae4-efea8c187b05")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The Carrier item.
            /// </summary>
            [RepositoryItem("b92f79a9-0339-45ff-8fc1-fbb908f1a6aa")]
            public virtual Ranorex.Text Carrier
            {
                get
                {
                    return _carrierInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The Carrier item info.
            /// </summary>
            [RepositoryItemInfo("b92f79a9-0339-45ff-8fc1-fbb908f1a6aa")]
            public virtual RepoItemInfo CarrierInfo
            {
                get
                {
                    return _carrierInfo;
                }
            }

            /// <summary>
            /// The Service1 item.
            /// </summary>
            [RepositoryItem("aebd4219-3f0d-449f-a528-215787c2b72a")]
            public virtual Ranorex.Text Service1
            {
                get
                {
                    return _service1Info.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The Service1 item info.
            /// </summary>
            [RepositoryItemInfo("aebd4219-3f0d-449f-a528-215787c2b72a")]
            public virtual RepoItemInfo Service1Info
            {
                get
                {
                    return _service1Info;
                }
            }

            /// <summary>
            /// The WeightOther item.
            /// </summary>
            [RepositoryItem("b511ec5c-c40b-4a1a-a15b-59817391caf0")]
            public virtual Ranorex.Text WeightOther
            {
                get
                {
                    return _weightotherInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The WeightOther item info.
            /// </summary>
            [RepositoryItemInfo("b511ec5c-c40b-4a1a-a15b-59817391caf0")]
            public virtual RepoItemInfo WeightOtherInfo
            {
                get
                {
                    return _weightotherInfo;
                }
            }

            /// <summary>
            /// The Cost item.
            /// </summary>
            [RepositoryItem("8cdb8673-4082-474b-949a-d54a4b913abb")]
            public virtual Ranorex.Text Cost
            {
                get
                {
                    return _costInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The Cost item info.
            /// </summary>
            [RepositoryItemInfo("8cdb8673-4082-474b-949a-d54a4b913abb")]
            public virtual RepoItemInfo CostInfo
            {
                get
                {
                    return _costInfo;
                }
            }

            /// <summary>
            /// The TrackingHash item.
            /// </summary>
            [RepositoryItem("8e83cd91-1699-43c7-83dd-271f30857ab2")]
            public virtual Ranorex.Text TrackingHash
            {
                get
                {
                    return _trackinghashInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The TrackingHash item info.
            /// </summary>
            [RepositoryItemInfo("8e83cd91-1699-43c7-83dd-271f30857ab2")]
            public virtual RepoItemInfo TrackingHashInfo
            {
                get
                {
                    return _trackinghashInfo;
                }
            }

            /// <summary>
            /// The UseInsurance item.
            /// </summary>
            [RepositoryItem("c8a53428-cb4c-4d98-957a-5c4a69601c13")]
            public virtual Ranorex.CheckBox UseInsurance
            {
                get
                {
                    return _useinsuranceInfo.CreateAdapter<Ranorex.CheckBox>(true);
                }
            }

            /// <summary>
            /// The UseInsurance item info.
            /// </summary>
            [RepositoryItemInfo("c8a53428-cb4c-4d98-957a-5c4a69601c13")]
            public virtual RepoItemInfo UseInsuranceInfo
            {
                get
                {
                    return _useinsuranceInfo;
                }
            }

            /// <summary>
            /// The InsuredValue item.
            /// </summary>
            [RepositoryItem("9032f7f4-d19e-4fed-ae81-9055d43143f9")]
            public virtual Ranorex.Text InsuredValue
            {
                get
                {
                    return _insuredvalueInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The InsuredValue item info.
            /// </summary>
            [RepositoryItemInfo("9032f7f4-d19e-4fed-ae81-9055d43143f9")]
            public virtual RepoItemInfo InsuredValueInfo
            {
                get
                {
                    return _insuredvalueInfo;
                }
            }
        }

    }
#pragma warning restore 0436
}