using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Controls when a command will be enabled
    /// </summary>
    public enum SelectionDependentType
    {
        Ignore,

        OneOrder,
        OneOrMoreOrders,
        TwoOrMoreOrders,

        OneCustomer,
        OneOrMoreCustomers,

        OneRow,
        OneOrMoreRows,

        AppliesFunction
    }
}
