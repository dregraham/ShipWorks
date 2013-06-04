using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// A single error as returned by Network Solutions
    /// </summary>
    [Serializable]
    public class NetworkSolutionsError
    {
        public int Number { get; set; }
        public string Message { get; set; }
        public string FieldInError { get; set; } 
    }
}
