﻿using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Interface for initializing order lookup pipelines under a top level lifetime scope
    /// </summary>
    [Service]
    public interface IScanToShipPipeline : IDisposable
    {
        /// <summary>
        /// Initialize the pipeline under the current scope
        /// </summary>
        void InitializeForCurrentScope();
    }
}
