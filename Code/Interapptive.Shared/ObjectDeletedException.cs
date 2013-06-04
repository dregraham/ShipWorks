﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Utility
{
    [Serializable]
    public class ObjectDeletedException : Exception
    {
        public ObjectDeletedException()
        {

        }

        public ObjectDeletedException(string message)
            : base(message)
        {

        }

        public ObjectDeletedException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
