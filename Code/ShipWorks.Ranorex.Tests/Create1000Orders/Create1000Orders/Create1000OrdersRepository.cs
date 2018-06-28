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

namespace Create1000Orders
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    /// The class representing the Create1000OrdersRepository element repository.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Ranorex", "6.0")]
    [RepositoryFolder("f2235957-cf06-4396-a170-9b0efb4a1be2")]
    public partial class Create1000OrdersRepository : RepoGenBaseFolder
    {
        static Create1000OrdersRepository instance = new Create1000OrdersRepository();
        Create1000OrdersRepositoryFolders.MainFormAppFolder _mainform;
        Create1000OrdersRepositoryFolders.AddOrderWizardAppFolder _addorderwizard;
        Create1000OrdersRepositoryFolders.EntityPickerDlgAppFolder _entitypickerdlg;

        /// <summary>
        /// Gets the singleton class instance representing the Create1000OrdersRepository element repository.
        /// </summary>
        [RepositoryFolder("f2235957-cf06-4396-a170-9b0efb4a1be2")]
        public static Create1000OrdersRepository Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Repository class constructor.
        /// </summary>
        public Create1000OrdersRepository() 
            : base("Create1000OrdersRepository", "/", null, 0, false, "f2235957-cf06-4396-a170-9b0efb4a1be2", ".\\RepositoryImages\\Create1000OrdersRepositoryf2235957.rximgres")
        {
            _mainform = new Create1000OrdersRepositoryFolders.MainFormAppFolder(this);
            _addorderwizard = new Create1000OrdersRepositoryFolders.AddOrderWizardAppFolder(this);
            _entitypickerdlg = new Create1000OrdersRepositoryFolders.EntityPickerDlgAppFolder(this);
        }

#region Variables

#endregion

        /// <summary>
        /// The Self item info.
        /// </summary>
        [RepositoryItemInfo("f2235957-cf06-4396-a170-9b0efb4a1be2")]
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
        [RepositoryFolder("9793b585-13b2-4ec6-b9ac-af02291a8523")]
        public virtual Create1000OrdersRepositoryFolders.MainFormAppFolder MainForm
        {
            get { return _mainform; }
        }

        /// <summary>
        /// The AddOrderWizard folder.
        /// </summary>
        [RepositoryFolder("b1b624cb-11c2-4b5d-abfd-d84e09b98322")]
        public virtual Create1000OrdersRepositoryFolders.AddOrderWizardAppFolder AddOrderWizard
        {
            get { return _addorderwizard; }
        }

        /// <summary>
        /// The EntityPickerDlg folder.
        /// </summary>
        [RepositoryFolder("bdc8ab56-8112-4b46-b0cd-0178c633efc8")]
        public virtual Create1000OrdersRepositoryFolders.EntityPickerDlgAppFolder EntityPickerDlg
        {
            get { return _entitypickerdlg; }
        }
    }

    /// <summary>
    /// Inner folder classes.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Ranorex", "6.0")]
    public partial class Create1000OrdersRepositoryFolders
    {
        /// <summary>
        /// The MainFormAppFolder folder.
        /// </summary>
        [RepositoryFolder("9793b585-13b2-4ec6-b9ac-af02291a8523")]
        public partial class MainFormAppFolder : RepoGenBaseFolder
        {
            RepoItemInfo _rawtextnewInfo;

            /// <summary>
            /// Creates a new MainForm  folder.
            /// </summary>
            public MainFormAppFolder(RepoGenBaseFolder parentFolder) :
                    base("MainForm", "/form[@title~'^ShipWorks\\ -\\ bbadvanced4@f']", parentFolder, 30000, null, true, "9793b585-13b2-4ec6-b9ac-af02291a8523", "")
            {
                _rawtextnewInfo = new RepoItemInfo(this, "RawTextNew", "?/?/element[@controlname='ribbonTabHome']/rawtext[@rawtext='New']", 30000, null, "b9c23b07-ae3d-438c-bc37-6ffeab750f74");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("9793b585-13b2-4ec6-b9ac-af02291a8523")]
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
            [RepositoryItemInfo("9793b585-13b2-4ec6-b9ac-af02291a8523")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The RawTextNew item.
            /// </summary>
            [RepositoryItem("b9c23b07-ae3d-438c-bc37-6ffeab750f74")]
            public virtual Ranorex.RawText RawTextNew
            {
                get
                {
                    return _rawtextnewInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The RawTextNew item info.
            /// </summary>
            [RepositoryItemInfo("b9c23b07-ae3d-438c-bc37-6ffeab750f74")]
            public virtual RepoItemInfo RawTextNewInfo
            {
                get
                {
                    return _rawtextnewInfo;
                }
            }
        }

        /// <summary>
        /// The AddOrderWizardAppFolder folder.
        /// </summary>
        [RepositoryFolder("b1b624cb-11c2-4b5d-abfd-d84e09b98322")]
        public partial class AddOrderWizardAppFolder : RepoGenBaseFolder
        {
            RepoItemInfo _radioassigncustomerInfo;
            RepoItemInfo _selectInfo;
            RepoItemInfo _nextInfo;
            RepoItemInfo _finishInfo;

            /// <summary>
            /// Creates a new AddOrderWizard  folder.
            /// </summary>
            public AddOrderWizardAppFolder(RepoGenBaseFolder parentFolder) :
                    base("AddOrderWizard", "/form[@controlname='AddOrderWizard']", parentFolder, 30000, null, true, "b1b624cb-11c2-4b5d-abfd-d84e09b98322", "")
            {
                _radioassigncustomerInfo = new RepoItemInfo(this, "RadioAssignCustomer", "container[@controlname='mainPanel']//radiobutton[@controlname='radioAssignCustomer'][@enabled='true']", 30000, null, "8e7b7f6a-3074-4273-9fa5-11d388befc90");
                _selectInfo = new RepoItemInfo(this, "Select", "container[@controlname='mainPanel']//text[@controlname='linkSelectCustomer']/rawtext[@rawtext='Select...'][@enabled='true']", 30000, null, "bd8b606e-89ce-4b34-92b2-11a0b504ae05");
                _nextInfo = new RepoItemInfo(this, "Next", "?/?/rawtext[@rawtext='Next >' and @row='0'][@enabled='true']", 30000, null, "69d029f8-d2fb-4d56-b819-416fb3c18791");
                _finishInfo = new RepoItemInfo(this, "Finish", "?/?/rawtext[@rawtext='Finish' and @row='0'][@enabled='true']", 30000, null, "ddc46a98-65d5-4e33-8fd7-824cbf9cff11");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("b1b624cb-11c2-4b5d-abfd-d84e09b98322")]
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
            [RepositoryItemInfo("b1b624cb-11c2-4b5d-abfd-d84e09b98322")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The RadioAssignCustomer item.
            /// </summary>
            [RepositoryItem("8e7b7f6a-3074-4273-9fa5-11d388befc90")]
            public virtual Ranorex.RadioButton RadioAssignCustomer
            {
                get
                {
                    return _radioassigncustomerInfo.CreateAdapter<Ranorex.RadioButton>(true);
                }
            }

            /// <summary>
            /// The RadioAssignCustomer item info.
            /// </summary>
            [RepositoryItemInfo("8e7b7f6a-3074-4273-9fa5-11d388befc90")]
            public virtual RepoItemInfo RadioAssignCustomerInfo
            {
                get
                {
                    return _radioassigncustomerInfo;
                }
            }

            /// <summary>
            /// The Select item.
            /// </summary>
            [RepositoryItem("bd8b606e-89ce-4b34-92b2-11a0b504ae05")]
            public virtual Ranorex.RawText Select
            {
                get
                {
                    return _selectInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The Select item info.
            /// </summary>
            [RepositoryItemInfo("bd8b606e-89ce-4b34-92b2-11a0b504ae05")]
            public virtual RepoItemInfo SelectInfo
            {
                get
                {
                    return _selectInfo;
                }
            }

            /// <summary>
            /// The Next item.
            /// </summary>
            [RepositoryItem("69d029f8-d2fb-4d56-b819-416fb3c18791")]
            public virtual Ranorex.RawText Next
            {
                get
                {
                    return _nextInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The Next item info.
            /// </summary>
            [RepositoryItemInfo("69d029f8-d2fb-4d56-b819-416fb3c18791")]
            public virtual RepoItemInfo NextInfo
            {
                get
                {
                    return _nextInfo;
                }
            }

            /// <summary>
            /// The Finish item.
            /// </summary>
            [RepositoryItem("ddc46a98-65d5-4e33-8fd7-824cbf9cff11")]
            public virtual Ranorex.RawText Finish
            {
                get
                {
                    return _finishInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The Finish item info.
            /// </summary>
            [RepositoryItemInfo("ddc46a98-65d5-4e33-8fd7-824cbf9cff11")]
            public virtual RepoItemInfo FinishInfo
            {
                get
                {
                    return _finishInfo;
                }
            }
        }

        /// <summary>
        /// The EntityPickerDlgAppFolder folder.
        /// </summary>
        [RepositoryFolder("bdc8ab56-8112-4b46-b0cd-0178c633efc8")]
        public partial class EntityPickerDlgAppFolder : RepoGenBaseFolder
        {
            RepoItemInfo _emanInfo;
            RepoItemInfo _buttonokInfo;

            /// <summary>
            /// Creates a new EntityPickerDlg  folder.
            /// </summary>
            public EntityPickerDlgAppFolder(RepoGenBaseFolder parentFolder) :
                    base("EntityPickerDlg", "/form[@controlname='EntityPickerDlg']", parentFolder, 30000, null, true, "bdc8ab56-8112-4b46-b0cd-0178c633efc8", "")
            {
                _emanInfo = new RepoItemInfo(this, "Eman", "container[@controlname='splitContainer']//container[@controlname='gridPanel']/?/?/rawtext[@rawtext='Eman'][@enabled='true']", 30000, null, "aafe9cff-fc35-485a-98a4-e2d3614b1897");
                _buttonokInfo = new RepoItemInfo(this, "ButtonOk", "button[@controlname='ok'][@enabled='true']", 30000, null, "1f207d7e-46b7-4a3c-85db-1e9095a5e4ab");
            }

            /// <summary>
            /// The Self item.
            /// </summary>
            [RepositoryItem("bdc8ab56-8112-4b46-b0cd-0178c633efc8")]
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
            [RepositoryItemInfo("bdc8ab56-8112-4b46-b0cd-0178c633efc8")]
            public virtual RepoItemInfo SelfInfo
            {
                get
                {
                    return _selfInfo;
                }
            }

            /// <summary>
            /// The Eman item.
            /// </summary>
            [RepositoryItem("aafe9cff-fc35-485a-98a4-e2d3614b1897")]
            public virtual Ranorex.RawText Eman
            {
                get
                {
                    return _emanInfo.CreateAdapter<Ranorex.RawText>(true);
                }
            }

            /// <summary>
            /// The Eman item info.
            /// </summary>
            [RepositoryItemInfo("aafe9cff-fc35-485a-98a4-e2d3614b1897")]
            public virtual RepoItemInfo EmanInfo
            {
                get
                {
                    return _emanInfo;
                }
            }

            /// <summary>
            /// The ButtonOk item.
            /// </summary>
            [RepositoryItem("1f207d7e-46b7-4a3c-85db-1e9095a5e4ab")]
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
            [RepositoryItemInfo("1f207d7e-46b7-4a3c-85db-1e9095a5e4ab")]
            public virtual RepoItemInfo ButtonOkInfo
            {
                get
                {
                    return _buttonokInfo;
                }
            }
        }

    }
#pragma warning restore 0436
}