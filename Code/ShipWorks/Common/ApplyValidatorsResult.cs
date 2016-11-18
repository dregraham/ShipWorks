﻿using System.Collections.Generic;
using Interapptive.Shared.Collections;

namespace ShipWorks.Common
{
    /// <summary>
    /// Results of applying validators to an object
    /// </summary>
    public class ApplyValidatorsResult : IApplyValidatorsResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplyValidatorsResult(bool success, IEnumerable<string> errors)
        {
            Success = success;
            Errors = errors.ToReadOnly();
        }

        /// <summary>
        /// Was validation successful
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Did validation fail
        /// </summary>
        public bool Failure => !Success;

        /// <summary>
        /// Collection of errors if validation failed
        /// </summary>
        public IEnumerable<string> Errors { get; }
    }
}
