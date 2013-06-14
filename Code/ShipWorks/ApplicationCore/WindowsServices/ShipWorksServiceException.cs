﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    [Serializable]
    public class ShipWorksServiceException : Exception
    {
        public ShipWorksServiceException(string message) : base(message)
        {
            
        }
    }
}
