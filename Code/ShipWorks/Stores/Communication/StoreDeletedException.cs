using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Communication
{
    class StoreDeletedException : Exception
    {
        public StoreDeletedException()
        {

        }

        public StoreDeletedException(Exception inner)
            : base("", inner)
        {

        }

        public override string Message
        {
            get
            {
                return "The store has been deleted.";
            }
        }
    }
}
