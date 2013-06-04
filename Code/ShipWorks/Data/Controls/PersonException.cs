using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Controls
{
    public class PersonException:Exception
    {

        /// <summary>
        /// Exception
        /// </summary>
        public PersonException(string message)
            : base(message)
        {
            
        }
    }
}
