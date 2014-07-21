﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SureDone
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class SureDoneStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SureDoneStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.SureDone;
            }
        }

        /// <summary>
        /// Log request/responses as SureDone
        /// </summary>
        public override ApiLogSource LogSource   
        {
            get
            {
                return ApiLogSource.SureDone;
            }
        }
    }
}
