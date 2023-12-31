﻿namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Controls when a command will be enabled
    /// </summary>
    public enum SelectionDependentType
    {
        Ignore,

        OneOrder,
        OneOrMoreOrders,

        OneCustomer,
        OneOrMoreCustomers,

        OneRow,
        OneOrMoreRows,

        AppliesFunction
    }
}
