﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.ExecutionMode.Initialization
{
    public class CommandLineExecutionModeInitializer : ExecutionModeInitializerBase
    {
        /// <summary>
        /// Intended for setting up/initializing any dependencies for an execution mode/context.
        /// </summary>
        /// <param name="executionMode">The execution mode.</param>
        public override void Initialize()
        {
            PerformCommonInitialization();
        }
    }
}
