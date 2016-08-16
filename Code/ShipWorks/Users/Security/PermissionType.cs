using System;
using System.Reflection;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// All actions that a user can perform in ShipWorks that are auditable and\or securable.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    [Serializable]
    public enum PermissionType
    {
        [PermissionScope(PermissionScope.Global)] AlwaysGrant = 0,
        [PermissionScope(PermissionScope.Global)] InterapptiveOnly = 1,

        [PermissionScope(PermissionScope.Global)] DatabaseSetup = 2,
        [PermissionScope(PermissionScope.Global)] DatabaseBackup = 3,
        [PermissionScope(PermissionScope.Global)] DatabaseRestore = 4,

        [PermissionScope(PermissionScope.Global)] ManageStores = 10,
        [PermissionScope(PermissionScope.Global)] ManageUsers = 11,
        [PermissionScope(PermissionScope.Global)] ManageActions = 12,
        [PermissionScope(PermissionScope.Global)] ManageFilters = 13,
        [PermissionScope(PermissionScope.Global)] ManageTemplates = 14,
        [PermissionScope(PermissionScope.Global)] ManageEmailAccounts = 15,

        [PermissionScope(PermissionScope.Global)] CustomersSendEmail = 20,
        [PermissionScope(PermissionScope.Global)] CustomersCreateEdit = 21,
        [PermissionScope(PermissionScope.Global)] CustomersDelete = 22,
        [PermissionScope(PermissionScope.Global)] CustomersEditNotes = 23,
        [PermissionScope(PermissionScope.Global)] CustomersAddOrder = 24, 

        [PermissionScope(PermissionScope.Store)] OrdersModify = 50,
        [PermissionScope(PermissionScope.Store)] OrdersViewPaymentData = 51,
        [PermissionScope(PermissionScope.Store)] OrdersEditStatus = 52,
        [PermissionScope(PermissionScope.Store)] OrdersEditNotes = 53,
        [PermissionScope(PermissionScope.Store)] OrdersSendEmail = 54,

        [PermissionScope(PermissionScope.IndirectRelatedObject)] RelatedObjectSendEmail = 60,
        [PermissionScope(PermissionScope.IndirectRelatedObject)] RelatedObjectEditNotes = 61,

        [PermissionScope(PermissionScope.IndirectEntityType)] EntityTypeSendEmail = 70,
        [PermissionScope(PermissionScope.IndirectEntityType)] EntityTypeEditNotes = 71,

        [PermissionScope(PermissionScope.Store)] ShipmentsCreateEditProcess = 80,
        [PermissionScope(PermissionScope.Store)] ShipmentsVoidDelete = 81,
        [PermissionScope(PermissionScope.Global)] ShipmentsManageSettings = 82
    }
}
