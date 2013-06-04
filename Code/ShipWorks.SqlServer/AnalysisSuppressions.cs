﻿// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1050:DeclareTypesInNamespaces", Scope = "type", Target = "Triggers")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Scope = "type", Target = "Triggers")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "ShipWorks.SqlServer.Data.Auditing.AuditColumnInfo.#ColumnID")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "ShipWorks.Filters.BuiltinFilter.#.cctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "InitialCounts", Scope = "member", Target = "StoredProcedures.#CalculateInitialFilterCounts()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CalculateInitialFilterCounts", Scope = "member", Target = "StoredProcedures.#CalculateInitialFilterCounts()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "UserDefinedFunctions.#GetComputerID()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "UserDefinedFunctions.#GetTransactionID()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "UserDefinedFunctions.#GetUserID()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1050:DeclareTypesInNamespaces", Scope = "type", Target = "StoredProcedures")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1050:DeclareTypesInNamespaces", Scope = "type", Target = "UserDefinedFunctions")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Scope = "member", Target = "UserDefinedFunctions.#GetTemplateDescendantOfFolder_FillRow(System.Object,System.Data.SqlTypes.SqlInt64&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Scope = "type", Target = "UserDefinedFunctions")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Scope = "type", Target = "StoredProcedures")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "ShipWorks.SqlServer.Data.Rollups.RollupUtility.#PerformRollups(System.Data.SqlClient.SqlConnection,Microsoft.SqlServer.Server.TriggerAction,System.String,System.String,System.String,System.String,System.String,System.Collections.Generic.List`1<ShipWorks.SqlServer.Data.Rollups.RollupColumn>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope = "member", Target = "UserDefinedFunctions.#GetTemplateDescendantOfFolder_FillRow(System.Object,System.Data.SqlTypes.SqlInt64&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "ShipWorks.SqlServer.Data.Rollups.RollupUtility.#UpdateRollups(System.String,System.String,System.String,System.String,System.Collections.Generic.List`1<ShipWorks.SqlServer.Data.Rollups.RollupColumn>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ReferenceKey", Scope = "member", Target = "ShipWorks.SqlServer.Common.Data.ObjectReferenceManager.#SetReference(System.Int64,System.String,System.Int64,System.String,System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "spgetapplock", Scope = "member", Target = "ShipWorks.SqlServer.Common.Data.SqlAppLockUtility.#AcquireLock(System.Data.SqlClient.SqlConnection,System.String,System.TimeSpan)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "spreleaseapplock", Scope = "member", Target = "ShipWorks.SqlServer.Common.Data.SqlAppLockUtility.#ReleaseLock(System.Data.SqlClient.SqlConnection,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "StoredProcedures.#DeleteAbandonedFilterCounts()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountUpdater.#CaptureNodesForUpdate(System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountUpdater.#GetCheckpoint(System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountUpdater.#PerformNextCalculation(ShipWorks.Filters.FilterTarget,System.String,System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountUpdater.#TruncateTable(System.String,System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeContentDirtyUtility.#MergeIntoFilterDirty(System.String,System.String,System.Int32,System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Data.Rollups.RollupUtility.#PerformRollups(System.Data.SqlClient.SqlConnection,Microsoft.SqlServer.Server.TriggerAction,System.String,System.String,System.String,System.String,System.String,System.Collections.Generic.List`1<ShipWorks.SqlServer.Data.Rollups.RollupColumn>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "StoredProcedures.#CalculateInitialFilterCounts()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "StoredProcedures.#GetNextCountToCalculate(System.Data.SqlClient.SqlConnection,System.Int64&,System.Int64&,System.String&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "Triggers.#FilterDirtyCustomer()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "Triggers.#FilterDirtyOrder()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "Triggers.#GetFieldValue(System.String,System.Int64,System.String,System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountUpdater.#GetTimestampValue(System.Byte[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountCheckpoint.#Duration")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountCheckpoint.#DirtyCount")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "ShipWorks.SqlServer.Data.Rollups.RollupColumn.#SourceDependencies")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "ShipWorks.SqlServer.Data.Rollups.RollupColumn.#SourceDependencies")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#.cctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "regeneraged", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#.cctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ColumnMask", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#.cctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#ConvertBitArrayToBitmask(System.Collections.BitArray)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#.cctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeContentDirtyUtility.#MergeIntoFilterDirty(System.String,System.String,System.String,System.Int32,System.Byte[],System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountUpdater.#CaptureNodesForUpdate(System.Byte[],System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unioned", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#CreateUnionedBitmask(System.Collections.Generic.List`1<System.Byte[]>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#CreateUnionedBitmask(System.Collections.Generic.List`1<System.Byte[]>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#CreateUnionedBitmask(System.Collections.Generic.List`1<System.Byte[]>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskUtility.#HasAnyTableBitsSet(System.Byte[],ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskTable)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames", Scope = "type", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeJoinType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "ShipWorks.SqlServer.Filters.FilterCountUpdater.#CaptureNodesForUpdate(System.Int64,System.Data.SqlClient.SqlConnection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Shopify", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskTable.#ShopifyOrder")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Shopify", Scope = "member", Target = "Triggers.#FilterDirtyShopifyOrder()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Etsy", Scope = "member", Target = "Triggers.#FilterDirtyEtsyOrder()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Etsy", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskTable.#EtsyOrder")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "DotCom", Scope = "member", Target = "ShipWorks.SqlServer.Filters.DirtyCounts.FilterNodeColumnMaskTable.#BuyDotComOrderItem")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "DotCom", Scope = "member", Target = "Triggers.#FilterDirtyBuyDotComOrderItem()")]
