﻿using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Platforms.GenericFile.Warehouse
{
    /// <summary>
    /// Generic File warehouse store DTO
    /// </summary>
    [Obfuscation]
    public class GenericFileStore : Store
    {
        public int FileFormat { get; set; }
    }
}
