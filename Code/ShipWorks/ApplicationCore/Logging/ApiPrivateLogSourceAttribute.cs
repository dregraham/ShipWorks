using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Attribute applied to ApiLogSource enum values to indicate they are not logged for general users.
    /// </summary>
    sealed class ApiPrivateLogSourceAttribute : Attribute
    {
    }
}
