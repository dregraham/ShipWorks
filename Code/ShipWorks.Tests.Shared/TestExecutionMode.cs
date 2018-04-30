﻿using System;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Empty execution mode used for testing
    /// </summary>
    public class TestExecutionMode : ExecutionMode
    {
        /// <summary>
        /// Is the UI displayed
        /// </summary>
        public override bool IsUIDisplayed => false;

        /// <summary>
        /// Is the UI supported
        /// </summary>
        public override bool IsUISupported => false;

        /// <summary>
        /// Name of the execution mode
        /// </summary>
        public override string Name => "Test Execution Mode";

        /// <summary>
        /// Execute
        /// </summary>
        public override Task Execute()
        {
            // Test doesn't need to do anything

            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle any exceptions
        /// </summary>
        public override Task HandleException(Exception exception, bool guiThread, string userEmail)
        {
            return Task.FromResult(true);
        }
    }
}
