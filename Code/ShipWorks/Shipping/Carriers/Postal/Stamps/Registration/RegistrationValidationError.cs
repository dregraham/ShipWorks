﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    public class RegistrationValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationValidationError"/> class.
        /// </summary>
        public RegistrationValidationError(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }
    }
}
