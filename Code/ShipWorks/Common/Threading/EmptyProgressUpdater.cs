﻿using Interapptive.Shared.Threading;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Update progress that does nothing
    /// </summary>
    public class EmptyProgressUpdater : IProgressUpdater
    {
        /// <summary>
        /// Update the progress
        /// </summary>
        public void Update()
        {

        }

        /// <summary>
        /// Update the progress
        /// </summary>
        public void Update(int finishedCount)
        {

        }
    }
}