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

namespace OrderCreation
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    /// The class representing the OrderCreationRepository element repository.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
    [RepositoryFolder("ea7232e0-41f1-428f-b543-5dbda2f4d92e")]
    public partial class OrderCreationRepository : RepoGenBaseFolder
    {
        static OrderCreationRepository instance = new OrderCreationRepository();

        /// <summary>
        /// Gets the singleton class instance representing the OrderCreationRepository element repository.
        /// </summary>
        [RepositoryFolder("ea7232e0-41f1-428f-b543-5dbda2f4d92e")]
        public static OrderCreationRepository Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Repository class constructor.
        /// </summary>
        public OrderCreationRepository() 
            : base("OrderCreationRepository", "/", null, 0, false, "ea7232e0-41f1-428f-b543-5dbda2f4d92e", ".\\RepositoryImages\\OrderCreationRepositoryea7232e0.rximgres")
        {
        }

#region Variables

#endregion

        /// <summary>
        /// The Self item info.
        /// </summary>
        [RepositoryItemInfo("ea7232e0-41f1-428f-b543-5dbda2f4d92e")]
        public virtual RepoItemInfo SelfInfo
        {
            get
            {
                return _selfInfo;
            }
        }
    }

    /// <summary>
    /// Inner folder classes.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
    public partial class OrderCreationRepositoryFolders
    {
    }
#pragma warning restore 0436
}