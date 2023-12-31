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
    /// The class representing the ShippingCarrierRegressionRepository element repository.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Ranorex", "5.4.5")]
    [RepositoryFolder("8b3666b0-4472-487b-baaa-0a31e8982b7d")]
    public partial class ShippingCarrierRegressionRepository : RepoGenBaseFolder
    {
        static ShippingCarrierRegressionRepository instance = new ShippingCarrierRegressionRepository();
        ShippingCarrierRegressionRepositoryFolders.ShipOrders1AppFolder _shiporders1;
        ShippingCarrierRegressionRepositoryFolders.ShippingDlgAppFolder _shippingdlg;

        /// <summary>
        /// Gets the singleton class instance representing the ShippingCarrierRegressionRepository element repository.
        /// </summary>
        [RepositoryFolder("8b3666b0-4472-487b-baaa-0a31e8982b7d")]
        public static ShippingCarrierRegressionRepository Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Repository class constructor.
        /// </summary>
        public ShippingCarrierRegressionRepository() 
            : base("ShippingCarrierRegressionRepository", "/", null, 0, false, "8b3666b0-4472-487b-baaa-0a31e8982b7d", ".\\RepositoryImages\\ShippingCarrierRegressionRepository8b3666b0.rximgres")
        {
            _shiporders1 = new ShippingCarrierRegressionRepositoryFolders.ShipOrders1AppFolder(this);
            _shippingdlg = new ShippingCarrierRegressionRepositoryFolders.ShippingDlgAppFolder(this);
        }

#region Variables

#endregion

        /// <summary>
        /// The Self item info.
        /// </summary>
        [RepositoryItemInfo("8b3666b0-4472-487b-baaa-0a31e8982b7d")]
        public virtual RepoItemInfo SelfInfo
        {
            get
            {
                return _selfInfo;
            }
        }

        /// <summary>
        /// The ShipOrders1 folder.
        /// </summary>
        [RepositoryFolder("b316b72b-5681-4152-85b9-e567c20842b5")]
        public virtual ShippingCarrierRegressionRepositoryFolders.ShipOrders1AppFolder ShipOrders1
        {
            get { return _shiporders1; }
        }

        /// <summary>
        /// The ShippingDlg folder.
        /// </summary>
        [RepositoryFolder("6f962f36-f89d-46b2-900c-1ad2de88d682")]
        public virtual ShippingCarrierRegressionRepositoryFolders.ShippingDlgAppFolder ShippingDlg
        {
            get { return _shippingdlg; }
        }
    }

    /// <summary>
    /// Inner folder classes.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Ranorex", "5.4.5")]
    public partial class ShippingCarrierRegressionRepositoryFolders
    {
        /// <summary>
        /// The ShipOrders1AppFolder folder.
        /// </summary>
        [RepositoryFolder("b316b72b-5681-4152-85b9-e567c20842b5")]
        public partial class ShipOrders1AppFolder : RepoGenBaseFolder
        {
            ShippingCarrierRegressionRepositoryFolders.SplitContainerFolder _splitcontainer;

            /// <summary>
            /// Creates a new ShipOrders1  folder.
            /// </summary>
            public ShipOrders1AppFolder(RepoGenBaseFolder parentFolder) :
                    base("ShipOrders1", "/form[@title='Ship Orders']", parentFolder, 30000, null, true, "b316b72b-5681-4152-85b9-e567c20842b5", "")
            {
                _splitcontainer = new ShippingCarrierRegressionRepositoryFolders.SplitContainerFolder(this);
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("b316b72b-5681-4152-85b9-e567c20842b5")]
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
            [RepositoryItemInfo("b316b72b-5681-4152-85b9-e567c20842b5")]
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
            [RepositoryFolder("704dbf8f-fbfb-447f-abc2-7ca908933d26")]
            public virtual ShippingCarrierRegressionRepositoryFolders.SplitContainerFolder SplitContainer
            {
                get { return _splitcontainer; }
            }
        }

        /// <summary>
        /// The SplitContainerFolder folder.
        /// </summary>
        [RepositoryFolder("704dbf8f-fbfb-447f-abc2-7ca908933d26")]
        public partial class SplitContainerFolder : RepoGenBaseFolder
        {
            RepoItemInfo _residentialdeterminationInfo;
            RepoItemInfo _confirmationInfo;
            RepoItemInfo _weightuspsInfo;
            RepoItemInfo _lengthuspsInfo;
            RepoItemInfo _widthuspsInfo;
            RepoItemInfo _heightuspsInfo;

            /// <summary>
            /// Creates a new SplitContainer  folder.
            /// </summary>
            public SplitContainerFolder(RepoGenBaseFolder parentFolder) :
                    base("SplitContainer", "container[@controlname='splitContainer']", parentFolder, 30000, null, false, "704dbf8f-fbfb-447f-abc2-7ca908933d26", "")
            {
                _residentialdeterminationInfo = new RepoItemInfo(this, "ResidentialDetermination", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/container[@controlname='FedExServiceControl']/container[@controlname='sectionRecipient']//combobox[@controlname='residentialDetermination']", 30000, null, "f9871dd3-361b-4526-b0db-cf5364a35849");
                _confirmationInfo = new RepoItemInfo(this, "Confirmation", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/container[@controlname='sectionShipment']//combobox[@controlname='confirmation']", 30000, null, "4c667a23-c151-4f98-aa8f-89c44c466708");
                _weightuspsInfo = new RepoItemInfo(this, "WeightUSPS", "container[@controlname='panel2']//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/?/container[@controlname='sectionShipment']//container[@controlname='weight']/?/?/text[@accessiblerole='Text']", 30000, null, "4649ef25-c0f8-4799-ae8c-5e38f939d50d");
                _lengthuspsInfo = new RepoItemInfo(this, "LengthUSPS", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/container[@controlname='sectionShipment']//container[@controlname='dimensionsControl']/text[@controlname='length']", 30000, null, "248478d8-9a2c-45c3-89e4-8775f7286bbe");
                _widthuspsInfo = new RepoItemInfo(this, "WidthUSPS", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/container[@controlname='sectionShipment']//container[@controlname='dimensionsControl']/text[@controlname='width']", 30000, null, "a132451d-0871-44ff-a2b0-bc310d7c4f0b");
                _heightuspsInfo = new RepoItemInfo(this, "HeightUSPS", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/container[@controlname='sectionShipment']//container[@controlname='dimensionsControl']/text[@controlname='height']", 30000, null, "a8b6dabb-c070-4cc9-97af-b4ed21521b06");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("704dbf8f-fbfb-447f-abc2-7ca908933d26")]
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
            [RepositoryItemInfo("704dbf8f-fbfb-447f-abc2-7ca908933d26")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The ResidentialDetermination item.
            /// </summary>
            [RepositoryItem("f9871dd3-361b-4526-b0db-cf5364a35849")]
            public virtual Ranorex.ComboBox ResidentialDetermination
            {
                get
                {
                    return _residentialdeterminationInfo.CreateAdapter<Ranorex.ComboBox>(true);
                }
            }

            /// <summary>
            /// The ResidentialDetermination item info.
            /// </summary>
            [RepositoryItemInfo("f9871dd3-361b-4526-b0db-cf5364a35849")]
            public virtual RepoItemInfo ResidentialDeterminationInfo
            {
                get
                {
                    return _residentialdeterminationInfo;
                }
            }

            /// <summary>
            /// The Confirmation item.
            /// </summary>
            [RepositoryItem("4c667a23-c151-4f98-aa8f-89c44c466708")]
            public virtual Ranorex.ComboBox Confirmation
            {
                get
                {
                    return _confirmationInfo.CreateAdapter<Ranorex.ComboBox>(true);
                }
            }

            /// <summary>
            /// The Confirmation item info.
            /// </summary>
            [RepositoryItemInfo("4c667a23-c151-4f98-aa8f-89c44c466708")]
            public virtual RepoItemInfo ConfirmationInfo
            {
                get
                {
                    return _confirmationInfo;
                }
            }

            /// <summary>
            /// The WeightUSPS item.
            /// </summary>
            [RepositoryItem("4649ef25-c0f8-4799-ae8c-5e38f939d50d")]
            public virtual Ranorex.Text WeightUSPS
            {
                get
                {
                    return _weightuspsInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The WeightUSPS item info.
            /// </summary>
            [RepositoryItemInfo("4649ef25-c0f8-4799-ae8c-5e38f939d50d")]
            public virtual RepoItemInfo WeightUSPSInfo
            {
                get
                {
                    return _weightuspsInfo;
                }
            }

            /// <summary>
            /// The LengthUSPS item.
            /// </summary>
            [RepositoryItem("248478d8-9a2c-45c3-89e4-8775f7286bbe")]
            public virtual Ranorex.Text LengthUSPS
            {
                get
                {
                    return _lengthuspsInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The LengthUSPS item info.
            /// </summary>
            [RepositoryItemInfo("248478d8-9a2c-45c3-89e4-8775f7286bbe")]
            public virtual RepoItemInfo LengthUSPSInfo
            {
                get
                {
                    return _lengthuspsInfo;
                }
            }

            /// <summary>
            /// The WidthUSPS item.
            /// </summary>
            [RepositoryItem("a132451d-0871-44ff-a2b0-bc310d7c4f0b")]
            public virtual Ranorex.Text WidthUSPS
            {
                get
                {
                    return _widthuspsInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The WidthUSPS item info.
            /// </summary>
            [RepositoryItemInfo("a132451d-0871-44ff-a2b0-bc310d7c4f0b")]
            public virtual RepoItemInfo WidthUSPSInfo
            {
                get
                {
                    return _widthuspsInfo;
                }
            }

            /// <summary>
            /// The HeightUSPS item.
            /// </summary>
            [RepositoryItem("a8b6dabb-c070-4cc9-97af-b4ed21521b06")]
            public virtual Ranorex.Text HeightUSPS
            {
                get
                {
                    return _heightuspsInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The HeightUSPS item info.
            /// </summary>
            [RepositoryItemInfo("a8b6dabb-c070-4cc9-97af-b4ed21521b06")]
            public virtual RepoItemInfo HeightUSPSInfo
            {
                get
                {
                    return _heightuspsInfo;
                }
            }
        }

        /// <summary>
        /// The ShippingDlgAppFolder folder.
        /// </summary>
        [RepositoryFolder("6f962f36-f89d-46b2-900c-1ad2de88d682")]
        public partial class ShippingDlgAppFolder : RepoGenBaseFolder
        {
            ShippingCarrierRegressionRepositoryFolders.DimensionsControlFolder _dimensionscontrol;
            ShippingCarrierRegressionRepositoryFolders.SplitContainerFolder1 _splitcontainer;

            /// <summary>
            /// Creates a new ShippingDlg  folder.
            /// </summary>
            public ShippingDlgAppFolder(RepoGenBaseFolder parentFolder) :
                    base("ShippingDlg", "/form[@controlname='ShippingDlg']", parentFolder, 30000, null, true, "6f962f36-f89d-46b2-900c-1ad2de88d682", "")
            {
                _dimensionscontrol = new ShippingCarrierRegressionRepositoryFolders.DimensionsControlFolder(this);
                _splitcontainer = new ShippingCarrierRegressionRepositoryFolders.SplitContainerFolder1(this);
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("6f962f36-f89d-46b2-900c-1ad2de88d682")]
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
            [RepositoryItemInfo("6f962f36-f89d-46b2-900c-1ad2de88d682")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The DimensionsControl folder.
            /// </summary>
            [RepositoryFolder("91c3dada-1243-4b10-ad20-450acaafb698")]
            public virtual ShippingCarrierRegressionRepositoryFolders.DimensionsControlFolder DimensionsControl
            {
                get { return _dimensionscontrol; }
            }

            /// <summary>
            /// The SplitContainer folder.
            /// </summary>
            [RepositoryFolder("1c0286e4-c6be-4bb9-8420-62ab72c3ae40")]
            public virtual ShippingCarrierRegressionRepositoryFolders.SplitContainerFolder1 SplitContainer
            {
                get { return _splitcontainer; }
            }
        }

        /// <summary>
        /// The DimensionsControlFolder folder.
        /// </summary>
        [RepositoryFolder("91c3dada-1243-4b10-ad20-450acaafb698")]
        public partial class DimensionsControlFolder : RepoGenBaseFolder
        {
            RepoItemInfo _lengthiparcelInfo;
            RepoItemInfo _widthiparcelInfo;
            RepoItemInfo _heightiparcelInfo;

            /// <summary>
            /// Creates a new DimensionsControl  folder.
            /// </summary>
            public DimensionsControlFolder(RepoGenBaseFolder parentFolder) :
                    base("DimensionsControl", "container[@controlname='splitContainer']//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/?/container[@controlname='sectionShipment']//container[@controlname='packageControl']/container[@controlname='panelPackage']/container[@controlname='dimensionsControl']", parentFolder, 30000, null, false, "91c3dada-1243-4b10-ad20-450acaafb698", "")
            {
                _lengthiparcelInfo = new RepoItemInfo(this, "Lengthiparcel", "text[@controlname='length']", 30000, null, "d39ceb27-21ff-4ad4-986e-f994f3d4a9a6");
                _widthiparcelInfo = new RepoItemInfo(this, "Widthiparcel", "text[@controlname='width']", 30000, null, "54d5dbf7-f198-4022-8b53-0d467d775ca9");
                _heightiparcelInfo = new RepoItemInfo(this, "Heightiparcel", "text[@controlname='height']", 30000, null, "d88c1b4b-9e30-43b7-b164-80f792f82d0f");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("91c3dada-1243-4b10-ad20-450acaafb698")]
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
            [RepositoryItemInfo("91c3dada-1243-4b10-ad20-450acaafb698")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The Lengthiparcel item.
            /// </summary>
            [RepositoryItem("d39ceb27-21ff-4ad4-986e-f994f3d4a9a6")]
            public virtual Ranorex.Text Lengthiparcel
            {
                get
                {
                    return _lengthiparcelInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The Lengthiparcel item info.
            /// </summary>
            [RepositoryItemInfo("d39ceb27-21ff-4ad4-986e-f994f3d4a9a6")]
            public virtual RepoItemInfo LengthiparcelInfo
            {
                get
                {
                    return _lengthiparcelInfo;
                }
            }

            /// <summary>
            /// The Widthiparcel item.
            /// </summary>
            [RepositoryItem("54d5dbf7-f198-4022-8b53-0d467d775ca9")]
            public virtual Ranorex.Text Widthiparcel
            {
                get
                {
                    return _widthiparcelInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The Widthiparcel item info.
            /// </summary>
            [RepositoryItemInfo("54d5dbf7-f198-4022-8b53-0d467d775ca9")]
            public virtual RepoItemInfo WidthiparcelInfo
            {
                get
                {
                    return _widthiparcelInfo;
                }
            }

            /// <summary>
            /// The Heightiparcel item.
            /// </summary>
            [RepositoryItem("d88c1b4b-9e30-43b7-b164-80f792f82d0f")]
            public virtual Ranorex.Text Heightiparcel
            {
                get
                {
                    return _heightiparcelInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The Heightiparcel item info.
            /// </summary>
            [RepositoryItemInfo("d88c1b4b-9e30-43b7-b164-80f792f82d0f")]
            public virtual RepoItemInfo HeightiparcelInfo
            {
                get
                {
                    return _heightiparcelInfo;
                }
            }
        }

        /// <summary>
        /// The SplitContainerFolder1 folder.
        /// </summary>
        [RepositoryFolder("1c0286e4-c6be-4bb9-8420-62ab72c3ae40")]
        public partial class SplitContainerFolder1 : RepoGenBaseFolder
        {
            RepoItemInfo _serviceInfo;
            RepoItemInfo _weightInfo;
            RepoItemInfo _customsInfo;
            RepoItemInfo _buttonaddInfo;
            RepoItemInfo _customsweightuspsInfo;
            RepoItemInfo _customsvalueuspsInfo;
            RepoItemInfo _customsselecteditemvalueuspsendiciaInfo;

            /// <summary>
            /// Creates a new SplitContainer  folder.
            /// </summary>
            public SplitContainerFolder1(RepoGenBaseFolder parentFolder) :
                    base("SplitContainer", "container[@controlname='splitContainer']", parentFolder, 30000, null, false, "1c0286e4-c6be-4bb9-8420-62ab72c3ae40", "")
            {
                _serviceInfo = new RepoItemInfo(this, "Service", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/container/container[@controlname='sectionShipment']//combobox[@controlname='service']", 30000, null, "3f814f41-2004-4277-90f6-31b572d0f36e");
                _weightInfo = new RepoItemInfo(this, "Weight", ".//tabpage[@controlname='tabPageService']/container[@controlname='serviceControlArea']/?/?/container[@controlname='sectionShipment']//container[@controlname='packageControl']/container[@controlname='panelPackage']/container[@controlname='weight']/text[@controlname='textBox']", 30000, null, "78c1cb69-4a6e-468a-9bd7-b0201e7d37ab");
                _customsInfo = new RepoItemInfo(this, "Customs", "?/?/container[@controlname='ratesSplitContainer']/container[@controlname='panel1']/tabpagelist/rawtext[1]", 30000, null, "5809f2b0-f3c2-4819-9779-7197fb71a1e9");
                _buttonaddInfo = new RepoItemInfo(this, "ButtonAdd", "container[@controlname='panel2']/?/?/container[@controlname='panel1']/?/?/tabpage[@controlname='tabPageCustoms']//container[@controlname='sectionContents']//button[@controlname='add']", 30000, null, "5c370e95-f650-459f-8235-2df901e0267d");
                _customsweightuspsInfo = new RepoItemInfo(this, "CustomsWeightUSPS", "?/?/container[@controlname='ratesSplitContainer']/container[@controlname='panel1']/?/?/tabpage[@controlname='tabPageCustoms']//container[@controlname='sectionContents']//container[@controlname='groupSelectedContent']/container[@controlname='weight']/text[@controlname='textBox']/text[@accessiblerole='Text']", 30000, null, "0371ac3c-347f-4ce2-b81f-26e846a305b6");
                _customsvalueuspsInfo = new RepoItemInfo(this, "CustomsValueUSPS", "container[@controlname='panel2']/?/?/container[@controlname='panel1']/tabpagelist[@controlname='tabControl']/tabpage[@controlname='tabPageCustoms']//container[@controlname='sectionGeneral']//text[@accessiblename='Value:']", 30000, null, "0aa31e4b-29ea-4033-8b4f-79fa46b86ed2");
                _customsselecteditemvalueuspsendiciaInfo = new RepoItemInfo(this, "CustomsSelectedItemValueUSPSEndicia", "container[@controlname='panel2']/?/?/container[@controlname='panel1']/?/?/tabpage[@controlname='tabPageCustoms']//container[@controlname='sectionContents']//container[@controlname='groupSelectedContent']/text[@controlname='value']", 30000, null, "5e23138d-2057-452b-814f-f088b6253960");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("1c0286e4-c6be-4bb9-8420-62ab72c3ae40")]
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
            [RepositoryItemInfo("1c0286e4-c6be-4bb9-8420-62ab72c3ae40")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The Service item.
            /// </summary>
            [RepositoryItem("3f814f41-2004-4277-90f6-31b572d0f36e")]
            public virtual Ranorex.ComboBox Service
            {
                get
                {
                    return _serviceInfo.CreateAdapter<Ranorex.ComboBox>(true);
                }
            }

            /// <summary>
            /// The Service item info.
            /// </summary>
            [RepositoryItemInfo("3f814f41-2004-4277-90f6-31b572d0f36e")]
            public virtual RepoItemInfo ServiceInfo
            {
                get
                {
                    return _serviceInfo;
                }
            }

            /// <summary>
            /// The Weight item.
            /// </summary>
            [RepositoryItem("78c1cb69-4a6e-468a-9bd7-b0201e7d37ab")]
            public virtual Ranorex.Text Weight
            {
                get
                {
                    return _weightInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The Weight item info.
            /// </summary>
            [RepositoryItemInfo("78c1cb69-4a6e-468a-9bd7-b0201e7d37ab")]
            public virtual RepoItemInfo WeightInfo
            {
                get
                {
                    return _weightInfo;
                }
            }

            /// <summary>
            /// The Customs item.
            /// </summary>
            [RepositoryItem("5809f2b0-f3c2-4819-9779-7197fb71a1e9")]
            public virtual Ranorex.RawText Customs
            {
                get
                {
                    return _customsInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The Customs item info.
            /// </summary>
            [RepositoryItemInfo("5809f2b0-f3c2-4819-9779-7197fb71a1e9")]
            public virtual RepoItemInfo CustomsInfo
            {
                get
                {
                    return _customsInfo;
                }
            }

            /// <summary>
            /// The ButtonAdd item.
            /// </summary>
            [RepositoryItem("5c370e95-f650-459f-8235-2df901e0267d")]
            public virtual Ranorex.Button ButtonAdd
            {
                get
                {
                    return _buttonaddInfo.CreateAdapter<Ranorex.Button>(true);
                }
            }

            /// <summary>
            /// The ButtonAdd item info.
            /// </summary>
            [RepositoryItemInfo("5c370e95-f650-459f-8235-2df901e0267d")]
            public virtual RepoItemInfo ButtonAddInfo
            {
                get
                {
                    return _buttonaddInfo;
                }
            }

            /// <summary>
            /// The CustomsWeightUSPS item.
            /// </summary>
            [RepositoryItem("0371ac3c-347f-4ce2-b81f-26e846a305b6")]
            public virtual Ranorex.Text CustomsWeightUSPS
            {
                get
                {
                    return _customsweightuspsInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The CustomsWeightUSPS item info.
            /// </summary>
            [RepositoryItemInfo("0371ac3c-347f-4ce2-b81f-26e846a305b6")]
            public virtual RepoItemInfo CustomsWeightUSPSInfo
            {
                get
                {
                    return _customsweightuspsInfo;
                }
            }

            /// <summary>
            /// The CustomsValueUSPS item.
            /// </summary>
            [RepositoryItem("0aa31e4b-29ea-4033-8b4f-79fa46b86ed2")]
            public virtual Ranorex.Text CustomsValueUSPS
            {
                get
                {
                    return _customsvalueuspsInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The CustomsValueUSPS item info.
            /// </summary>
            [RepositoryItemInfo("0aa31e4b-29ea-4033-8b4f-79fa46b86ed2")]
            public virtual RepoItemInfo CustomsValueUSPSInfo
            {
                get
                {
                    return _customsvalueuspsInfo;
                }
            }

            /// <summary>
            /// The CustomsSelectedItemValueUSPSEndicia item.
            /// </summary>
            [RepositoryItem("5e23138d-2057-452b-814f-f088b6253960")]
            public virtual Ranorex.Text CustomsSelectedItemValueUSPSEndicia
            {
                get
                {
                    return _customsselecteditemvalueuspsendiciaInfo.CreateAdapter<Ranorex.Text>(true);
                }
            }

            /// <summary>
            /// The CustomsSelectedItemValueUSPSEndicia item info.
            /// </summary>
            [RepositoryItemInfo("5e23138d-2057-452b-814f-f088b6253960")]
            public virtual RepoItemInfo CustomsSelectedItemValueUSPSEndiciaInfo
            {
                get
                {
                    return _customsselecteditemvalueuspsendiciaInfo;
                }
            }
        }

    }
#pragma warning restore 0436
}