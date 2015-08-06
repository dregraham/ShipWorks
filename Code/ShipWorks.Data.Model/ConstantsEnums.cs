///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Reflection;

namespace ShipWorks.Data.Model
{

	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Action.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ActionFieldIndex:int
	{
		///<summary>ActionID. </summary>
		ActionID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Name. </summary>
		Name,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>ComputerLimitedType. </summary>
		ComputerLimitedType,
		///<summary>InternalComputerLimitedList. </summary>
		InternalComputerLimitedList,
		///<summary>StoreLimited. </summary>
		StoreLimited,
		///<summary>InternalStoreLimitedList. </summary>
		InternalStoreLimitedList,
		///<summary>TriggerType. </summary>
		TriggerType,
		///<summary>TriggerSettings. </summary>
		TriggerSettings,
		///<summary>TaskSummary. </summary>
		TaskSummary,
		///<summary>InternalOwner. </summary>
		InternalOwner,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ActionFilterTrigger.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ActionFilterTriggerFieldIndex:int
	{
		///<summary>ActionID. </summary>
		ActionID,
		///<summary>FilterNodeID. </summary>
		FilterNodeID,
		///<summary>Direction. </summary>
		Direction,
		///<summary>ComputerLimitedType. </summary>
		ComputerLimitedType,
		///<summary>InternalComputerLimitedList. </summary>
		InternalComputerLimitedList,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ActionQueue.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ActionQueueFieldIndex:int
	{
		///<summary>ActionQueueID. </summary>
		ActionQueueID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ActionID. </summary>
		ActionID,
		///<summary>ActionName. </summary>
		ActionName,
		///<summary>ActionQueueType. </summary>
		ActionQueueType,
		///<summary>ActionVersion. </summary>
		ActionVersion,
		///<summary>QueueVersion. </summary>
		QueueVersion,
		///<summary>TriggerDate. </summary>
		TriggerDate,
		///<summary>TriggerComputerID. </summary>
		TriggerComputerID,
		///<summary>InternalComputerLimitedList. </summary>
		InternalComputerLimitedList,
		///<summary>ObjectID. </summary>
		ObjectID,
		///<summary>Status. </summary>
		Status,
		///<summary>NextStep. </summary>
		NextStep,
		///<summary>ContextLock. </summary>
		ContextLock,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ActionQueueSelection.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ActionQueueSelectionFieldIndex:int
	{
		///<summary>ActionQueueSelectionID. </summary>
		ActionQueueSelectionID,
		///<summary>ActionQueueID. </summary>
		ActionQueueID,
		///<summary>ObjectID. </summary>
		ObjectID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ActionQueueStep.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ActionQueueStepFieldIndex:int
	{
		///<summary>ActionQueueStepID. </summary>
		ActionQueueStepID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ActionQueueID. </summary>
		ActionQueueID,
		///<summary>StepStatus. </summary>
		StepStatus,
		///<summary>StepIndex. </summary>
		StepIndex,
		///<summary>StepName. </summary>
		StepName,
		///<summary>TaskIdentifier. </summary>
		TaskIdentifier,
		///<summary>TaskSettings. </summary>
		TaskSettings,
		///<summary>InputSource. </summary>
		InputSource,
		///<summary>InputFilterNodeID. </summary>
		InputFilterNodeID,
		///<summary>FilterCondition. </summary>
		FilterCondition,
		///<summary>FilterConditionNodeID. </summary>
		FilterConditionNodeID,
		///<summary>FlowSuccess. </summary>
		FlowSuccess,
		///<summary>FlowSkipped. </summary>
		FlowSkipped,
		///<summary>FlowError. </summary>
		FlowError,
		///<summary>AttemptDate. </summary>
		AttemptDate,
		///<summary>AttemptError. </summary>
		AttemptError,
		///<summary>AttemptCount. </summary>
		AttemptCount,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ActionTask.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ActionTaskFieldIndex:int
	{
		///<summary>ActionTaskID. </summary>
		ActionTaskID,
		///<summary>ActionID. </summary>
		ActionID,
		///<summary>TaskIdentifier. </summary>
		TaskIdentifier,
		///<summary>TaskSettings. </summary>
		TaskSettings,
		///<summary>StepIndex. </summary>
		StepIndex,
		///<summary>InputSource. </summary>
		InputSource,
		///<summary>InputFilterNodeID. </summary>
		InputFilterNodeID,
		///<summary>FilterCondition. </summary>
		FilterCondition,
		///<summary>FilterConditionNodeID. </summary>
		FilterConditionNodeID,
		///<summary>FlowSuccess. </summary>
		FlowSuccess,
		///<summary>FlowSkipped. </summary>
		FlowSkipped,
		///<summary>FlowError. </summary>
		FlowError,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: AmazonAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AmazonAccountFieldIndex:int
	{
		///<summary>AmazonAccountID. </summary>
		AmazonAccountID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>MerchantID. </summary>
		MerchantID,
		///<summary>AuthToken. </summary>
		AuthToken,
		///<summary>FirstName. </summary>
		FirstName,
		///<summary>MiddleName. </summary>
		MiddleName,
		///<summary>LastName. </summary>
		LastName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: AmazonASIN.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AmazonASINFieldIndex:int
	{
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>SKU. </summary>
		SKU,
		///<summary>AmazonASIN. </summary>
		AmazonASIN,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: AmazonOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AmazonOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>AmazonOrderID. </summary>
		AmazonOrderID,
		///<summary>AmazonCommission. </summary>
		AmazonCommission,
		///<summary>FulfillmentChannel. </summary>
		FulfillmentChannel,
		///<summary>IsPrime. </summary>
		IsPrime,
		///<summary>EarliestExpectedDeliveryDate. </summary>
		EarliestExpectedDeliveryDate,
		///<summary>LatestExpectedDeliveryDate. </summary>
		LatestExpectedDeliveryDate,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: AmazonOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AmazonOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>AmazonOrderItemCode. </summary>
		AmazonOrderItemCode,
		///<summary>ASIN. </summary>
		ASIN,
		///<summary>ConditionNote. </summary>
		ConditionNote,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: AmazonStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AmazonStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>AmazonApi. </summary>
		AmazonApi,
		///<summary>AmazonApiRegion. </summary>
		AmazonApiRegion,
		///<summary>SellerCentralUsername. </summary>
		SellerCentralUsername,
		///<summary>SellerCentralPassword. </summary>
		SellerCentralPassword,
		///<summary>MerchantName. </summary>
		MerchantName,
		///<summary>MerchantToken. </summary>
		MerchantToken,
		///<summary>AccessKeyID. </summary>
		AccessKeyID,
		///<summary>AuthToken. </summary>
		AuthToken,
		///<summary>Cookie. </summary>
		Cookie,
		///<summary>CookieExpires. </summary>
		CookieExpires,
		///<summary>CookieWaitUntil. </summary>
		CookieWaitUntil,
		///<summary>Certificate. </summary>
		Certificate,
		///<summary>WeightDownloads. </summary>
		WeightDownloads,
		///<summary>MerchantID. </summary>
		MerchantID,
		///<summary>MarketplaceID. </summary>
		MarketplaceID,
		///<summary>ExcludeFBA. </summary>
		ExcludeFBA,
		///<summary>DomainName. </summary>
		DomainName,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: AmeriCommerceStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AmeriCommerceStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>Username. </summary>
		Username,
		///<summary>Password. </summary>
		Password,
		///<summary>StoreUrl. </summary>
		StoreUrl,
		///<summary>StoreCode. </summary>
		StoreCode,
		///<summary>StatusCodes. </summary>
		StatusCodes,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Audit.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AuditFieldIndex:int
	{
		///<summary>AuditID. </summary>
		AuditID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>TransactionID. </summary>
		TransactionID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>ComputerID. </summary>
		ComputerID,
		///<summary>Reason. </summary>
		Reason,
		///<summary>ReasonDetail. </summary>
		ReasonDetail,
		///<summary>Date. </summary>
		Date,
		///<summary>Action. </summary>
		Action,
		///<summary>ObjectID. </summary>
		ObjectID,
		///<summary>HasEvents. </summary>
		HasEvents,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: AuditChange.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AuditChangeFieldIndex:int
	{
		///<summary>AuditChangeID. </summary>
		AuditChangeID,
		///<summary>AuditID. </summary>
		AuditID,
		///<summary>ChangeType. </summary>
		ChangeType,
		///<summary>ObjectID. </summary>
		ObjectID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: AuditChangeDetail.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum AuditChangeDetailFieldIndex:int
	{
		///<summary>AuditChangeDetailID. </summary>
		AuditChangeDetailID,
		///<summary>AuditChangeID. </summary>
		AuditChangeID,
		///<summary>AuditID. </summary>
		AuditID,
		///<summary>DisplayName. </summary>
		DisplayName,
		///<summary>DisplayFormat. </summary>
		DisplayFormat,
		///<summary>DataType. </summary>
		DataType,
		///<summary>TextOld. </summary>
		TextOld,
		///<summary>TextNew. </summary>
		TextNew,
		///<summary>VariantOld. </summary>
		VariantOld,
		///<summary>VariantNew. </summary>
		VariantNew,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: BestRateProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum BestRateProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>Weight. </summary>
		Weight,
		///<summary>ServiceLevel. </summary>
		ServiceLevel,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: BestRateShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum BestRateShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>ServiceLevel. </summary>
		ServiceLevel,
		///<summary>InsuranceValue. </summary>
		InsuranceValue,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: BigCommerceOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum BigCommerceOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>OrderAddressID. </summary>
		OrderAddressID,
		///<summary>OrderProductID. </summary>
		OrderProductID,
		///<summary>IsDigitalItem. </summary>
		IsDigitalItem,
		///<summary>EventDate. </summary>
		EventDate,
		///<summary>EventName. </summary>
		EventName,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: BigCommerceStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum BigCommerceStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>ApiUrl. </summary>
		ApiUrl,
		///<summary>ApiUserName. </summary>
		ApiUserName,
		///<summary>ApiToken. </summary>
		ApiToken,
		///<summary>StatusCodes. </summary>
		StatusCodes,
		///<summary>WeightUnitOfMeasure. </summary>
		WeightUnitOfMeasure,
		///<summary>DownloadModifiedNumberOfDaysBack. </summary>
		DownloadModifiedNumberOfDaysBack,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: BuyDotComOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum BuyDotComOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>ReceiptItemID. </summary>
		ReceiptItemID,
		///<summary>ListingID. </summary>
		ListingID,
		///<summary>Shipping. </summary>
		Shipping,
		///<summary>Tax. </summary>
		Tax,
		///<summary>Commission. </summary>
		Commission,
		///<summary>ItemFee. </summary>
		ItemFee,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: BuyDotComStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum BuyDotComStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>FtpUsername. </summary>
		FtpUsername,
		///<summary>FtpPassword. </summary>
		FtpPassword,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ChannelAdvisorOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ChannelAdvisorOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>CustomOrderIdentifier. </summary>
		CustomOrderIdentifier,
		///<summary>ResellerID. </summary>
		ResellerID,
		///<summary>OnlineShippingStatus. </summary>
		OnlineShippingStatus,
		///<summary>OnlineCheckoutStatus. </summary>
		OnlineCheckoutStatus,
		///<summary>OnlinePaymentStatus. </summary>
		OnlinePaymentStatus,
		///<summary>FlagStyle. </summary>
		FlagStyle,
		///<summary>FlagDescription. </summary>
		FlagDescription,
		///<summary>FlagType. </summary>
		FlagType,
		///<summary>MarketplaceNames. </summary>
		MarketplaceNames,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ChannelAdvisorOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ChannelAdvisorOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>MarketplaceName. </summary>
		MarketplaceName,
		///<summary>MarketplaceStoreName. </summary>
		MarketplaceStoreName,
		///<summary>MarketplaceBuyerID. </summary>
		MarketplaceBuyerID,
		///<summary>MarketplaceSalesID. </summary>
		MarketplaceSalesID,
		///<summary>Classification. </summary>
		Classification,
		///<summary>DistributionCenter. </summary>
		DistributionCenter,
		///<summary>HarmonizedCode. </summary>
		HarmonizedCode,
		///<summary>IsFBA. </summary>
		IsFBA,
		///<summary>MPN. </summary>
		MPN,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ChannelAdvisorStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ChannelAdvisorStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>AccountKey. </summary>
		AccountKey,
		///<summary>ProfileID. </summary>
		ProfileID,
		///<summary>AttributesToDownload. </summary>
		AttributesToDownload,
		///<summary>ConsolidatorAsUsps. </summary>
		ConsolidatorAsUsps,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ClickCartProOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ClickCartProOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>ClickCartProOrderID. </summary>
		ClickCartProOrderID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: CommerceInterfaceOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum CommerceInterfaceOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>CommerceInterfaceOrderNumber. </summary>
		CommerceInterfaceOrderNumber,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Computer.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ComputerFieldIndex:int
	{
		///<summary>ComputerID. </summary>
		ComputerID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Identifier. </summary>
		Identifier,
		///<summary>Name. </summary>
		Name,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Configuration.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ConfigurationFieldIndex:int
	{
		///<summary>ConfigurationID. </summary>
		ConfigurationID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>LogOnMethod. </summary>
		LogOnMethod,
		///<summary>AddressCasing. </summary>
		AddressCasing,
		///<summary>CustomerCompareEmail. </summary>
		CustomerCompareEmail,
		///<summary>CustomerCompareAddress. </summary>
		CustomerCompareAddress,
		///<summary>CustomerUpdateBilling. </summary>
		CustomerUpdateBilling,
		///<summary>CustomerUpdateShipping. </summary>
		CustomerUpdateShipping,
		///<summary>CustomerUpdateModifiedBilling. </summary>
		CustomerUpdateModifiedBilling,
		///<summary>CustomerUpdateModifiedShipping. </summary>
		CustomerUpdateModifiedShipping,
		///<summary>AuditNewOrders. </summary>
		AuditNewOrders,
		///<summary>AuditDeletedOrders. </summary>
		AuditDeletedOrders,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Customer.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum CustomerFieldIndex:int
	{
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>RollupOrderCount. </summary>
		RollupOrderCount,
		///<summary>RollupOrderTotal. </summary>
		RollupOrderTotal,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: DimensionsProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum DimensionsProfileFieldIndex:int
	{
		///<summary>DimensionsProfileID. </summary>
		DimensionsProfileID,
		///<summary>Name. </summary>
		Name,
		///<summary>Length. </summary>
		Length,
		///<summary>Width. </summary>
		Width,
		///<summary>Height. </summary>
		Height,
		///<summary>Weight. </summary>
		Weight,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Download.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum DownloadFieldIndex:int
	{
		///<summary>DownloadID. </summary>
		DownloadID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>ComputerID. </summary>
		ComputerID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>InitiatedBy. </summary>
		InitiatedBy,
		///<summary>Started. </summary>
		Started,
		///<summary>Ended. </summary>
		Ended,
		///<summary>Duration. </summary>
		Duration,
		///<summary>QuantityTotal. </summary>
		QuantityTotal,
		///<summary>QuantityNew. </summary>
		QuantityNew,
		///<summary>Result. </summary>
		Result,
		///<summary>ErrorMessage. </summary>
		ErrorMessage,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: DownloadDetail.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum DownloadDetailFieldIndex:int
	{
		///<summary>DownloadedDetailID. </summary>
		DownloadedDetailID,
		///<summary>DownloadID. </summary>
		DownloadID,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>InitialDownload. </summary>
		InitialDownload,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>ExtraBigIntData1. </summary>
		ExtraBigIntData1,
		///<summary>ExtraBigIntData2. </summary>
		ExtraBigIntData2,
		///<summary>ExtraBigIntData3. </summary>
		ExtraBigIntData3,
		///<summary>ExtraStringData1. </summary>
		ExtraStringData1,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EbayCombinedOrderRelation.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EbayCombinedOrderRelationFieldIndex:int
	{
		///<summary>EbayCombinedOrderRelationID. </summary>
		EbayCombinedOrderRelationID,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>EbayOrderID. </summary>
		EbayOrderID,
		///<summary>StoreID. </summary>
		StoreID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EbayOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EbayOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>EbayOrderID. </summary>
		EbayOrderID,
		///<summary>EbayBuyerID. </summary>
		EbayBuyerID,
		///<summary>CombinedLocally. </summary>
		CombinedLocally,
		///<summary>SelectedShippingMethod. </summary>
		SelectedShippingMethod,
		///<summary>SellingManagerRecord. </summary>
		SellingManagerRecord,
		///<summary>GspEligible. </summary>
		GspEligible,
		///<summary>GspFirstName. </summary>
		GspFirstName,
		///<summary>GspLastName. </summary>
		GspLastName,
		///<summary>GspStreet1. </summary>
		GspStreet1,
		///<summary>GspStreet2. </summary>
		GspStreet2,
		///<summary>GspCity. </summary>
		GspCity,
		///<summary>GspStateProvince. </summary>
		GspStateProvince,
		///<summary>GspPostalCode. </summary>
		GspPostalCode,
		///<summary>GspCountryCode. </summary>
		GspCountryCode,
		///<summary>GspReferenceID. </summary>
		GspReferenceID,
		///<summary>RollupEbayItemCount. </summary>
		RollupEbayItemCount,
		///<summary>RollupEffectiveCheckoutStatus. </summary>
		RollupEffectiveCheckoutStatus,
		///<summary>RollupEffectivePaymentMethod. </summary>
		RollupEffectivePaymentMethod,
		///<summary>RollupFeedbackLeftType. </summary>
		RollupFeedbackLeftType,
		///<summary>RollupFeedbackLeftComments. </summary>
		RollupFeedbackLeftComments,
		///<summary>RollupFeedbackReceivedType. </summary>
		RollupFeedbackReceivedType,
		///<summary>RollupFeedbackReceivedComments. </summary>
		RollupFeedbackReceivedComments,
		///<summary>RollupPayPalAddressStatus. </summary>
		RollupPayPalAddressStatus,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EbayOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EbayOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>LocalEbayOrderID. </summary>
		LocalEbayOrderID,
		///<summary>EbayItemID. </summary>
		EbayItemID,
		///<summary>EbayTransactionID. </summary>
		EbayTransactionID,
		///<summary>SellingManagerRecord. </summary>
		SellingManagerRecord,
		///<summary>EffectiveCheckoutStatus. </summary>
		EffectiveCheckoutStatus,
		///<summary>EffectivePaymentMethod. </summary>
		EffectivePaymentMethod,
		///<summary>PaymentStatus. </summary>
		PaymentStatus,
		///<summary>PaymentMethod. </summary>
		PaymentMethod,
		///<summary>CompleteStatus. </summary>
		CompleteStatus,
		///<summary>FeedbackLeftType. </summary>
		FeedbackLeftType,
		///<summary>FeedbackLeftComments. </summary>
		FeedbackLeftComments,
		///<summary>FeedbackReceivedType. </summary>
		FeedbackReceivedType,
		///<summary>FeedbackReceivedComments. </summary>
		FeedbackReceivedComments,
		///<summary>MyEbayPaid. </summary>
		MyEbayPaid,
		///<summary>MyEbayShipped. </summary>
		MyEbayShipped,
		///<summary>PayPalTransactionID. </summary>
		PayPalTransactionID,
		///<summary>PayPalAddressStatus. </summary>
		PayPalAddressStatus,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EbayStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EbayStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>EBayUserID. </summary>
		EBayUserID,
		///<summary>EBayToken. </summary>
		EBayToken,
		///<summary>EBayTokenExpire. </summary>
		EBayTokenExpire,
		///<summary>AcceptedPaymentList. </summary>
		AcceptedPaymentList,
		///<summary>DownloadItemDetails. </summary>
		DownloadItemDetails,
		///<summary>DownloadOlderOrders. </summary>
		DownloadOlderOrders,
		///<summary>DownloadPayPalDetails. </summary>
		DownloadPayPalDetails,
		///<summary>PayPalApiCredentialType. </summary>
		PayPalApiCredentialType,
		///<summary>PayPalApiUserName. </summary>
		PayPalApiUserName,
		///<summary>PayPalApiPassword. </summary>
		PayPalApiPassword,
		///<summary>PayPalApiSignature. </summary>
		PayPalApiSignature,
		///<summary>PayPalApiCertificate. </summary>
		PayPalApiCertificate,
		///<summary>DomesticShippingService. </summary>
		DomesticShippingService,
		///<summary>InternationalShippingService. </summary>
		InternationalShippingService,
		///<summary>FeedbackUpdatedThrough. </summary>
		FeedbackUpdatedThrough,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EmailAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EmailAccountFieldIndex:int
	{
		///<summary>EmailAccountID. </summary>
		EmailAccountID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>AccountName. </summary>
		AccountName,
		///<summary>DisplayName. </summary>
		DisplayName,
		///<summary>EmailAddress. </summary>
		EmailAddress,
		///<summary>IncomingServer. </summary>
		IncomingServer,
		///<summary>IncomingServerType. </summary>
		IncomingServerType,
		///<summary>IncomingPort. </summary>
		IncomingPort,
		///<summary>IncomingSecurityType. </summary>
		IncomingSecurityType,
		///<summary>IncomingUsername. </summary>
		IncomingUsername,
		///<summary>IncomingPassword. </summary>
		IncomingPassword,
		///<summary>OutgoingServer. </summary>
		OutgoingServer,
		///<summary>OutgoingPort. </summary>
		OutgoingPort,
		///<summary>OutgoingSecurityType. </summary>
		OutgoingSecurityType,
		///<summary>OutgoingCredentialSource. </summary>
		OutgoingCredentialSource,
		///<summary>OutgoingUsername. </summary>
		OutgoingUsername,
		///<summary>OutgoingPassword. </summary>
		OutgoingPassword,
		///<summary>AutoSend. </summary>
		AutoSend,
		///<summary>AutoSendMinutes. </summary>
		AutoSendMinutes,
		///<summary>AutoSendLastTime. </summary>
		AutoSendLastTime,
		///<summary>LimitMessagesPerConnection. </summary>
		LimitMessagesPerConnection,
		///<summary>LimitMessagesPerConnectionQuantity. </summary>
		LimitMessagesPerConnectionQuantity,
		///<summary>LimitMessagesPerHour. </summary>
		LimitMessagesPerHour,
		///<summary>LimitMessagesPerHourQuantity. </summary>
		LimitMessagesPerHourQuantity,
		///<summary>LimitMessageInterval. </summary>
		LimitMessageInterval,
		///<summary>LimitMessageIntervalSeconds. </summary>
		LimitMessageIntervalSeconds,
		///<summary>InternalOwnerID. </summary>
		InternalOwnerID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EmailOutbound.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EmailOutboundFieldIndex:int
	{
		///<summary>EmailOutboundID. </summary>
		EmailOutboundID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ContextID. </summary>
		ContextID,
		///<summary>ContextType. </summary>
		ContextType,
		///<summary>TemplateID. </summary>
		TemplateID,
		///<summary>AccountID. </summary>
		AccountID,
		///<summary>Visibility. </summary>
		Visibility,
		///<summary>FromAddress. </summary>
		FromAddress,
		///<summary>ToList. </summary>
		ToList,
		///<summary>CcList. </summary>
		CcList,
		///<summary>BccList. </summary>
		BccList,
		///<summary>Subject. </summary>
		Subject,
		///<summary>HtmlPartResourceID. </summary>
		HtmlPartResourceID,
		///<summary>PlainPartResourceID. </summary>
		PlainPartResourceID,
		///<summary>Encoding. </summary>
		Encoding,
		///<summary>ComposedDate. </summary>
		ComposedDate,
		///<summary>SentDate. </summary>
		SentDate,
		///<summary>DontSendBefore. </summary>
		DontSendBefore,
		///<summary>SendStatus. </summary>
		SendStatus,
		///<summary>SendAttemptCount. </summary>
		SendAttemptCount,
		///<summary>SendAttemptLastError. </summary>
		SendAttemptLastError,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EmailOutboundRelation.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EmailOutboundRelationFieldIndex:int
	{
		///<summary>EmailOutboundRelationID. </summary>
		EmailOutboundRelationID,
		///<summary>EmailOutboundID. </summary>
		EmailOutboundID,
		///<summary>ObjectID. </summary>
		ObjectID,
		///<summary>RelationType. </summary>
		RelationType,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EndiciaAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EndiciaAccountFieldIndex:int
	{
		///<summary>EndiciaAccountID. </summary>
		EndiciaAccountID,
		///<summary>EndiciaReseller. </summary>
		EndiciaReseller,
		///<summary>AccountNumber. </summary>
		AccountNumber,
		///<summary>SignupConfirmation. </summary>
		SignupConfirmation,
		///<summary>WebPassword. </summary>
		WebPassword,
		///<summary>ApiInitialPassword. </summary>
		ApiInitialPassword,
		///<summary>ApiUserPassword. </summary>
		ApiUserPassword,
		///<summary>AccountType. </summary>
		AccountType,
		///<summary>TestAccount. </summary>
		TestAccount,
		///<summary>CreatedByShipWorks. </summary>
		CreatedByShipWorks,
		///<summary>Description. </summary>
		Description,
		///<summary>FirstName. </summary>
		FirstName,
		///<summary>LastName. </summary>
		LastName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>MailingPostalCode. </summary>
		MailingPostalCode,
		///<summary>ScanFormAddressSource. </summary>
		ScanFormAddressSource,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EndiciaProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EndiciaProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>EndiciaAccountID. </summary>
		EndiciaAccountID,
		///<summary>StealthPostage. </summary>
		StealthPostage,
		///<summary>ReferenceID. </summary>
		ReferenceID,
		///<summary>ScanBasedReturn. </summary>
		ScanBasedReturn,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EndiciaScanForm.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EndiciaScanFormFieldIndex:int
	{
		///<summary>EndiciaScanFormID. </summary>
		EndiciaScanFormID,
		///<summary>EndiciaAccountID. </summary>
		EndiciaAccountID,
		///<summary>EndiciaAccountNumber. </summary>
		EndiciaAccountNumber,
		///<summary>SubmissionID. </summary>
		SubmissionID,
		///<summary>CreatedDate. </summary>
		CreatedDate,
		///<summary>ScanFormBatchID. </summary>
		ScanFormBatchID,
		///<summary>Description. </summary>
		Description,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EndiciaShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EndiciaShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>EndiciaAccountID. </summary>
		EndiciaAccountID,
		///<summary>OriginalEndiciaAccountID. </summary>
		OriginalEndiciaAccountID,
		///<summary>StealthPostage. </summary>
		StealthPostage,
		///<summary>ReferenceID. </summary>
		ReferenceID,
		///<summary>TransactionID. </summary>
		TransactionID,
		///<summary>RefundFormID. </summary>
		RefundFormID,
		///<summary>ScanFormBatchID. </summary>
		ScanFormBatchID,
		///<summary>ScanBasedReturn. </summary>
		ScanBasedReturn,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EtsyOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EtsyOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>WasPaid. </summary>
		WasPaid,
		///<summary>WasShipped. </summary>
		WasShipped,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: EtsyStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EtsyStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>EtsyShopID. </summary>
		EtsyShopID,
		///<summary>EtsyLoginName. </summary>
		EtsyLoginName,
		///<summary>EtsyStoreName. </summary>
		EtsyStoreName,
		///<summary>OAuthToken. </summary>
		OAuthToken,
		///<summary>OAuthTokenSecret. </summary>
		OAuthTokenSecret,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ExcludedPackageType.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ExcludedPackageTypeFieldIndex:int
	{
		///<summary>ShipmentType. </summary>
		ShipmentType,
		///<summary>PackageType. </summary>
		PackageType,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ExcludedServiceType.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ExcludedServiceTypeFieldIndex:int
	{
		///<summary>ShipmentType. </summary>
		ShipmentType,
		///<summary>ServiceType. </summary>
		ServiceType,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FedExAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FedExAccountFieldIndex:int
	{
		///<summary>FedExAccountID. </summary>
		FedExAccountID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Description. </summary>
		Description,
		///<summary>AccountNumber. </summary>
		AccountNumber,
		///<summary>SignatureRelease. </summary>
		SignatureRelease,
		///<summary>MeterNumber. </summary>
		MeterNumber,
		///<summary>SmartPostHubList. </summary>
		SmartPostHubList,
		///<summary>FirstName. </summary>
		FirstName,
		///<summary>MiddleName. </summary>
		MiddleName,
		///<summary>LastName. </summary>
		LastName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FedExEndOfDayClose.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FedExEndOfDayCloseFieldIndex:int
	{
		///<summary>FedExEndOfDayCloseID. </summary>
		FedExEndOfDayCloseID,
		///<summary>FedExAccountID. </summary>
		FedExAccountID,
		///<summary>AccountNumber. </summary>
		AccountNumber,
		///<summary>CloseDate. </summary>
		CloseDate,
		///<summary>IsSmartPost. </summary>
		IsSmartPost,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FedExPackage.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FedExPackageFieldIndex:int
	{
		///<summary>FedExPackageID. </summary>
		FedExPackageID,
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>Weight. </summary>
		Weight,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>SkidPieces. </summary>
		SkidPieces,
		///<summary>Insurance. </summary>
		Insurance,
		///<summary>InsuranceValue. </summary>
		InsuranceValue,
		///<summary>InsurancePennyOne. </summary>
		InsurancePennyOne,
		///<summary>DeclaredValue. </summary>
		DeclaredValue,
		///<summary>TrackingNumber. </summary>
		TrackingNumber,
		///<summary>PriorityAlert. </summary>
		PriorityAlert,
		///<summary>PriorityAlertEnhancementType. </summary>
		PriorityAlertEnhancementType,
		///<summary>PriorityAlertDetailContent. </summary>
		PriorityAlertDetailContent,
		///<summary>DryIceWeight. </summary>
		DryIceWeight,
		///<summary>ContainsAlcohol. </summary>
		ContainsAlcohol,
		///<summary>DangerousGoodsEnabled. </summary>
		DangerousGoodsEnabled,
		///<summary>DangerousGoodsType. </summary>
		DangerousGoodsType,
		///<summary>DangerousGoodsAccessibilityType. </summary>
		DangerousGoodsAccessibilityType,
		///<summary>DangerousGoodsCargoAircraftOnly. </summary>
		DangerousGoodsCargoAircraftOnly,
		///<summary>DangerousGoodsEmergencyContactPhone. </summary>
		DangerousGoodsEmergencyContactPhone,
		///<summary>DangerousGoodsOfferor. </summary>
		DangerousGoodsOfferor,
		///<summary>DangerousGoodsPackagingCount. </summary>
		DangerousGoodsPackagingCount,
		///<summary>HazardousMaterialNumber. </summary>
		HazardousMaterialNumber,
		///<summary>HazardousMaterialClass. </summary>
		HazardousMaterialClass,
		///<summary>HazardousMaterialProperName. </summary>
		HazardousMaterialProperName,
		///<summary>HazardousMaterialPackingGroup. </summary>
		HazardousMaterialPackingGroup,
		///<summary>HazardousMaterialQuantityValue. </summary>
		HazardousMaterialQuantityValue,
		///<summary>HazardousMaterialQuanityUnits. </summary>
		HazardousMaterialQuanityUnits,
		///<summary>HazardousMaterialTechnicalName. </summary>
		HazardousMaterialTechnicalName,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FedExProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FedExProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>FedExAccountID. </summary>
		FedExAccountID,
		///<summary>Service. </summary>
		Service,
		///<summary>Signature. </summary>
		Signature,
		///<summary>PackagingType. </summary>
		PackagingType,
		///<summary>NonStandardContainer. </summary>
		NonStandardContainer,
		///<summary>ReferenceCustomer. </summary>
		ReferenceCustomer,
		///<summary>ReferenceInvoice. </summary>
		ReferenceInvoice,
		///<summary>ReferencePO. </summary>
		ReferencePO,
		///<summary>ReferenceShipmentIntegrity. </summary>
		ReferenceShipmentIntegrity,
		///<summary>PayorTransportType. </summary>
		PayorTransportType,
		///<summary>PayorTransportAccount. </summary>
		PayorTransportAccount,
		///<summary>PayorDutiesType. </summary>
		PayorDutiesType,
		///<summary>PayorDutiesAccount. </summary>
		PayorDutiesAccount,
		///<summary>SaturdayDelivery. </summary>
		SaturdayDelivery,
		///<summary>EmailNotifySender. </summary>
		EmailNotifySender,
		///<summary>EmailNotifyRecipient. </summary>
		EmailNotifyRecipient,
		///<summary>EmailNotifyOther. </summary>
		EmailNotifyOther,
		///<summary>EmailNotifyOtherAddress. </summary>
		EmailNotifyOtherAddress,
		///<summary>EmailNotifyMessage. </summary>
		EmailNotifyMessage,
		///<summary>ResidentialDetermination. </summary>
		ResidentialDetermination,
		///<summary>SmartPostIndicia. </summary>
		SmartPostIndicia,
		///<summary>SmartPostEndorsement. </summary>
		SmartPostEndorsement,
		///<summary>SmartPostConfirmation. </summary>
		SmartPostConfirmation,
		///<summary>SmartPostCustomerManifest. </summary>
		SmartPostCustomerManifest,
		///<summary>SmartPostHubID. </summary>
		SmartPostHubID,
		///<summary>EmailNotifyBroker. </summary>
		EmailNotifyBroker,
		///<summary>DropoffType. </summary>
		DropoffType,
		///<summary>OriginResidentialDetermination. </summary>
		OriginResidentialDetermination,
		///<summary>PayorTransportName. </summary>
		PayorTransportName,
		///<summary>ReturnType. </summary>
		ReturnType,
		///<summary>RmaNumber. </summary>
		RmaNumber,
		///<summary>RmaReason. </summary>
		RmaReason,
		///<summary>ReturnSaturdayPickup. </summary>
		ReturnSaturdayPickup,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FedExProfilePackage.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FedExProfilePackageFieldIndex:int
	{
		///<summary>FedExProfilePackageID. </summary>
		FedExProfilePackageID,
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>Weight. </summary>
		Weight,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>PriorityAlert. </summary>
		PriorityAlert,
		///<summary>PriorityAlertEnhancementType. </summary>
		PriorityAlertEnhancementType,
		///<summary>PriorityAlertDetailContent. </summary>
		PriorityAlertDetailContent,
		///<summary>DryIceWeight. </summary>
		DryIceWeight,
		///<summary>ContainsAlcohol. </summary>
		ContainsAlcohol,
		///<summary>DangerousGoodsEnabled. </summary>
		DangerousGoodsEnabled,
		///<summary>DangerousGoodsType. </summary>
		DangerousGoodsType,
		///<summary>DangerousGoodsAccessibilityType. </summary>
		DangerousGoodsAccessibilityType,
		///<summary>DangerousGoodsCargoAircraftOnly. </summary>
		DangerousGoodsCargoAircraftOnly,
		///<summary>DangerousGoodsEmergencyContactPhone. </summary>
		DangerousGoodsEmergencyContactPhone,
		///<summary>DangerousGoodsOfferor. </summary>
		DangerousGoodsOfferor,
		///<summary>DangerousGoodsPackagingCount. </summary>
		DangerousGoodsPackagingCount,
		///<summary>HazardousMaterialNumber. </summary>
		HazardousMaterialNumber,
		///<summary>HazardousMaterialClass. </summary>
		HazardousMaterialClass,
		///<summary>HazardousMaterialProperName. </summary>
		HazardousMaterialProperName,
		///<summary>HazardousMaterialPackingGroup. </summary>
		HazardousMaterialPackingGroup,
		///<summary>HazardousMaterialQuantityValue. </summary>
		HazardousMaterialQuantityValue,
		///<summary>HazardousMaterialQuanityUnits. </summary>
		HazardousMaterialQuanityUnits,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FedExShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FedExShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>FedExAccountID. </summary>
		FedExAccountID,
		///<summary>MasterFormID. </summary>
		MasterFormID,
		///<summary>Service. </summary>
		Service,
		///<summary>Signature. </summary>
		Signature,
		///<summary>PackagingType. </summary>
		PackagingType,
		///<summary>NonStandardContainer. </summary>
		NonStandardContainer,
		///<summary>ReferenceCustomer. </summary>
		ReferenceCustomer,
		///<summary>ReferenceInvoice. </summary>
		ReferenceInvoice,
		///<summary>ReferencePO. </summary>
		ReferencePO,
		///<summary>ReferenceShipmentIntegrity. </summary>
		ReferenceShipmentIntegrity,
		///<summary>PayorTransportType. </summary>
		PayorTransportType,
		///<summary>PayorTransportName. </summary>
		PayorTransportName,
		///<summary>PayorTransportAccount. </summary>
		PayorTransportAccount,
		///<summary>PayorDutiesType. </summary>
		PayorDutiesType,
		///<summary>PayorDutiesAccount. </summary>
		PayorDutiesAccount,
		///<summary>PayorDutiesName. </summary>
		PayorDutiesName,
		///<summary>PayorDutiesCountryCode. </summary>
		PayorDutiesCountryCode,
		///<summary>SaturdayDelivery. </summary>
		SaturdayDelivery,
		///<summary>HomeDeliveryType. </summary>
		HomeDeliveryType,
		///<summary>HomeDeliveryInstructions. </summary>
		HomeDeliveryInstructions,
		///<summary>HomeDeliveryDate. </summary>
		HomeDeliveryDate,
		///<summary>HomeDeliveryPhone. </summary>
		HomeDeliveryPhone,
		///<summary>FreightInsidePickup. </summary>
		FreightInsidePickup,
		///<summary>FreightInsideDelivery. </summary>
		FreightInsideDelivery,
		///<summary>FreightBookingNumber. </summary>
		FreightBookingNumber,
		///<summary>FreightLoadAndCount. </summary>
		FreightLoadAndCount,
		///<summary>EmailNotifyBroker. </summary>
		EmailNotifyBroker,
		///<summary>EmailNotifySender. </summary>
		EmailNotifySender,
		///<summary>EmailNotifyRecipient. </summary>
		EmailNotifyRecipient,
		///<summary>EmailNotifyOther. </summary>
		EmailNotifyOther,
		///<summary>EmailNotifyOtherAddress. </summary>
		EmailNotifyOtherAddress,
		///<summary>EmailNotifyMessage. </summary>
		EmailNotifyMessage,
		///<summary>CodEnabled. </summary>
		CodEnabled,
		///<summary>CodAmount. </summary>
		CodAmount,
		///<summary>CodPaymentType. </summary>
		CodPaymentType,
		///<summary>CodAddFreight. </summary>
		CodAddFreight,
		///<summary>CodOriginID. </summary>
		CodOriginID,
		///<summary>CodFirstName. </summary>
		CodFirstName,
		///<summary>CodLastName. </summary>
		CodLastName,
		///<summary>CodCompany. </summary>
		CodCompany,
		///<summary>CodStreet1. </summary>
		CodStreet1,
		///<summary>CodStreet2. </summary>
		CodStreet2,
		///<summary>CodStreet3. </summary>
		CodStreet3,
		///<summary>CodCity. </summary>
		CodCity,
		///<summary>CodStateProvCode. </summary>
		CodStateProvCode,
		///<summary>CodPostalCode. </summary>
		CodPostalCode,
		///<summary>CodCountryCode. </summary>
		CodCountryCode,
		///<summary>CodPhone. </summary>
		CodPhone,
		///<summary>CodTrackingNumber. </summary>
		CodTrackingNumber,
		///<summary>CodTrackingFormID. </summary>
		CodTrackingFormID,
		///<summary>CodTIN. </summary>
		CodTIN,
		///<summary>CodChargeBasis. </summary>
		CodChargeBasis,
		///<summary>CodAccountNumber. </summary>
		CodAccountNumber,
		///<summary>BrokerEnabled. </summary>
		BrokerEnabled,
		///<summary>BrokerAccount. </summary>
		BrokerAccount,
		///<summary>BrokerFirstName. </summary>
		BrokerFirstName,
		///<summary>BrokerLastName. </summary>
		BrokerLastName,
		///<summary>BrokerCompany. </summary>
		BrokerCompany,
		///<summary>BrokerStreet1. </summary>
		BrokerStreet1,
		///<summary>BrokerStreet2. </summary>
		BrokerStreet2,
		///<summary>BrokerStreet3. </summary>
		BrokerStreet3,
		///<summary>BrokerCity. </summary>
		BrokerCity,
		///<summary>BrokerStateProvCode. </summary>
		BrokerStateProvCode,
		///<summary>BrokerPostalCode. </summary>
		BrokerPostalCode,
		///<summary>BrokerCountryCode. </summary>
		BrokerCountryCode,
		///<summary>BrokerPhone. </summary>
		BrokerPhone,
		///<summary>BrokerPhoneExtension. </summary>
		BrokerPhoneExtension,
		///<summary>BrokerEmail. </summary>
		BrokerEmail,
		///<summary>CustomsAdmissibilityPackaging. </summary>
		CustomsAdmissibilityPackaging,
		///<summary>CustomsRecipientTIN. </summary>
		CustomsRecipientTIN,
		///<summary>CustomsDocumentsOnly. </summary>
		CustomsDocumentsOnly,
		///<summary>CustomsDocumentsDescription. </summary>
		CustomsDocumentsDescription,
		///<summary>CustomsExportFilingOption. </summary>
		CustomsExportFilingOption,
		///<summary>CustomsAESEEI. </summary>
		CustomsAESEEI,
		///<summary>CustomsRecipientIdentificationType. </summary>
		CustomsRecipientIdentificationType,
		///<summary>CustomsRecipientIdentificationValue. </summary>
		CustomsRecipientIdentificationValue,
		///<summary>CustomsOptionsType. </summary>
		CustomsOptionsType,
		///<summary>CustomsOptionsDesription. </summary>
		CustomsOptionsDesription,
		///<summary>CommercialInvoice. </summary>
		CommercialInvoice,
		///<summary>CommercialInvoiceFileElectronically. </summary>
		CommercialInvoiceFileElectronically,
		///<summary>CommercialInvoiceTermsOfSale. </summary>
		CommercialInvoiceTermsOfSale,
		///<summary>CommercialInvoicePurpose. </summary>
		CommercialInvoicePurpose,
		///<summary>CommercialInvoiceComments. </summary>
		CommercialInvoiceComments,
		///<summary>CommercialInvoiceFreight. </summary>
		CommercialInvoiceFreight,
		///<summary>CommercialInvoiceInsurance. </summary>
		CommercialInvoiceInsurance,
		///<summary>CommercialInvoiceOther. </summary>
		CommercialInvoiceOther,
		///<summary>CommercialInvoiceReference. </summary>
		CommercialInvoiceReference,
		///<summary>ImporterOfRecord. </summary>
		ImporterOfRecord,
		///<summary>ImporterAccount. </summary>
		ImporterAccount,
		///<summary>ImporterTIN. </summary>
		ImporterTIN,
		///<summary>ImporterFirstName. </summary>
		ImporterFirstName,
		///<summary>ImporterLastName. </summary>
		ImporterLastName,
		///<summary>ImporterCompany. </summary>
		ImporterCompany,
		///<summary>ImporterStreet1. </summary>
		ImporterStreet1,
		///<summary>ImporterStreet2. </summary>
		ImporterStreet2,
		///<summary>ImporterStreet3. </summary>
		ImporterStreet3,
		///<summary>ImporterCity. </summary>
		ImporterCity,
		///<summary>ImporterStateProvCode. </summary>
		ImporterStateProvCode,
		///<summary>ImporterPostalCode. </summary>
		ImporterPostalCode,
		///<summary>ImporterCountryCode. </summary>
		ImporterCountryCode,
		///<summary>ImporterPhone. </summary>
		ImporterPhone,
		///<summary>SmartPostIndicia. </summary>
		SmartPostIndicia,
		///<summary>SmartPostEndorsement. </summary>
		SmartPostEndorsement,
		///<summary>SmartPostConfirmation. </summary>
		SmartPostConfirmation,
		///<summary>SmartPostCustomerManifest. </summary>
		SmartPostCustomerManifest,
		///<summary>SmartPostHubID. </summary>
		SmartPostHubID,
		///<summary>SmartPostUspsApplicationId. </summary>
		SmartPostUspsApplicationId,
		///<summary>DropoffType. </summary>
		DropoffType,
		///<summary>OriginResidentialDetermination. </summary>
		OriginResidentialDetermination,
		///<summary>FedExHoldAtLocationEnabled. </summary>
		FedExHoldAtLocationEnabled,
		///<summary>HoldLocationId. </summary>
		HoldLocationId,
		///<summary>HoldLocationType. </summary>
		HoldLocationType,
		///<summary>HoldContactId. </summary>
		HoldContactId,
		///<summary>HoldPersonName. </summary>
		HoldPersonName,
		///<summary>HoldTitle. </summary>
		HoldTitle,
		///<summary>HoldCompanyName. </summary>
		HoldCompanyName,
		///<summary>HoldPhoneNumber. </summary>
		HoldPhoneNumber,
		///<summary>HoldPhoneExtension. </summary>
		HoldPhoneExtension,
		///<summary>HoldPagerNumber. </summary>
		HoldPagerNumber,
		///<summary>HoldFaxNumber. </summary>
		HoldFaxNumber,
		///<summary>HoldEmailAddress. </summary>
		HoldEmailAddress,
		///<summary>HoldStreet1. </summary>
		HoldStreet1,
		///<summary>HoldStreet2. </summary>
		HoldStreet2,
		///<summary>HoldStreet3. </summary>
		HoldStreet3,
		///<summary>HoldCity. </summary>
		HoldCity,
		///<summary>HoldStateOrProvinceCode. </summary>
		HoldStateOrProvinceCode,
		///<summary>HoldPostalCode. </summary>
		HoldPostalCode,
		///<summary>HoldUrbanizationCode. </summary>
		HoldUrbanizationCode,
		///<summary>HoldCountryCode. </summary>
		HoldCountryCode,
		///<summary>HoldResidential. </summary>
		HoldResidential,
		///<summary>CustomsNaftaEnabled. </summary>
		CustomsNaftaEnabled,
		///<summary>CustomsNaftaPreferenceType. </summary>
		CustomsNaftaPreferenceType,
		///<summary>CustomsNaftaDeterminationCode. </summary>
		CustomsNaftaDeterminationCode,
		///<summary>CustomsNaftaProducerId. </summary>
		CustomsNaftaProducerId,
		///<summary>CustomsNaftaNetCostMethod. </summary>
		CustomsNaftaNetCostMethod,
		///<summary>ReturnType. </summary>
		ReturnType,
		///<summary>RmaNumber. </summary>
		RmaNumber,
		///<summary>RmaReason. </summary>
		RmaReason,
		///<summary>ReturnSaturdayPickup. </summary>
		ReturnSaturdayPickup,
		///<summary>TrafficInArmsLicenseNumber. </summary>
		TrafficInArmsLicenseNumber,
		///<summary>IntlExportDetailType. </summary>
		IntlExportDetailType,
		///<summary>IntlExportDetailForeignTradeZoneCode. </summary>
		IntlExportDetailForeignTradeZoneCode,
		///<summary>IntlExportDetailEntryNumber. </summary>
		IntlExportDetailEntryNumber,
		///<summary>IntlExportDetailLicenseOrPermitNumber. </summary>
		IntlExportDetailLicenseOrPermitNumber,
		///<summary>IntlExportDetailLicenseOrPermitExpirationDate. </summary>
		IntlExportDetailLicenseOrPermitExpirationDate,
		///<summary>WeightUnitType. </summary>
		WeightUnitType,
		///<summary>LinearUnitType. </summary>
		LinearUnitType,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Filter.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FilterFieldIndex:int
	{
		///<summary>FilterID. </summary>
		FilterID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Name. </summary>
		Name,
		///<summary>FilterTarget. </summary>
		FilterTarget,
		///<summary>IsFolder. </summary>
		IsFolder,
		///<summary>Definition. </summary>
		Definition,
		///<summary>State. </summary>
		State,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FilterLayout.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FilterLayoutFieldIndex:int
	{
		///<summary>FilterLayoutID. </summary>
		FilterLayoutID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>UserID. </summary>
		UserID,
		///<summary>FilterTarget. </summary>
		FilterTarget,
		///<summary>FilterNodeID. </summary>
		FilterNodeID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FilterNode.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FilterNodeFieldIndex:int
	{
		///<summary>FilterNodeID. </summary>
		FilterNodeID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ParentFilterNodeID. </summary>
		ParentFilterNodeID,
		///<summary>FilterSequenceID. </summary>
		FilterSequenceID,
		///<summary>FilterNodeContentID. </summary>
		FilterNodeContentID,
		///<summary>Created. </summary>
		Created,
		///<summary>Purpose. </summary>
		Purpose,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FilterNodeColumnSettings.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FilterNodeColumnSettingsFieldIndex:int
	{
		///<summary>FilterNodeColumnSettingsID. </summary>
		FilterNodeColumnSettingsID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>FilterNodeID. </summary>
		FilterNodeID,
		///<summary>Inherit. </summary>
		Inherit,
		///<summary>GridColumnLayoutID. </summary>
		GridColumnLayoutID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FilterNodeContent.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FilterNodeContentFieldIndex:int
	{
		///<summary>FilterNodeContentID. </summary>
		FilterNodeContentID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>CountVersion. </summary>
		CountVersion,
		///<summary>Status. </summary>
		Status,
		///<summary>InitialCalculation. </summary>
		InitialCalculation,
		///<summary>UpdateCalculation. </summary>
		UpdateCalculation,
		///<summary>ColumnMask. </summary>
		ColumnMask,
		///<summary>JoinMask. </summary>
		JoinMask,
		///<summary>Cost. </summary>
		Cost,
		///<summary>Count. </summary>
		Count,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FilterNodeContentDetail.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FilterNodeContentDetailFieldIndex:int
	{
		///<summary>FilterNodeContentID. </summary>
		FilterNodeContentID,
		///<summary>ObjectID. </summary>
		ObjectID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FilterSequence.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FilterSequenceFieldIndex:int
	{
		///<summary>FilterSequenceID. </summary>
		FilterSequenceID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ParentFilterID. </summary>
		ParentFilterID,
		///<summary>FilterID. </summary>
		FilterID,
		///<summary>Position. </summary>
		Position,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: FtpAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum FtpAccountFieldIndex:int
	{
		///<summary>FtpAccountID. </summary>
		FtpAccountID,
		///<summary>Host. </summary>
		Host,
		///<summary>Username. </summary>
		Username,
		///<summary>Password. </summary>
		Password,
		///<summary>Port. </summary>
		Port,
		///<summary>SecurityType. </summary>
		SecurityType,
		///<summary>Passive. </summary>
		Passive,
		///<summary>InternalOwnerID. </summary>
		InternalOwnerID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: GenericFileStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum GenericFileStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>FileFormat. </summary>
		FileFormat,
		///<summary>FileSource. </summary>
		FileSource,
		///<summary>DiskFolder. </summary>
		DiskFolder,
		///<summary>FtpAccountID. </summary>
		FtpAccountID,
		///<summary>FtpFolder. </summary>
		FtpFolder,
		///<summary>EmailAccountID. </summary>
		EmailAccountID,
		///<summary>EmailIncomingFolder. </summary>
		EmailIncomingFolder,
		///<summary>EmailFolderValidityID. </summary>
		EmailFolderValidityID,
		///<summary>EmailFolderLastMessageID. </summary>
		EmailFolderLastMessageID,
		///<summary>EmailOnlyUnread. </summary>
		EmailOnlyUnread,
		///<summary>NamePatternMatch. </summary>
		NamePatternMatch,
		///<summary>NamePatternSkip. </summary>
		NamePatternSkip,
		///<summary>SuccessAction. </summary>
		SuccessAction,
		///<summary>SuccessMoveFolder. </summary>
		SuccessMoveFolder,
		///<summary>ErrorAction. </summary>
		ErrorAction,
		///<summary>ErrorMoveFolder. </summary>
		ErrorMoveFolder,
		///<summary>XmlXsltFileName. </summary>
		XmlXsltFileName,
		///<summary>XmlXsltContent. </summary>
		XmlXsltContent,
		///<summary>FlatImportMap. </summary>
		FlatImportMap,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: GenericModuleStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum GenericModuleStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>ModuleUsername. </summary>
		ModuleUsername,
		///<summary>ModulePassword. </summary>
		ModulePassword,
		///<summary>ModuleUrl. </summary>
		ModuleUrl,
		///<summary>ModuleVersion. </summary>
		ModuleVersion,
		///<summary>ModulePlatform. </summary>
		ModulePlatform,
		///<summary>ModuleDeveloper. </summary>
		ModuleDeveloper,
		///<summary>ModuleOnlineStoreCode. </summary>
		ModuleOnlineStoreCode,
		///<summary>ModuleStatusCodes. </summary>
		ModuleStatusCodes,
		///<summary>ModuleDownloadPageSize. </summary>
		ModuleDownloadPageSize,
		///<summary>ModuleRequestTimeout. </summary>
		ModuleRequestTimeout,
		///<summary>ModuleDownloadStrategy. </summary>
		ModuleDownloadStrategy,
		///<summary>ModuleOnlineStatusSupport. </summary>
		ModuleOnlineStatusSupport,
		///<summary>ModuleOnlineStatusDataType. </summary>
		ModuleOnlineStatusDataType,
		///<summary>ModuleOnlineCustomerSupport. </summary>
		ModuleOnlineCustomerSupport,
		///<summary>ModuleOnlineCustomerDataType. </summary>
		ModuleOnlineCustomerDataType,
		///<summary>ModuleOnlineShipmentDetails. </summary>
		ModuleOnlineShipmentDetails,
		///<summary>ModuleHttpExpect100Continue. </summary>
		ModuleHttpExpect100Continue,
		///<summary>ModuleResponseEncoding. </summary>
		ModuleResponseEncoding,
		///<summary>SchemaVersion. </summary>
		SchemaVersion,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: GridColumnFormat.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum GridColumnFormatFieldIndex:int
	{
		///<summary>GridColumnFormatID. </summary>
		GridColumnFormatID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>ColumnGuid. </summary>
		ColumnGuid,
		///<summary>Settings. </summary>
		Settings,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: GridColumnLayout.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum GridColumnLayoutFieldIndex:int
	{
		///<summary>GridColumnLayoutID. </summary>
		GridColumnLayoutID,
		///<summary>DefinitionSet. </summary>
		DefinitionSet,
		///<summary>DefaultSortColumnGuid. </summary>
		DefaultSortColumnGuid,
		///<summary>DefaultSortOrder. </summary>
		DefaultSortOrder,
		///<summary>LastSortColumnGuid. </summary>
		LastSortColumnGuid,
		///<summary>LastSortOrder. </summary>
		LastSortOrder,
		///<summary>DetailViewSettings. </summary>
		DetailViewSettings,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: GridColumnPosition.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum GridColumnPositionFieldIndex:int
	{
		///<summary>GridColumnPositionID. </summary>
		GridColumnPositionID,
		///<summary>GridColumnLayoutID. </summary>
		GridColumnLayoutID,
		///<summary>ColumnGuid. </summary>
		ColumnGuid,
		///<summary>Visible. </summary>
		Visible,
		///<summary>Width. </summary>
		Width,
		///<summary>Position. </summary>
		Position,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: GrouponOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum GrouponOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>GrouponOrderID. </summary>
		GrouponOrderID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: GrouponOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum GrouponOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>Permalink. </summary>
		Permalink,
		///<summary>ChannelSKUProvided. </summary>
		ChannelSKUProvided,
		///<summary>FulfillmentLineItemID. </summary>
		FulfillmentLineItemID,
		///<summary>BomSKU. </summary>
		BomSKU,
		///<summary>GrouponLineItemID. </summary>
		GrouponLineItemID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: GrouponStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum GrouponStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>SupplierID. </summary>
		SupplierID,
		///<summary>Token. </summary>
		Token,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: InfopiaOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum InfopiaOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>Marketplace. </summary>
		Marketplace,
		///<summary>MarketplaceItemID. </summary>
		MarketplaceItemID,
		///<summary>BuyerID. </summary>
		BuyerID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: InfopiaStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum InfopiaStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>ApiToken. </summary>
		ApiToken,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: InsurancePolicy.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum InsurancePolicyFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>InsureShipStoreName. </summary>
		InsureShipStoreName,
		///<summary>CreatedWithApi. </summary>
		CreatedWithApi,
		///<summary>ItemName. </summary>
		ItemName,
		///<summary>Description. </summary>
		Description,
		///<summary>ClaimType. </summary>
		ClaimType,
		///<summary>DamageValue. </summary>
		DamageValue,
		///<summary>SubmissionDate. </summary>
		SubmissionDate,
		///<summary>ClaimID. </summary>
		ClaimID,
		///<summary>EmailAddress. </summary>
		EmailAddress,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: IParcelAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum IParcelAccountFieldIndex:int
	{
		///<summary>IParcelAccountID. </summary>
		IParcelAccountID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Username. </summary>
		Username,
		///<summary>Password. </summary>
		Password,
		///<summary>Description. </summary>
		Description,
		///<summary>FirstName. </summary>
		FirstName,
		///<summary>MiddleName. </summary>
		MiddleName,
		///<summary>LastName. </summary>
		LastName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: IParcelPackage.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum IParcelPackageFieldIndex:int
	{
		///<summary>IParcelPackageID. </summary>
		IParcelPackageID,
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>Weight. </summary>
		Weight,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>Insurance. </summary>
		Insurance,
		///<summary>InsuranceValue. </summary>
		InsuranceValue,
		///<summary>InsurancePennyOne. </summary>
		InsurancePennyOne,
		///<summary>DeclaredValue. </summary>
		DeclaredValue,
		///<summary>TrackingNumber. </summary>
		TrackingNumber,
		///<summary>ParcelNumber. </summary>
		ParcelNumber,
		///<summary>SkuAndQuantities. </summary>
		SkuAndQuantities,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: IParcelProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum IParcelProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>IParcelAccountID. </summary>
		IParcelAccountID,
		///<summary>Service. </summary>
		Service,
		///<summary>Reference. </summary>
		Reference,
		///<summary>TrackByEmail. </summary>
		TrackByEmail,
		///<summary>TrackBySMS. </summary>
		TrackBySMS,
		///<summary>IsDeliveryDutyPaid. </summary>
		IsDeliveryDutyPaid,
		///<summary>SkuAndQuantities. </summary>
		SkuAndQuantities,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: IParcelProfilePackage.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum IParcelProfilePackageFieldIndex:int
	{
		///<summary>IParcelProfilePackageID. </summary>
		IParcelProfilePackageID,
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>Weight. </summary>
		Weight,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: IParcelShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum IParcelShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>IParcelAccountID. </summary>
		IParcelAccountID,
		///<summary>Service. </summary>
		Service,
		///<summary>Reference. </summary>
		Reference,
		///<summary>TrackByEmail. </summary>
		TrackByEmail,
		///<summary>TrackBySMS. </summary>
		TrackBySMS,
		///<summary>IsDeliveryDutyPaid. </summary>
		IsDeliveryDutyPaid,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: LabelSheet.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum LabelSheetFieldIndex:int
	{
		///<summary>LabelSheetID. </summary>
		LabelSheetID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Name. </summary>
		Name,
		///<summary>PaperSizeHeight. </summary>
		PaperSizeHeight,
		///<summary>PaperSizeWidth. </summary>
		PaperSizeWidth,
		///<summary>MarginTop. </summary>
		MarginTop,
		///<summary>MarginLeft. </summary>
		MarginLeft,
		///<summary>LabelHeight. </summary>
		LabelHeight,
		///<summary>LabelWidth. </summary>
		LabelWidth,
		///<summary>VerticalSpacing. </summary>
		VerticalSpacing,
		///<summary>HorizontalSpacing. </summary>
		HorizontalSpacing,
		///<summary>Rows. </summary>
		Rows,
		///<summary>Columns. </summary>
		Columns,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: MagentoOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum MagentoOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>MagentoOrderID. </summary>
		MagentoOrderID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: MagentoStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum MagentoStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. Inherited from GenericModuleStore</summary>
		StoreID_GenericModuleStore,
		///<summary>ModuleUsername. </summary>
		ModuleUsername,
		///<summary>ModulePassword. </summary>
		ModulePassword,
		///<summary>ModuleUrl. </summary>
		ModuleUrl,
		///<summary>ModuleVersion. </summary>
		ModuleVersion,
		///<summary>ModulePlatform. </summary>
		ModulePlatform,
		///<summary>ModuleDeveloper. </summary>
		ModuleDeveloper,
		///<summary>ModuleOnlineStoreCode. </summary>
		ModuleOnlineStoreCode,
		///<summary>ModuleStatusCodes. </summary>
		ModuleStatusCodes,
		///<summary>ModuleDownloadPageSize. </summary>
		ModuleDownloadPageSize,
		///<summary>ModuleRequestTimeout. </summary>
		ModuleRequestTimeout,
		///<summary>ModuleDownloadStrategy. </summary>
		ModuleDownloadStrategy,
		///<summary>ModuleOnlineStatusSupport. </summary>
		ModuleOnlineStatusSupport,
		///<summary>ModuleOnlineStatusDataType. </summary>
		ModuleOnlineStatusDataType,
		///<summary>ModuleOnlineCustomerSupport. </summary>
		ModuleOnlineCustomerSupport,
		///<summary>ModuleOnlineCustomerDataType. </summary>
		ModuleOnlineCustomerDataType,
		///<summary>ModuleOnlineShipmentDetails. </summary>
		ModuleOnlineShipmentDetails,
		///<summary>ModuleHttpExpect100Continue. </summary>
		ModuleHttpExpect100Continue,
		///<summary>ModuleResponseEncoding. </summary>
		ModuleResponseEncoding,
		///<summary>SchemaVersion. </summary>
		SchemaVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>MagentoTrackingEmails. </summary>
		MagentoTrackingEmails,
		///<summary>MagentoConnect. </summary>
		MagentoConnect,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: MarketplaceAdvisorOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum MarketplaceAdvisorOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>BuyerNumber. </summary>
		BuyerNumber,
		///<summary>SellerOrderNumber. </summary>
		SellerOrderNumber,
		///<summary>InvoiceNumber. </summary>
		InvoiceNumber,
		///<summary>ParcelID. </summary>
		ParcelID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: MarketplaceAdvisorStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum MarketplaceAdvisorStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>Username. </summary>
		Username,
		///<summary>Password. </summary>
		Password,
		///<summary>AccountType. </summary>
		AccountType,
		///<summary>DownloadFlags. </summary>
		DownloadFlags,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: MivaOrderItemAttribute.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum MivaOrderItemAttributeFieldIndex:int
	{
		///<summary>OrderItemAttributeID. Inherited from OrderItemAttribute</summary>
		OrderItemAttributeID_OrderItemAttribute,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>Name. </summary>
		Name,
		///<summary>Description. </summary>
		Description,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemAttributeID. </summary>
		OrderItemAttributeID,
		///<summary>MivaOptionCode. </summary>
		MivaOptionCode,
		///<summary>MivaAttributeID. </summary>
		MivaAttributeID,
		///<summary>MivaAttributeCode. </summary>
		MivaAttributeCode,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: MivaStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum MivaStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. Inherited from GenericModuleStore</summary>
		StoreID_GenericModuleStore,
		///<summary>ModuleUsername. </summary>
		ModuleUsername,
		///<summary>ModulePassword. </summary>
		ModulePassword,
		///<summary>ModuleUrl. </summary>
		ModuleUrl,
		///<summary>ModuleVersion. </summary>
		ModuleVersion,
		///<summary>ModulePlatform. </summary>
		ModulePlatform,
		///<summary>ModuleDeveloper. </summary>
		ModuleDeveloper,
		///<summary>ModuleOnlineStoreCode. </summary>
		ModuleOnlineStoreCode,
		///<summary>ModuleStatusCodes. </summary>
		ModuleStatusCodes,
		///<summary>ModuleDownloadPageSize. </summary>
		ModuleDownloadPageSize,
		///<summary>ModuleRequestTimeout. </summary>
		ModuleRequestTimeout,
		///<summary>ModuleDownloadStrategy. </summary>
		ModuleDownloadStrategy,
		///<summary>ModuleOnlineStatusSupport. </summary>
		ModuleOnlineStatusSupport,
		///<summary>ModuleOnlineStatusDataType. </summary>
		ModuleOnlineStatusDataType,
		///<summary>ModuleOnlineCustomerSupport. </summary>
		ModuleOnlineCustomerSupport,
		///<summary>ModuleOnlineCustomerDataType. </summary>
		ModuleOnlineCustomerDataType,
		///<summary>ModuleOnlineShipmentDetails. </summary>
		ModuleOnlineShipmentDetails,
		///<summary>ModuleHttpExpect100Continue. </summary>
		ModuleHttpExpect100Continue,
		///<summary>ModuleResponseEncoding. </summary>
		ModuleResponseEncoding,
		///<summary>SchemaVersion. </summary>
		SchemaVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>EncryptionPassphrase. </summary>
		EncryptionPassphrase,
		///<summary>LiveManualOrderNumbers. </summary>
		LiveManualOrderNumbers,
		///<summary>SebenzaCheckoutDataEnabled. </summary>
		SebenzaCheckoutDataEnabled,
		///<summary>OnlineUpdateStrategy. </summary>
		OnlineUpdateStrategy,
		///<summary>OnlineUpdateStatusChangeEmail. </summary>
		OnlineUpdateStatusChangeEmail,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: NetworkSolutionsOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum NetworkSolutionsOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>NetworkSolutionsOrderID. </summary>
		NetworkSolutionsOrderID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: NetworkSolutionsStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum NetworkSolutionsStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>UserToken. </summary>
		UserToken,
		///<summary>DownloadOrderStatuses. </summary>
		DownloadOrderStatuses,
		///<summary>StatusCodes. </summary>
		StatusCodes,
		///<summary>StoreUrl. </summary>
		StoreUrl,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: NeweggOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum NeweggOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>InvoiceNumber. </summary>
		InvoiceNumber,
		///<summary>RefundAmount. </summary>
		RefundAmount,
		///<summary>IsAutoVoid. </summary>
		IsAutoVoid,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: NeweggOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum NeweggOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>SellerPartNumber. </summary>
		SellerPartNumber,
		///<summary>NeweggItemNumber. </summary>
		NeweggItemNumber,
		///<summary>ManufacturerPartNumber. </summary>
		ManufacturerPartNumber,
		///<summary>ShippingStatusID. </summary>
		ShippingStatusID,
		///<summary>ShippingStatusDescription. </summary>
		ShippingStatusDescription,
		///<summary>QuantityShipped. </summary>
		QuantityShipped,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: NeweggStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum NeweggStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>SellerID. </summary>
		SellerID,
		///<summary>SecretKey. </summary>
		SecretKey,
		///<summary>ExcludeFulfilledByNewegg. </summary>
		ExcludeFulfilledByNewegg,
		///<summary>Channel. </summary>
		Channel,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Note.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum NoteFieldIndex:int
	{
		///<summary>NoteID. </summary>
		NoteID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ObjectID. </summary>
		ObjectID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>Edited. </summary>
		Edited,
		///<summary>Text. </summary>
		Text,
		///<summary>Source. </summary>
		Source,
		///<summary>Visibility. </summary>
		Visibility,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ObjectLabel.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ObjectLabelFieldIndex:int
	{
		///<summary>ObjectID. </summary>
		ObjectID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ObjectType. </summary>
		ObjectType,
		///<summary>ParentID. </summary>
		ParentID,
		///<summary>Label. </summary>
		Label,
		///<summary>IsDeleted. </summary>
		IsDeleted,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ObjectReference.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ObjectReferenceFieldIndex:int
	{
		///<summary>ObjectReferenceID. </summary>
		ObjectReferenceID,
		///<summary>ConsumerID. </summary>
		ConsumerID,
		///<summary>ReferenceKey. </summary>
		ReferenceKey,
		///<summary>ObjectID. </summary>
		ObjectID,
		///<summary>Reason. </summary>
		Reason,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OnTracAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OnTracAccountFieldIndex:int
	{
		///<summary>OnTracAccountID. </summary>
		OnTracAccountID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>AccountNumber. </summary>
		AccountNumber,
		///<summary>Password. </summary>
		Password,
		///<summary>Description. </summary>
		Description,
		///<summary>FirstName. </summary>
		FirstName,
		///<summary>MiddleName. </summary>
		MiddleName,
		///<summary>LastName. </summary>
		LastName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Email. </summary>
		Email,
		///<summary>Phone. </summary>
		Phone,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OnTracProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OnTracProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>OnTracAccountID. </summary>
		OnTracAccountID,
		///<summary>ResidentialDetermination. </summary>
		ResidentialDetermination,
		///<summary>Service. </summary>
		Service,
		///<summary>SaturdayDelivery. </summary>
		SaturdayDelivery,
		///<summary>SignatureRequired. </summary>
		SignatureRequired,
		///<summary>PackagingType. </summary>
		PackagingType,
		///<summary>Weight. </summary>
		Weight,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>Reference1. </summary>
		Reference1,
		///<summary>Reference2. </summary>
		Reference2,
		///<summary>Instructions. </summary>
		Instructions,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OnTracShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OnTracShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>OnTracAccountID. </summary>
		OnTracAccountID,
		///<summary>Service. </summary>
		Service,
		///<summary>IsCod. </summary>
		IsCod,
		///<summary>CodType. </summary>
		CodType,
		///<summary>CodAmount. </summary>
		CodAmount,
		///<summary>SaturdayDelivery. </summary>
		SaturdayDelivery,
		///<summary>SignatureRequired. </summary>
		SignatureRequired,
		///<summary>PackagingType. </summary>
		PackagingType,
		///<summary>Instructions. </summary>
		Instructions,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>Reference1. </summary>
		Reference1,
		///<summary>Reference2. </summary>
		Reference2,
		///<summary>InsuranceValue. </summary>
		InsuranceValue,
		///<summary>InsurancePennyOne. </summary>
		InsurancePennyOne,
		///<summary>DeclaredValue. </summary>
		DeclaredValue,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Order.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OrderFieldIndex:int
	{
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OrderCharge.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OrderChargeFieldIndex:int
	{
		///<summary>OrderChargeID. </summary>
		OrderChargeID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Type. </summary>
		Type,
		///<summary>Description. </summary>
		Description,
		///<summary>Amount. </summary>
		Amount,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OrderItemFieldIndex:int
	{
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OrderItemAttribute.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OrderItemAttributeFieldIndex:int
	{
		///<summary>OrderItemAttributeID. </summary>
		OrderItemAttributeID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>Name. </summary>
		Name,
		///<summary>Description. </summary>
		Description,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>IsManual. </summary>
		IsManual,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OrderMotionOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OrderMotionOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>OrderMotionShipmentID. </summary>
		OrderMotionShipmentID,
		///<summary>OrderMotionPromotion. </summary>
		OrderMotionPromotion,
		///<summary>OrderMotionInvoiceNumber. </summary>
		OrderMotionInvoiceNumber,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OrderMotionStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OrderMotionStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>OrderMotionEmailAccountID. </summary>
		OrderMotionEmailAccountID,
		///<summary>OrderMotionBizID. </summary>
		OrderMotionBizID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OrderPaymentDetail.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OrderPaymentDetailFieldIndex:int
	{
		///<summary>OrderPaymentDetailID. </summary>
		OrderPaymentDetailID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Label. </summary>
		Label,
		///<summary>Value. </summary>
		Value,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OtherProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OtherProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>Carrier. </summary>
		Carrier,
		///<summary>Service. </summary>
		Service,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: OtherShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum OtherShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>Carrier. </summary>
		Carrier,
		///<summary>Service. </summary>
		Service,
		///<summary>InsuranceValue. </summary>
		InsuranceValue,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: PayPalOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum PayPalOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>TransactionID. </summary>
		TransactionID,
		///<summary>AddressStatus. </summary>
		AddressStatus,
		///<summary>PayPalFee. </summary>
		PayPalFee,
		///<summary>PaymentStatus. </summary>
		PaymentStatus,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: PayPalStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum PayPalStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>ApiCredentialType. </summary>
		ApiCredentialType,
		///<summary>ApiUserName. </summary>
		ApiUserName,
		///<summary>ApiPassword. </summary>
		ApiPassword,
		///<summary>ApiSignature. </summary>
		ApiSignature,
		///<summary>ApiCertificate. </summary>
		ApiCertificate,
		///<summary>LastTransactionDate. </summary>
		LastTransactionDate,
		///<summary>LastValidTransactionDate. </summary>
		LastValidTransactionDate,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Permission.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum PermissionFieldIndex:int
	{
		///<summary>PermissionID. </summary>
		PermissionID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>PermissionType. </summary>
		PermissionType,
		///<summary>ObjectID. </summary>
		ObjectID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: PostalProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum PostalProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>Service. </summary>
		Service,
		///<summary>Confirmation. </summary>
		Confirmation,
		///<summary>Weight. </summary>
		Weight,
		///<summary>PackagingType. </summary>
		PackagingType,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>NonRectangular. </summary>
		NonRectangular,
		///<summary>NonMachinable. </summary>
		NonMachinable,
		///<summary>CustomsContentType. </summary>
		CustomsContentType,
		///<summary>CustomsContentDescription. </summary>
		CustomsContentDescription,
		///<summary>ExpressSignatureWaiver. </summary>
		ExpressSignatureWaiver,
		///<summary>SortType. </summary>
		SortType,
		///<summary>EntryFacility. </summary>
		EntryFacility,
		///<summary>Memo1. </summary>
		Memo1,
		///<summary>Memo2. </summary>
		Memo2,
		///<summary>Memo3. </summary>
		Memo3,
		///<summary>NoPostage. </summary>
		NoPostage,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: PostalShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum PostalShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>Service. </summary>
		Service,
		///<summary>Confirmation. </summary>
		Confirmation,
		///<summary>PackagingType. </summary>
		PackagingType,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>NonRectangular. </summary>
		NonRectangular,
		///<summary>NonMachinable. </summary>
		NonMachinable,
		///<summary>CustomsContentType. </summary>
		CustomsContentType,
		///<summary>CustomsContentDescription. </summary>
		CustomsContentDescription,
		///<summary>InsuranceValue. </summary>
		InsuranceValue,
		///<summary>ExpressSignatureWaiver. </summary>
		ExpressSignatureWaiver,
		///<summary>SortType. </summary>
		SortType,
		///<summary>EntryFacility. </summary>
		EntryFacility,
		///<summary>Memo1. </summary>
		Memo1,
		///<summary>Memo2. </summary>
		Memo2,
		///<summary>Memo3. </summary>
		Memo3,
		///<summary>NoPostage. </summary>
		NoPostage,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: PrintResult.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum PrintResultFieldIndex:int
	{
		///<summary>PrintResultID. </summary>
		PrintResultID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>JobIdentifier. </summary>
		JobIdentifier,
		///<summary>RelatedObjectID. </summary>
		RelatedObjectID,
		///<summary>ContextObjectID. </summary>
		ContextObjectID,
		///<summary>TemplateID. </summary>
		TemplateID,
		///<summary>TemplateType. </summary>
		TemplateType,
		///<summary>OutputFormat. </summary>
		OutputFormat,
		///<summary>LabelSheetID. </summary>
		LabelSheetID,
		///<summary>ComputerID. </summary>
		ComputerID,
		///<summary>ContentResourceID. </summary>
		ContentResourceID,
		///<summary>PrintDate. </summary>
		PrintDate,
		///<summary>PrinterName. </summary>
		PrinterName,
		///<summary>PaperSource. </summary>
		PaperSource,
		///<summary>PaperSourceName. </summary>
		PaperSourceName,
		///<summary>Copies. </summary>
		Copies,
		///<summary>Collated. </summary>
		Collated,
		///<summary>PageMarginLeft. </summary>
		PageMarginLeft,
		///<summary>PageMarginRight. </summary>
		PageMarginRight,
		///<summary>PageMarginBottom. </summary>
		PageMarginBottom,
		///<summary>PageMarginTop. </summary>
		PageMarginTop,
		///<summary>PageWidth. </summary>
		PageWidth,
		///<summary>PageHeight. </summary>
		PageHeight,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ProStoresOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ProStoresOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>ConfirmationNumber. </summary>
		ConfirmationNumber,
		///<summary>AuthorizedDate. </summary>
		AuthorizedDate,
		///<summary>AuthorizedBy. </summary>
		AuthorizedBy,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ProStoresStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ProStoresStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>ShortName. </summary>
		ShortName,
		///<summary>Username. </summary>
		Username,
		///<summary>LoginMethod. </summary>
		LoginMethod,
		///<summary>ApiEntryPoint. </summary>
		ApiEntryPoint,
		///<summary>ApiToken. </summary>
		ApiToken,
		///<summary>ApiStorefrontUrl. </summary>
		ApiStorefrontUrl,
		///<summary>ApiTokenLogonUrl. </summary>
		ApiTokenLogonUrl,
		///<summary>ApiXteUrl. </summary>
		ApiXteUrl,
		///<summary>ApiRestSecureUrl. </summary>
		ApiRestSecureUrl,
		///<summary>ApiRestNonSecureUrl. </summary>
		ApiRestNonSecureUrl,
		///<summary>ApiRestScriptSuffix. </summary>
		ApiRestScriptSuffix,
		///<summary>LegacyAdminUrl. </summary>
		LegacyAdminUrl,
		///<summary>LegacyXtePath. </summary>
		LegacyXtePath,
		///<summary>LegacyPrefix. </summary>
		LegacyPrefix,
		///<summary>LegacyPassword. </summary>
		LegacyPassword,
		///<summary>LegacyCanUpgrade. </summary>
		LegacyCanUpgrade,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Resource.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ResourceFieldIndex:int
	{
		///<summary>ResourceID. </summary>
		ResourceID,
		///<summary>Data. </summary>
		Data,
		///<summary>Checksum. </summary>
		Checksum,
		///<summary>Compressed. </summary>
		Compressed,
		///<summary>Filename. </summary>
		Filename,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ScanFormBatch.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ScanFormBatchFieldIndex:int
	{
		///<summary>ScanFormBatchID. </summary>
		ScanFormBatchID,
		///<summary>ShipmentType. </summary>
		ShipmentType,
		///<summary>CreatedDate. </summary>
		CreatedDate,
		///<summary>ShipmentCount. </summary>
		ShipmentCount,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Search.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum SearchFieldIndex:int
	{
		///<summary>SearchID. </summary>
		SearchID,
		///<summary>Started. </summary>
		Started,
		///<summary>Pinged. </summary>
		Pinged,
		///<summary>FilterNodeID. </summary>
		FilterNodeID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>ComputerID. </summary>
		ComputerID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: SearsOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum SearsOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>PoNumber. </summary>
		PoNumber,
		///<summary>PoNumberWithDate. </summary>
		PoNumberWithDate,
		///<summary>LocationID. </summary>
		LocationID,
		///<summary>Commission. </summary>
		Commission,
		///<summary>CustomerPickup. </summary>
		CustomerPickup,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: SearsOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum SearsOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>LineNumber. </summary>
		LineNumber,
		///<summary>ItemID. </summary>
		ItemID,
		///<summary>Commission. </summary>
		Commission,
		///<summary>Shipping. </summary>
		Shipping,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: SearsStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum SearsStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. Inherited from Store</summary>
		Email_Store,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>Email. </summary>
		Email,
		///<summary>Password. </summary>
		Password,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ServerMessage.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ServerMessageFieldIndex:int
	{
		///<summary>ServerMessageID. </summary>
		ServerMessageID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Number. </summary>
		Number,
		///<summary>Published. </summary>
		Published,
		///<summary>Active. </summary>
		Active,
		///<summary>Dismissable. </summary>
		Dismissable,
		///<summary>Expires. </summary>
		Expires,
		///<summary>ResponseTo. </summary>
		ResponseTo,
		///<summary>ResponseAction. </summary>
		ResponseAction,
		///<summary>EditTo. </summary>
		EditTo,
		///<summary>Image. </summary>
		Image,
		///<summary>PrimaryText. </summary>
		PrimaryText,
		///<summary>SecondaryText. </summary>
		SecondaryText,
		///<summary>Actions. </summary>
		Actions,
		///<summary>Stores. </summary>
		Stores,
		///<summary>Shippers. </summary>
		Shippers,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ServerMessageSignoff.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ServerMessageSignoffFieldIndex:int
	{
		///<summary>ServerMessageSignoffID. </summary>
		ServerMessageSignoffID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ServerMessageID. </summary>
		ServerMessageID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>ComputerID. </summary>
		ComputerID,
		///<summary>Dismissed. </summary>
		Dismissed,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ServiceStatus.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ServiceStatusFieldIndex:int
	{
		///<summary>ServiceStatusID. </summary>
		ServiceStatusID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ComputerID. </summary>
		ComputerID,
		///<summary>ServiceType. </summary>
		ServiceType,
		///<summary>LastStartDateTime. </summary>
		LastStartDateTime,
		///<summary>LastStopDateTime. </summary>
		LastStopDateTime,
		///<summary>LastCheckInDateTime. </summary>
		LastCheckInDateTime,
		///<summary>ServiceFullName. </summary>
		ServiceFullName,
		///<summary>ServiceDisplayName. </summary>
		ServiceDisplayName,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Shipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>ShipmentType. </summary>
		ShipmentType,
		///<summary>ContentWeight. </summary>
		ContentWeight,
		///<summary>TotalWeight. </summary>
		TotalWeight,
		///<summary>Processed. </summary>
		Processed,
		///<summary>ProcessedDate. </summary>
		ProcessedDate,
		///<summary>ProcessedUserID. </summary>
		ProcessedUserID,
		///<summary>ProcessedComputerID. </summary>
		ProcessedComputerID,
		///<summary>ShipDate. </summary>
		ShipDate,
		///<summary>ShipmentCost. </summary>
		ShipmentCost,
		///<summary>Voided. </summary>
		Voided,
		///<summary>VoidedDate. </summary>
		VoidedDate,
		///<summary>VoidedUserID. </summary>
		VoidedUserID,
		///<summary>VoidedComputerID. </summary>
		VoidedComputerID,
		///<summary>TrackingNumber. </summary>
		TrackingNumber,
		///<summary>CustomsGenerated. </summary>
		CustomsGenerated,
		///<summary>CustomsValue. </summary>
		CustomsValue,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		///<summary>ActualLabelFormat. </summary>
		ActualLabelFormat,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>ResidentialDetermination. </summary>
		ResidentialDetermination,
		///<summary>ResidentialResult. </summary>
		ResidentialResult,
		///<summary>OriginOriginID. </summary>
		OriginOriginID,
		///<summary>OriginFirstName. </summary>
		OriginFirstName,
		///<summary>OriginMiddleName. </summary>
		OriginMiddleName,
		///<summary>OriginLastName. </summary>
		OriginLastName,
		///<summary>OriginCompany. </summary>
		OriginCompany,
		///<summary>OriginStreet1. </summary>
		OriginStreet1,
		///<summary>OriginStreet2. </summary>
		OriginStreet2,
		///<summary>OriginStreet3. </summary>
		OriginStreet3,
		///<summary>OriginCity. </summary>
		OriginCity,
		///<summary>OriginStateProvCode. </summary>
		OriginStateProvCode,
		///<summary>OriginPostalCode. </summary>
		OriginPostalCode,
		///<summary>OriginCountryCode. </summary>
		OriginCountryCode,
		///<summary>OriginPhone. </summary>
		OriginPhone,
		///<summary>OriginFax. </summary>
		OriginFax,
		///<summary>OriginEmail. </summary>
		OriginEmail,
		///<summary>OriginWebsite. </summary>
		OriginWebsite,
		///<summary>ReturnShipment. </summary>
		ReturnShipment,
		///<summary>Insurance. </summary>
		Insurance,
		///<summary>InsuranceProvider. </summary>
		InsuranceProvider,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>OriginNameParseStatus. </summary>
		OriginNameParseStatus,
		///<summary>OriginUnparsedName. </summary>
		OriginUnparsedName,
		///<summary>BestRateEvents. </summary>
		BestRateEvents,
		///<summary>ShipSenseStatus. </summary>
		ShipSenseStatus,
		///<summary>ShipSenseChangeSets. </summary>
		ShipSenseChangeSets,
		///<summary>ShipSenseEntry. </summary>
		ShipSenseEntry,
		///<summary>OnlineShipmentID. </summary>
		OnlineShipmentID,
		///<summary>BilledType. </summary>
		BilledType,
		///<summary>BilledWeight. </summary>
		BilledWeight,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShipmentCustomsItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShipmentCustomsItemFieldIndex:int
	{
		///<summary>ShipmentCustomsItemID. </summary>
		ShipmentCustomsItemID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>Description. </summary>
		Description,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>Weight. </summary>
		Weight,
		///<summary>UnitValue. </summary>
		UnitValue,
		///<summary>CountryOfOrigin. </summary>
		CountryOfOrigin,
		///<summary>HarmonizedCode. </summary>
		HarmonizedCode,
		///<summary>NumberOfPieces. </summary>
		NumberOfPieces,
		///<summary>UnitPriceAmount. </summary>
		UnitPriceAmount,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShippingDefaultsRule.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShippingDefaultsRuleFieldIndex:int
	{
		///<summary>ShippingDefaultsRuleID. </summary>
		ShippingDefaultsRuleID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ShipmentType. </summary>
		ShipmentType,
		///<summary>FilterNodeID. </summary>
		FilterNodeID,
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>Position. </summary>
		Position,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShippingOrigin.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShippingOriginFieldIndex:int
	{
		///<summary>ShippingOriginID. </summary>
		ShippingOriginID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Description. </summary>
		Description,
		///<summary>FirstName. </summary>
		FirstName,
		///<summary>MiddleName. </summary>
		MiddleName,
		///<summary>LastName. </summary>
		LastName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShippingPrintOutput.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShippingPrintOutputFieldIndex:int
	{
		///<summary>ShippingPrintOutputID. </summary>
		ShippingPrintOutputID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ShipmentType. </summary>
		ShipmentType,
		///<summary>Name. </summary>
		Name,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShippingPrintOutputRule.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShippingPrintOutputRuleFieldIndex:int
	{
		///<summary>ShippingPrintOutputRuleID. </summary>
		ShippingPrintOutputRuleID,
		///<summary>ShippingPrintOutputID. </summary>
		ShippingPrintOutputID,
		///<summary>FilterNodeID. </summary>
		FilterNodeID,
		///<summary>TemplateID. </summary>
		TemplateID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShippingProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShippingProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Name. </summary>
		Name,
		///<summary>ShipmentType. </summary>
		ShipmentType,
		///<summary>ShipmentTypePrimary. </summary>
		ShipmentTypePrimary,
		///<summary>OriginID. </summary>
		OriginID,
		///<summary>Insurance. </summary>
		Insurance,
		///<summary>InsuranceInitialValueSource. </summary>
		InsuranceInitialValueSource,
		///<summary>InsuranceInitialValueAmount. </summary>
		InsuranceInitialValueAmount,
		///<summary>ReturnShipment. </summary>
		ReturnShipment,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShippingProviderRule.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShippingProviderRuleFieldIndex:int
	{
		///<summary>ShippingProviderRuleID. </summary>
		ShippingProviderRuleID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>FilterNodeID. </summary>
		FilterNodeID,
		///<summary>ShipmentType. </summary>
		ShipmentType,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShippingSettings.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShippingSettingsFieldIndex:int
	{
		///<summary>ShippingSettingsID. </summary>
		ShippingSettingsID,
		///<summary>InternalActivated. </summary>
		InternalActivated,
		///<summary>InternalConfigured. </summary>
		InternalConfigured,
		///<summary>InternalExcluded. </summary>
		InternalExcluded,
		///<summary>DefaultType. </summary>
		DefaultType,
		///<summary>BlankPhoneOption. </summary>
		BlankPhoneOption,
		///<summary>BlankPhoneNumber. </summary>
		BlankPhoneNumber,
		///<summary>InsurancePolicy. </summary>
		InsurancePolicy,
		///<summary>InsuranceLastAgreed. </summary>
		InsuranceLastAgreed,
		///<summary>FedExUsername. </summary>
		FedExUsername,
		///<summary>FedExPassword. </summary>
		FedExPassword,
		///<summary>FedExMaskAccount. </summary>
		FedExMaskAccount,
		///<summary>FedExThermalDocTab. </summary>
		FedExThermalDocTab,
		///<summary>FedExThermalDocTabType. </summary>
		FedExThermalDocTabType,
		///<summary>FedExInsuranceProvider. </summary>
		FedExInsuranceProvider,
		///<summary>FedExInsurancePennyOne. </summary>
		FedExInsurancePennyOne,
		///<summary>UpsAccessKey. </summary>
		UpsAccessKey,
		///<summary>UpsInsuranceProvider. </summary>
		UpsInsuranceProvider,
		///<summary>UpsInsurancePennyOne. </summary>
		UpsInsurancePennyOne,
		///<summary>EndiciaCustomsCertify. </summary>
		EndiciaCustomsCertify,
		///<summary>EndiciaCustomsSigner. </summary>
		EndiciaCustomsSigner,
		///<summary>EndiciaThermalDocTab. </summary>
		EndiciaThermalDocTab,
		///<summary>EndiciaThermalDocTabType. </summary>
		EndiciaThermalDocTabType,
		///<summary>EndiciaAutomaticExpress1. </summary>
		EndiciaAutomaticExpress1,
		///<summary>EndiciaAutomaticExpress1Account. </summary>
		EndiciaAutomaticExpress1Account,
		///<summary>EndiciaInsuranceProvider. </summary>
		EndiciaInsuranceProvider,
		///<summary>WorldShipLaunch. </summary>
		WorldShipLaunch,
		///<summary>UspsAutomaticExpress1. </summary>
		UspsAutomaticExpress1,
		///<summary>UspsAutomaticExpress1Account. </summary>
		UspsAutomaticExpress1Account,
		///<summary>UspsInsuranceProvider. </summary>
		UspsInsuranceProvider,
		///<summary>Express1EndiciaCustomsCertify. </summary>
		Express1EndiciaCustomsCertify,
		///<summary>Express1EndiciaCustomsSigner. </summary>
		Express1EndiciaCustomsSigner,
		///<summary>Express1EndiciaThermalDocTab. </summary>
		Express1EndiciaThermalDocTab,
		///<summary>Express1EndiciaThermalDocTabType. </summary>
		Express1EndiciaThermalDocTabType,
		///<summary>Express1EndiciaSingleSource. </summary>
		Express1EndiciaSingleSource,
		///<summary>OnTracInsuranceProvider. </summary>
		OnTracInsuranceProvider,
		///<summary>OnTracInsurancePennyOne. </summary>
		OnTracInsurancePennyOne,
		///<summary>IParcelInsuranceProvider. </summary>
		IParcelInsuranceProvider,
		///<summary>IParcelInsurancePennyOne. </summary>
		IParcelInsurancePennyOne,
		///<summary>Express1UspsSingleSource. </summary>
		Express1UspsSingleSource,
		///<summary>UpsMailInnovationsEnabled. </summary>
		UpsMailInnovationsEnabled,
		///<summary>WorldShipMailInnovationsEnabled. </summary>
		WorldShipMailInnovationsEnabled,
		///<summary>InternalBestRateExcludedShipmentTypes. </summary>
		InternalBestRateExcludedShipmentTypes,
		///<summary>ShipSenseEnabled. </summary>
		ShipSenseEnabled,
		///<summary>ShipSenseUniquenessXml. </summary>
		ShipSenseUniquenessXml,
		///<summary>ShipSenseProcessedShipmentID. </summary>
		ShipSenseProcessedShipmentID,
		///<summary>ShipSenseEndShipmentID. </summary>
		ShipSenseEndShipmentID,
		///<summary>AutoCreateShipments. </summary>
		AutoCreateShipments,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShipSenseKnowledgebase.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShipSenseKnowledgebaseFieldIndex:int
	{
		///<summary>Hash. </summary>
		Hash,
		///<summary>Entry. </summary>
		Entry,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShopifyOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShopifyOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>ShopifyOrderID. </summary>
		ShopifyOrderID,
		///<summary>FulfillmentStatusCode. </summary>
		FulfillmentStatusCode,
		///<summary>PaymentStatusCode. </summary>
		PaymentStatusCode,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShopifyOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShopifyOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>ShopifyOrderItemID. </summary>
		ShopifyOrderItemID,
		///<summary>ShopifyProductID. </summary>
		ShopifyProductID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShopifyStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShopifyStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>ShopifyShopUrlName. </summary>
		ShopifyShopUrlName,
		///<summary>ShopifyShopDisplayName. </summary>
		ShopifyShopDisplayName,
		///<summary>ShopifyAccessToken. </summary>
		ShopifyAccessToken,
		///<summary>ShopifyRequestedShippingOption. </summary>
		ShopifyRequestedShippingOption,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ShopSiteStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ShopSiteStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>Username. </summary>
		Username,
		///<summary>Password. </summary>
		Password,
		///<summary>CgiUrl. </summary>
		CgiUrl,
		///<summary>RequireSSL. </summary>
		RequireSSL,
		///<summary>DownloadPageSize. </summary>
		DownloadPageSize,
		///<summary>RequestTimeout. </summary>
		RequestTimeout,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: StatusPreset.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum StatusPresetFieldIndex:int
	{
		///<summary>StatusPresetID. </summary>
		StatusPresetID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>StatusTarget. </summary>
		StatusTarget,
		///<summary>StatusText. </summary>
		StatusText,
		///<summary>IsDefault. </summary>
		IsDefault,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Store.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum StoreFieldIndex:int
	{
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: SystemData.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum SystemDataFieldIndex:int
	{
		///<summary>SystemDataID. </summary>
		SystemDataID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>DatabaseID. </summary>
		DatabaseID,
		///<summary>DateFiltersLastUpdate. </summary>
		DateFiltersLastUpdate,
		///<summary>TemplateVersion. </summary>
		TemplateVersion,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Template.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum TemplateFieldIndex:int
	{
		///<summary>TemplateID. </summary>
		TemplateID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ParentFolderID. </summary>
		ParentFolderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Xsl. </summary>
		Xsl,
		///<summary>Type. </summary>
		Type,
		///<summary>Context. </summary>
		Context,
		///<summary>OutputFormat. </summary>
		OutputFormat,
		///<summary>OutputEncoding. </summary>
		OutputEncoding,
		///<summary>PageMarginLeft. </summary>
		PageMarginLeft,
		///<summary>PageMarginRight. </summary>
		PageMarginRight,
		///<summary>PageMarginBottom. </summary>
		PageMarginBottom,
		///<summary>PageMarginTop. </summary>
		PageMarginTop,
		///<summary>PageWidth. </summary>
		PageWidth,
		///<summary>PageHeight. </summary>
		PageHeight,
		///<summary>LabelSheetID. </summary>
		LabelSheetID,
		///<summary>PrintCopies. </summary>
		PrintCopies,
		///<summary>PrintCollate. </summary>
		PrintCollate,
		///<summary>SaveFileName. </summary>
		SaveFileName,
		///<summary>SaveFileFolder. </summary>
		SaveFileFolder,
		///<summary>SaveFilePrompt. </summary>
		SaveFilePrompt,
		///<summary>SaveFileBOM. </summary>
		SaveFileBOM,
		///<summary>SaveFileOnlineResources. </summary>
		SaveFileOnlineResources,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: TemplateComputerSettings.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum TemplateComputerSettingsFieldIndex:int
	{
		///<summary>TemplateComputerSettingsID. </summary>
		TemplateComputerSettingsID,
		///<summary>TemplateID. </summary>
		TemplateID,
		///<summary>ComputerID. </summary>
		ComputerID,
		///<summary>PrinterName. </summary>
		PrinterName,
		///<summary>PaperSource. </summary>
		PaperSource,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: TemplateFolder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum TemplateFolderFieldIndex:int
	{
		///<summary>TemplateFolderID. </summary>
		TemplateFolderID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>ParentFolderID. </summary>
		ParentFolderID,
		///<summary>Name. </summary>
		Name,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: TemplateStoreSettings.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum TemplateStoreSettingsFieldIndex:int
	{
		///<summary>TemplateStoreSettingsID. </summary>
		TemplateStoreSettingsID,
		///<summary>TemplateID. </summary>
		TemplateID,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>EmailUseDefault. </summary>
		EmailUseDefault,
		///<summary>EmailAccountID. </summary>
		EmailAccountID,
		///<summary>EmailTo. </summary>
		EmailTo,
		///<summary>EmailCc. </summary>
		EmailCc,
		///<summary>EmailBcc. </summary>
		EmailBcc,
		///<summary>EmailSubject. </summary>
		EmailSubject,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: TemplateUserSettings.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum TemplateUserSettingsFieldIndex:int
	{
		///<summary>TemplateUserSettingsID. </summary>
		TemplateUserSettingsID,
		///<summary>TemplateID. </summary>
		TemplateID,
		///<summary>UserID. </summary>
		UserID,
		///<summary>PreviewSource. </summary>
		PreviewSource,
		///<summary>PreviewCount. </summary>
		PreviewCount,
		///<summary>PreviewFilterNodeID. </summary>
		PreviewFilterNodeID,
		///<summary>PreviewZoom. </summary>
		PreviewZoom,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ThreeDCartOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ThreeDCartOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>ThreeDCartShipmentID. </summary>
		ThreeDCartShipmentID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ThreeDCartStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ThreeDCartStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>StoreUrl. </summary>
		StoreUrl,
		///<summary>ApiUserKey. </summary>
		ApiUserKey,
		///<summary>TimeZoneID. </summary>
		TimeZoneID,
		///<summary>StatusCodes. </summary>
		StatusCodes,
		///<summary>DownloadModifiedNumberOfDaysBack. </summary>
		DownloadModifiedNumberOfDaysBack,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UpsAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UpsAccountFieldIndex:int
	{
		///<summary>UpsAccountID. </summary>
		UpsAccountID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Description. </summary>
		Description,
		///<summary>AccountNumber. </summary>
		AccountNumber,
		///<summary>UserID. </summary>
		UserID,
		///<summary>Password. </summary>
		Password,
		///<summary>RateType. </summary>
		RateType,
		///<summary>InvoiceAuth. </summary>
		InvoiceAuth,
		///<summary>FirstName. </summary>
		FirstName,
		///<summary>MiddleName. </summary>
		MiddleName,
		///<summary>LastName. </summary>
		LastName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UpsPackage.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UpsPackageFieldIndex:int
	{
		///<summary>UpsPackageID. </summary>
		UpsPackageID,
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>PackagingType. </summary>
		PackagingType,
		///<summary>Weight. </summary>
		Weight,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>Insurance. </summary>
		Insurance,
		///<summary>InsuranceValue. </summary>
		InsuranceValue,
		///<summary>InsurancePennyOne. </summary>
		InsurancePennyOne,
		///<summary>DeclaredValue. </summary>
		DeclaredValue,
		///<summary>TrackingNumber. </summary>
		TrackingNumber,
		///<summary>UspsTrackingNumber. </summary>
		UspsTrackingNumber,
		///<summary>AdditionalHandlingEnabled. </summary>
		AdditionalHandlingEnabled,
		///<summary>VerbalConfirmationEnabled. </summary>
		VerbalConfirmationEnabled,
		///<summary>VerbalConfirmationName. </summary>
		VerbalConfirmationName,
		///<summary>VerbalConfirmationPhone. </summary>
		VerbalConfirmationPhone,
		///<summary>VerbalConfirmationPhoneExtension. </summary>
		VerbalConfirmationPhoneExtension,
		///<summary>DryIceEnabled. </summary>
		DryIceEnabled,
		///<summary>DryIceRegulationSet. </summary>
		DryIceRegulationSet,
		///<summary>DryIceWeight. </summary>
		DryIceWeight,
		///<summary>DryIceIsForMedicalUse. </summary>
		DryIceIsForMedicalUse,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UpsProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UpsProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>UpsAccountID. </summary>
		UpsAccountID,
		///<summary>Service. </summary>
		Service,
		///<summary>SaturdayDelivery. </summary>
		SaturdayDelivery,
		///<summary>ResidentialDetermination. </summary>
		ResidentialDetermination,
		///<summary>DeliveryConfirmation. </summary>
		DeliveryConfirmation,
		///<summary>ReferenceNumber. </summary>
		ReferenceNumber,
		///<summary>ReferenceNumber2. </summary>
		ReferenceNumber2,
		///<summary>PayorType. </summary>
		PayorType,
		///<summary>PayorAccount. </summary>
		PayorAccount,
		///<summary>PayorPostalCode. </summary>
		PayorPostalCode,
		///<summary>PayorCountryCode. </summary>
		PayorCountryCode,
		///<summary>EmailNotifySender. </summary>
		EmailNotifySender,
		///<summary>EmailNotifyRecipient. </summary>
		EmailNotifyRecipient,
		///<summary>EmailNotifyOther. </summary>
		EmailNotifyOther,
		///<summary>EmailNotifyOtherAddress. </summary>
		EmailNotifyOtherAddress,
		///<summary>EmailNotifyFrom. </summary>
		EmailNotifyFrom,
		///<summary>EmailNotifySubject. </summary>
		EmailNotifySubject,
		///<summary>EmailNotifyMessage. </summary>
		EmailNotifyMessage,
		///<summary>ReturnService. </summary>
		ReturnService,
		///<summary>ReturnUndeliverableEmail. </summary>
		ReturnUndeliverableEmail,
		///<summary>ReturnContents. </summary>
		ReturnContents,
		///<summary>Endorsement. </summary>
		Endorsement,
		///<summary>Subclassification. </summary>
		Subclassification,
		///<summary>PaperlessAdditionalDocumentation. </summary>
		PaperlessAdditionalDocumentation,
		///<summary>ShipperRelease. </summary>
		ShipperRelease,
		///<summary>CarbonNeutral. </summary>
		CarbonNeutral,
		///<summary>CommercialPaperlessInvoice. </summary>
		CommercialPaperlessInvoice,
		///<summary>CostCenter. </summary>
		CostCenter,
		///<summary>IrregularIndicator. </summary>
		IrregularIndicator,
		///<summary>Cn22Number. </summary>
		Cn22Number,
		///<summary>ShipmentChargeType. </summary>
		ShipmentChargeType,
		///<summary>ShipmentChargeAccount. </summary>
		ShipmentChargeAccount,
		///<summary>ShipmentChargePostalCode. </summary>
		ShipmentChargePostalCode,
		///<summary>ShipmentChargeCountryCode. </summary>
		ShipmentChargeCountryCode,
		///<summary>UspsPackageID. </summary>
		UspsPackageID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UpsProfilePackage.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UpsProfilePackageFieldIndex:int
	{
		///<summary>UpsProfilePackageID. </summary>
		UpsProfilePackageID,
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>PackagingType. </summary>
		PackagingType,
		///<summary>Weight. </summary>
		Weight,
		///<summary>DimsProfileID. </summary>
		DimsProfileID,
		///<summary>DimsLength. </summary>
		DimsLength,
		///<summary>DimsWidth. </summary>
		DimsWidth,
		///<summary>DimsHeight. </summary>
		DimsHeight,
		///<summary>DimsWeight. </summary>
		DimsWeight,
		///<summary>DimsAddWeight. </summary>
		DimsAddWeight,
		///<summary>AdditionalHandlingEnabled. </summary>
		AdditionalHandlingEnabled,
		///<summary>VerbalConfirmationEnabled. </summary>
		VerbalConfirmationEnabled,
		///<summary>VerbalConfirmationName. </summary>
		VerbalConfirmationName,
		///<summary>VerbalConfirmationPhone. </summary>
		VerbalConfirmationPhone,
		///<summary>VerbalConfirmationPhoneExtension. </summary>
		VerbalConfirmationPhoneExtension,
		///<summary>DryIceEnabled. </summary>
		DryIceEnabled,
		///<summary>DryIceRegulationSet. </summary>
		DryIceRegulationSet,
		///<summary>DryIceWeight. </summary>
		DryIceWeight,
		///<summary>DryIceIsForMedicalUse. </summary>
		DryIceIsForMedicalUse,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UpsShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UpsShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>UpsAccountID. </summary>
		UpsAccountID,
		///<summary>Service. </summary>
		Service,
		///<summary>SaturdayDelivery. </summary>
		SaturdayDelivery,
		///<summary>CodEnabled. </summary>
		CodEnabled,
		///<summary>CodAmount. </summary>
		CodAmount,
		///<summary>CodPaymentType. </summary>
		CodPaymentType,
		///<summary>DeliveryConfirmation. </summary>
		DeliveryConfirmation,
		///<summary>ReferenceNumber. </summary>
		ReferenceNumber,
		///<summary>ReferenceNumber2. </summary>
		ReferenceNumber2,
		///<summary>PayorType. </summary>
		PayorType,
		///<summary>PayorAccount. </summary>
		PayorAccount,
		///<summary>PayorPostalCode. </summary>
		PayorPostalCode,
		///<summary>PayorCountryCode. </summary>
		PayorCountryCode,
		///<summary>EmailNotifySender. </summary>
		EmailNotifySender,
		///<summary>EmailNotifyRecipient. </summary>
		EmailNotifyRecipient,
		///<summary>EmailNotifyOther. </summary>
		EmailNotifyOther,
		///<summary>EmailNotifyOtherAddress. </summary>
		EmailNotifyOtherAddress,
		///<summary>EmailNotifyFrom. </summary>
		EmailNotifyFrom,
		///<summary>EmailNotifySubject. </summary>
		EmailNotifySubject,
		///<summary>EmailNotifyMessage. </summary>
		EmailNotifyMessage,
		///<summary>CustomsDocumentsOnly. </summary>
		CustomsDocumentsOnly,
		///<summary>CustomsDescription. </summary>
		CustomsDescription,
		///<summary>CommercialPaperlessInvoice. </summary>
		CommercialPaperlessInvoice,
		///<summary>CommercialInvoiceTermsOfSale. </summary>
		CommercialInvoiceTermsOfSale,
		///<summary>CommercialInvoicePurpose. </summary>
		CommercialInvoicePurpose,
		///<summary>CommercialInvoiceComments. </summary>
		CommercialInvoiceComments,
		///<summary>CommercialInvoiceFreight. </summary>
		CommercialInvoiceFreight,
		///<summary>CommercialInvoiceInsurance. </summary>
		CommercialInvoiceInsurance,
		///<summary>CommercialInvoiceOther. </summary>
		CommercialInvoiceOther,
		///<summary>WorldShipStatus. </summary>
		WorldShipStatus,
		///<summary>PublishedCharges. </summary>
		PublishedCharges,
		///<summary>NegotiatedRate. </summary>
		NegotiatedRate,
		///<summary>ReturnService. </summary>
		ReturnService,
		///<summary>ReturnUndeliverableEmail. </summary>
		ReturnUndeliverableEmail,
		///<summary>ReturnContents. </summary>
		ReturnContents,
		///<summary>UspsTrackingNumber. </summary>
		UspsTrackingNumber,
		///<summary>Endorsement. </summary>
		Endorsement,
		///<summary>Subclassification. </summary>
		Subclassification,
		///<summary>PaperlessAdditionalDocumentation. </summary>
		PaperlessAdditionalDocumentation,
		///<summary>ShipperRelease. </summary>
		ShipperRelease,
		///<summary>CarbonNeutral. </summary>
		CarbonNeutral,
		///<summary>CostCenter. </summary>
		CostCenter,
		///<summary>IrregularIndicator. </summary>
		IrregularIndicator,
		///<summary>Cn22Number. </summary>
		Cn22Number,
		///<summary>ShipmentChargeType. </summary>
		ShipmentChargeType,
		///<summary>ShipmentChargeAccount. </summary>
		ShipmentChargeAccount,
		///<summary>ShipmentChargePostalCode. </summary>
		ShipmentChargePostalCode,
		///<summary>ShipmentChargeCountryCode. </summary>
		ShipmentChargeCountryCode,
		///<summary>UspsPackageID. </summary>
		UspsPackageID,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: User.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UserFieldIndex:int
	{
		///<summary>UserID. </summary>
		UserID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Username. </summary>
		Username,
		///<summary>Password. </summary>
		Password,
		///<summary>Email. </summary>
		Email,
		///<summary>IsAdmin. </summary>
		IsAdmin,
		///<summary>IsDeleted. </summary>
		IsDeleted,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UserColumnSettings.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UserColumnSettingsFieldIndex:int
	{
		///<summary>UserColumnSettingsID. </summary>
		UserColumnSettingsID,
		///<summary>SettingsKey. </summary>
		SettingsKey,
		///<summary>UserID. </summary>
		UserID,
		///<summary>InitialSortType. </summary>
		InitialSortType,
		///<summary>GridColumnLayoutID. </summary>
		GridColumnLayoutID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UserSettings.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UserSettingsFieldIndex:int
	{
		///<summary>UserID. </summary>
		UserID,
		///<summary>DisplayColorScheme. </summary>
		DisplayColorScheme,
		///<summary>DisplaySystemTray. </summary>
		DisplaySystemTray,
		///<summary>WindowLayout. </summary>
		WindowLayout,
		///<summary>GridMenuLayout. </summary>
		GridMenuLayout,
		///<summary>FilterInitialUseLastActive. </summary>
		FilterInitialUseLastActive,
		///<summary>FilterInitialSpecified. </summary>
		FilterInitialSpecified,
		///<summary>FilterInitialSortType. </summary>
		FilterInitialSortType,
		///<summary>OrderFilterLastActive. </summary>
		OrderFilterLastActive,
		///<summary>OrderFilterExpandedFolders. </summary>
		OrderFilterExpandedFolders,
		///<summary>ShippingWeightFormat. </summary>
		ShippingWeightFormat,
		///<summary>TemplateExpandedFolders. </summary>
		TemplateExpandedFolders,
		///<summary>TemplateLastSelected. </summary>
		TemplateLastSelected,
		///<summary>CustomerFilterLastActive. </summary>
		CustomerFilterLastActive,
		///<summary>CustomerFilterExpandedFolders. </summary>
		CustomerFilterExpandedFolders,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UspsAccount.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UspsAccountFieldIndex:int
	{
		///<summary>UspsAccountID. </summary>
		UspsAccountID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>Description. </summary>
		Description,
		///<summary>Username. </summary>
		Username,
		///<summary>Password. </summary>
		Password,
		///<summary>FirstName. </summary>
		FirstName,
		///<summary>MiddleName. </summary>
		MiddleName,
		///<summary>LastName. </summary>
		LastName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>MailingPostalCode. </summary>
		MailingPostalCode,
		///<summary>UspsReseller. </summary>
		UspsReseller,
		///<summary>ContractType. </summary>
		ContractType,
		///<summary>CreatedDate. </summary>
		CreatedDate,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UspsProfile.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UspsProfileFieldIndex:int
	{
		///<summary>ShippingProfileID. </summary>
		ShippingProfileID,
		///<summary>UspsAccountID. </summary>
		UspsAccountID,
		///<summary>HidePostage. </summary>
		HidePostage,
		///<summary>RequireFullAddressValidation. </summary>
		RequireFullAddressValidation,
		///<summary>RateShop. </summary>
		RateShop,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UspsScanForm.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UspsScanFormFieldIndex:int
	{
		///<summary>UspsScanFormID. </summary>
		UspsScanFormID,
		///<summary>UspsAccountID. </summary>
		UspsAccountID,
		///<summary>ScanFormTransactionID. </summary>
		ScanFormTransactionID,
		///<summary>ScanFormUrl. </summary>
		ScanFormUrl,
		///<summary>CreatedDate. </summary>
		CreatedDate,
		///<summary>ScanFormBatchID. </summary>
		ScanFormBatchID,
		///<summary>Description. </summary>
		Description,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: UspsShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum UspsShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>UspsAccountID. </summary>
		UspsAccountID,
		///<summary>HidePostage. </summary>
		HidePostage,
		///<summary>RequireFullAddressValidation. </summary>
		RequireFullAddressValidation,
		///<summary>IntegratorTransactionID. </summary>
		IntegratorTransactionID,
		///<summary>UspsTransactionID. </summary>
		UspsTransactionID,
		///<summary>OriginalUspsAccountID. </summary>
		OriginalUspsAccountID,
		///<summary>ScanFormBatchID. </summary>
		ScanFormBatchID,
		///<summary>RequestedLabelFormat. </summary>
		RequestedLabelFormat,
		///<summary>RateShop. </summary>
		RateShop,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: ValidatedAddress.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum ValidatedAddressFieldIndex:int
	{
		///<summary>ValidatedAddressID. </summary>
		ValidatedAddressID,
		///<summary>ConsumerID. </summary>
		ConsumerID,
		///<summary>AddressPrefix. </summary>
		AddressPrefix,
		///<summary>IsOriginal. </summary>
		IsOriginal,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>ResidentialStatus. </summary>
		ResidentialStatus,
		///<summary>POBox. </summary>
		POBox,
		///<summary>USTerritory. </summary>
		USTerritory,
		///<summary>MilitaryAddress. </summary>
		MilitaryAddress,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: VersionSignoff.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum VersionSignoffFieldIndex:int
	{
		///<summary>VersionSignoffID. </summary>
		VersionSignoffID,
		///<summary>Version. </summary>
		Version,
		///<summary>UserID. </summary>
		UserID,
		///<summary>ComputerID. </summary>
		ComputerID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: VolusionStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum VolusionStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>StoreUrl. </summary>
		StoreUrl,
		///<summary>WebUserName. </summary>
		WebUserName,
		///<summary>WebPassword. </summary>
		WebPassword,
		///<summary>ApiPassword. </summary>
		ApiPassword,
		///<summary>PaymentMethods. </summary>
		PaymentMethods,
		///<summary>ShipmentMethods. </summary>
		ShipmentMethods,
		///<summary>DownloadOrderStatuses. </summary>
		DownloadOrderStatuses,
		///<summary>ServerTimeZone. </summary>
		ServerTimeZone,
		///<summary>ServerTimeZoneDST. </summary>
		ServerTimeZoneDST,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: WorldShipGoods.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum WorldShipGoodsFieldIndex:int
	{
		///<summary>WorldShipGoodsID. </summary>
		WorldShipGoodsID,
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>ShipmentCustomsItemID. </summary>
		ShipmentCustomsItemID,
		///<summary>Description. </summary>
		Description,
		///<summary>TariffCode. </summary>
		TariffCode,
		///<summary>CountryOfOrigin. </summary>
		CountryOfOrigin,
		///<summary>Units. </summary>
		Units,
		///<summary>UnitOfMeasure. </summary>
		UnitOfMeasure,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>Weight. </summary>
		Weight,
		///<summary>InvoiceCurrencyCode. </summary>
		InvoiceCurrencyCode,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: WorldShipPackage.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum WorldShipPackageFieldIndex:int
	{
		///<summary>UpsPackageID. </summary>
		UpsPackageID,
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>PackageType. </summary>
		PackageType,
		///<summary>Weight. </summary>
		Weight,
		///<summary>ReferenceNumber. </summary>
		ReferenceNumber,
		///<summary>ReferenceNumber2. </summary>
		ReferenceNumber2,
		///<summary>CodOption. </summary>
		CodOption,
		///<summary>CodAmount. </summary>
		CodAmount,
		///<summary>CodCashOnly. </summary>
		CodCashOnly,
		///<summary>DeliveryConfirmation. </summary>
		DeliveryConfirmation,
		///<summary>DeliveryConfirmationSignature. </summary>
		DeliveryConfirmationSignature,
		///<summary>DeliveryConfirmationAdult. </summary>
		DeliveryConfirmationAdult,
		///<summary>Length. </summary>
		Length,
		///<summary>Width. </summary>
		Width,
		///<summary>Height. </summary>
		Height,
		///<summary>DeclaredValueAmount. </summary>
		DeclaredValueAmount,
		///<summary>DeclaredValueOption. </summary>
		DeclaredValueOption,
		///<summary>CN22GoodsType. </summary>
		CN22GoodsType,
		///<summary>CN22Description. </summary>
		CN22Description,
		///<summary>PostalSubClass. </summary>
		PostalSubClass,
		///<summary>MIDeliveryConfirmation. </summary>
		MIDeliveryConfirmation,
		///<summary>QvnOption. </summary>
		QvnOption,
		///<summary>QvnFrom. </summary>
		QvnFrom,
		///<summary>QvnSubjectLine. </summary>
		QvnSubjectLine,
		///<summary>QvnMemo. </summary>
		QvnMemo,
		///<summary>Qvn1ShipNotify. </summary>
		Qvn1ShipNotify,
		///<summary>Qvn1ContactName. </summary>
		Qvn1ContactName,
		///<summary>Qvn1Email. </summary>
		Qvn1Email,
		///<summary>Qvn2ShipNotify. </summary>
		Qvn2ShipNotify,
		///<summary>Qvn2ContactName. </summary>
		Qvn2ContactName,
		///<summary>Qvn2Email. </summary>
		Qvn2Email,
		///<summary>Qvn3ShipNotify. </summary>
		Qvn3ShipNotify,
		///<summary>Qvn3ContactName. </summary>
		Qvn3ContactName,
		///<summary>Qvn3Email. </summary>
		Qvn3Email,
		///<summary>ShipperRelease. </summary>
		ShipperRelease,
		///<summary>AdditionalHandlingEnabled. </summary>
		AdditionalHandlingEnabled,
		///<summary>VerbalConfirmationOption. </summary>
		VerbalConfirmationOption,
		///<summary>VerbalConfirmationContactName. </summary>
		VerbalConfirmationContactName,
		///<summary>VerbalConfirmationTelephone. </summary>
		VerbalConfirmationTelephone,
		///<summary>DryIceRegulationSet. </summary>
		DryIceRegulationSet,
		///<summary>DryIceWeight. </summary>
		DryIceWeight,
		///<summary>DryIceMedicalPurpose. </summary>
		DryIceMedicalPurpose,
		///<summary>DryIceOption. </summary>
		DryIceOption,
		///<summary>DryIceWeightUnitOfMeasure. </summary>
		DryIceWeightUnitOfMeasure,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: WorldShipProcessed.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum WorldShipProcessedFieldIndex:int
	{
		///<summary>WorldShipProcessedID. </summary>
		WorldShipProcessedID,
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>PublishedCharges. </summary>
		PublishedCharges,
		///<summary>NegotiatedCharges. </summary>
		NegotiatedCharges,
		///<summary>TrackingNumber. </summary>
		TrackingNumber,
		///<summary>UspsTrackingNumber. </summary>
		UspsTrackingNumber,
		///<summary>ServiceType. </summary>
		ServiceType,
		///<summary>PackageType. </summary>
		PackageType,
		///<summary>UpsPackageID. </summary>
		UpsPackageID,
		///<summary>DeclaredValueAmount. </summary>
		DeclaredValueAmount,
		///<summary>DeclaredValueOption. </summary>
		DeclaredValueOption,
		///<summary>WorldShipShipmentID. </summary>
		WorldShipShipmentID,
		///<summary>VoidIndicator. </summary>
		VoidIndicator,
		///<summary>NumberOfPackages. </summary>
		NumberOfPackages,
		///<summary>LeadTrackingNumber. </summary>
		LeadTrackingNumber,
		///<summary>ShipmentIdCalculated. </summary>
		ShipmentIdCalculated,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: WorldShipShipment.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum WorldShipShipmentFieldIndex:int
	{
		///<summary>ShipmentID. </summary>
		ShipmentID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>FromCompanyOrName. </summary>
		FromCompanyOrName,
		///<summary>FromAttention. </summary>
		FromAttention,
		///<summary>FromAddress1. </summary>
		FromAddress1,
		///<summary>FromAddress2. </summary>
		FromAddress2,
		///<summary>FromAddress3. </summary>
		FromAddress3,
		///<summary>FromCountryCode. </summary>
		FromCountryCode,
		///<summary>FromPostalCode. </summary>
		FromPostalCode,
		///<summary>FromCity. </summary>
		FromCity,
		///<summary>FromStateProvCode. </summary>
		FromStateProvCode,
		///<summary>FromTelephone. </summary>
		FromTelephone,
		///<summary>FromEmail. </summary>
		FromEmail,
		///<summary>FromAccountNumber. </summary>
		FromAccountNumber,
		///<summary>ToCustomerID. </summary>
		ToCustomerID,
		///<summary>ToCompanyOrName. </summary>
		ToCompanyOrName,
		///<summary>ToAttention. </summary>
		ToAttention,
		///<summary>ToAddress1. </summary>
		ToAddress1,
		///<summary>ToAddress2. </summary>
		ToAddress2,
		///<summary>ToAddress3. </summary>
		ToAddress3,
		///<summary>ToCountryCode. </summary>
		ToCountryCode,
		///<summary>ToPostalCode. </summary>
		ToPostalCode,
		///<summary>ToCity. </summary>
		ToCity,
		///<summary>ToStateProvCode. </summary>
		ToStateProvCode,
		///<summary>ToTelephone. </summary>
		ToTelephone,
		///<summary>ToEmail. </summary>
		ToEmail,
		///<summary>ToAccountNumber. </summary>
		ToAccountNumber,
		///<summary>ToResidential. </summary>
		ToResidential,
		///<summary>ServiceType. </summary>
		ServiceType,
		///<summary>BillTransportationTo. </summary>
		BillTransportationTo,
		///<summary>SaturdayDelivery. </summary>
		SaturdayDelivery,
		///<summary>QvnOption. </summary>
		QvnOption,
		///<summary>QvnFrom. </summary>
		QvnFrom,
		///<summary>QvnSubjectLine. </summary>
		QvnSubjectLine,
		///<summary>QvnMemo. </summary>
		QvnMemo,
		///<summary>Qvn1ShipNotify. </summary>
		Qvn1ShipNotify,
		///<summary>Qvn1DeliveryNotify. </summary>
		Qvn1DeliveryNotify,
		///<summary>Qvn1ExceptionNotify. </summary>
		Qvn1ExceptionNotify,
		///<summary>Qvn1ContactName. </summary>
		Qvn1ContactName,
		///<summary>Qvn1Email. </summary>
		Qvn1Email,
		///<summary>Qvn2ShipNotify. </summary>
		Qvn2ShipNotify,
		///<summary>Qvn2DeliveryNotify. </summary>
		Qvn2DeliveryNotify,
		///<summary>Qvn2ExceptionNotify. </summary>
		Qvn2ExceptionNotify,
		///<summary>Qvn2ContactName. </summary>
		Qvn2ContactName,
		///<summary>Qvn2Email. </summary>
		Qvn2Email,
		///<summary>Qvn3ShipNotify. </summary>
		Qvn3ShipNotify,
		///<summary>Qvn3DeliveryNotify. </summary>
		Qvn3DeliveryNotify,
		///<summary>Qvn3ExceptionNotify. </summary>
		Qvn3ExceptionNotify,
		///<summary>Qvn3ContactName. </summary>
		Qvn3ContactName,
		///<summary>Qvn3Email. </summary>
		Qvn3Email,
		///<summary>CustomsDescriptionOfGoods. </summary>
		CustomsDescriptionOfGoods,
		///<summary>CustomsDocumentsOnly. </summary>
		CustomsDocumentsOnly,
		///<summary>ShipperNumber. </summary>
		ShipperNumber,
		///<summary>PackageCount. </summary>
		PackageCount,
		///<summary>DeliveryConfirmation. </summary>
		DeliveryConfirmation,
		///<summary>DeliveryConfirmationAdult. </summary>
		DeliveryConfirmationAdult,
		///<summary>InvoiceTermsOfSale. </summary>
		InvoiceTermsOfSale,
		///<summary>InvoiceReasonForExport. </summary>
		InvoiceReasonForExport,
		///<summary>InvoiceComments. </summary>
		InvoiceComments,
		///<summary>InvoiceCurrencyCode. </summary>
		InvoiceCurrencyCode,
		///<summary>InvoiceChargesFreight. </summary>
		InvoiceChargesFreight,
		///<summary>InvoiceChargesInsurance. </summary>
		InvoiceChargesInsurance,
		///<summary>InvoiceChargesOther. </summary>
		InvoiceChargesOther,
		///<summary>ShipmentProcessedOnComputerID. </summary>
		ShipmentProcessedOnComputerID,
		///<summary>UspsEndorsement. </summary>
		UspsEndorsement,
		///<summary>CarbonNeutral. </summary>
		CarbonNeutral,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: YahooOrder.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum YahooOrderFieldIndex:int
	{
		///<summary>OrderID. Inherited from Order</summary>
		OrderID_Order,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>CustomerID. </summary>
		CustomerID,
		///<summary>OrderNumber. </summary>
		OrderNumber,
		///<summary>OrderNumberComplete. </summary>
		OrderNumberComplete,
		///<summary>OrderDate. </summary>
		OrderDate,
		///<summary>OrderTotal. </summary>
		OrderTotal,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OnlineLastModified. </summary>
		OnlineLastModified,
		///<summary>OnlineCustomerID. </summary>
		OnlineCustomerID,
		///<summary>OnlineStatus. </summary>
		OnlineStatus,
		///<summary>OnlineStatusCode. </summary>
		OnlineStatusCode,
		///<summary>RequestedShipping. </summary>
		RequestedShipping,
		///<summary>BillFirstName. </summary>
		BillFirstName,
		///<summary>BillMiddleName. </summary>
		BillMiddleName,
		///<summary>BillLastName. </summary>
		BillLastName,
		///<summary>BillCompany. </summary>
		BillCompany,
		///<summary>BillStreet1. </summary>
		BillStreet1,
		///<summary>BillStreet2. </summary>
		BillStreet2,
		///<summary>BillStreet3. </summary>
		BillStreet3,
		///<summary>BillCity. </summary>
		BillCity,
		///<summary>BillStateProvCode. </summary>
		BillStateProvCode,
		///<summary>BillPostalCode. </summary>
		BillPostalCode,
		///<summary>BillCountryCode. </summary>
		BillCountryCode,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>BillFax. </summary>
		BillFax,
		///<summary>BillEmail. </summary>
		BillEmail,
		///<summary>BillWebsite. </summary>
		BillWebsite,
		///<summary>BillAddressValidationSuggestionCount. </summary>
		BillAddressValidationSuggestionCount,
		///<summary>BillAddressValidationStatus. </summary>
		BillAddressValidationStatus,
		///<summary>BillAddressValidationError. </summary>
		BillAddressValidationError,
		///<summary>BillResidentialStatus. </summary>
		BillResidentialStatus,
		///<summary>BillPOBox. </summary>
		BillPOBox,
		///<summary>BillUSTerritory. </summary>
		BillUSTerritory,
		///<summary>BillMilitaryAddress. </summary>
		BillMilitaryAddress,
		///<summary>ShipFirstName. </summary>
		ShipFirstName,
		///<summary>ShipMiddleName. </summary>
		ShipMiddleName,
		///<summary>ShipLastName. </summary>
		ShipLastName,
		///<summary>ShipCompany. </summary>
		ShipCompany,
		///<summary>ShipStreet1. </summary>
		ShipStreet1,
		///<summary>ShipStreet2. </summary>
		ShipStreet2,
		///<summary>ShipStreet3. </summary>
		ShipStreet3,
		///<summary>ShipCity. </summary>
		ShipCity,
		///<summary>ShipStateProvCode. </summary>
		ShipStateProvCode,
		///<summary>ShipPostalCode. </summary>
		ShipPostalCode,
		///<summary>ShipCountryCode. </summary>
		ShipCountryCode,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipFax. </summary>
		ShipFax,
		///<summary>ShipEmail. </summary>
		ShipEmail,
		///<summary>ShipWebsite. </summary>
		ShipWebsite,
		///<summary>ShipAddressValidationSuggestionCount. </summary>
		ShipAddressValidationSuggestionCount,
		///<summary>ShipAddressValidationStatus. </summary>
		ShipAddressValidationStatus,
		///<summary>ShipAddressValidationError. </summary>
		ShipAddressValidationError,
		///<summary>ShipResidentialStatus. </summary>
		ShipResidentialStatus,
		///<summary>ShipPOBox. </summary>
		ShipPOBox,
		///<summary>ShipUSTerritory. </summary>
		ShipUSTerritory,
		///<summary>ShipMilitaryAddress. </summary>
		ShipMilitaryAddress,
		///<summary>RollupItemCount. </summary>
		RollupItemCount,
		///<summary>RollupItemName. </summary>
		RollupItemName,
		///<summary>RollupItemCode. </summary>
		RollupItemCode,
		///<summary>RollupItemSKU. </summary>
		RollupItemSKU,
		///<summary>RollupItemLocation. </summary>
		RollupItemLocation,
		///<summary>RollupItemQuantity. </summary>
		RollupItemQuantity,
		///<summary>RollupItemTotalWeight. </summary>
		RollupItemTotalWeight,
		///<summary>RollupNoteCount. </summary>
		RollupNoteCount,
		///<summary>BillNameParseStatus. </summary>
		BillNameParseStatus,
		///<summary>BillUnparsedName. </summary>
		BillUnparsedName,
		///<summary>ShipNameParseStatus. </summary>
		ShipNameParseStatus,
		///<summary>ShipUnparsedName. </summary>
		ShipUnparsedName,
		///<summary>ShipSenseHashKey. </summary>
		ShipSenseHashKey,
		///<summary>ShipSenseRecognitionStatus. </summary>
		ShipSenseRecognitionStatus,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>YahooOrderID. </summary>
		YahooOrderID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: YahooOrderItem.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum YahooOrderItemFieldIndex:int
	{
		///<summary>OrderItemID. Inherited from OrderItem</summary>
		OrderItemID_OrderItem,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>OrderID. </summary>
		OrderID,
		///<summary>Name. </summary>
		Name,
		///<summary>Code. </summary>
		Code,
		///<summary>SKU. </summary>
		SKU,
		///<summary>ISBN. </summary>
		ISBN,
		///<summary>UPC. </summary>
		UPC,
		///<summary>Description. </summary>
		Description,
		///<summary>Location. </summary>
		Location,
		///<summary>Image. </summary>
		Image,
		///<summary>Thumbnail. </summary>
		Thumbnail,
		///<summary>UnitPrice. </summary>
		UnitPrice,
		///<summary>UnitCost. </summary>
		UnitCost,
		///<summary>Weight. </summary>
		Weight,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>LocalStatus. </summary>
		LocalStatus,
		///<summary>IsManual. </summary>
		IsManual,
		///<summary>OrderItemID. </summary>
		OrderItemID,
		///<summary>YahooProductID. </summary>
		YahooProductID,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: YahooProduct.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum YahooProductFieldIndex:int
	{
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>YahooProductID. </summary>
		YahooProductID,
		///<summary>Weight. </summary>
		Weight,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: YahooStore.
	/// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum YahooStoreFieldIndex:int
	{
		///<summary>StoreID. Inherited from Store</summary>
		StoreID_Store,
		///<summary>RowVersion. </summary>
		RowVersion,
		///<summary>License. </summary>
		License,
		///<summary>Edition. </summary>
		Edition,
		///<summary>TypeCode. </summary>
		TypeCode,
		///<summary>Enabled. </summary>
		Enabled,
		///<summary>SetupComplete. </summary>
		SetupComplete,
		///<summary>StoreName. </summary>
		StoreName,
		///<summary>Company. </summary>
		Company,
		///<summary>Street1. </summary>
		Street1,
		///<summary>Street2. </summary>
		Street2,
		///<summary>Street3. </summary>
		Street3,
		///<summary>City. </summary>
		City,
		///<summary>StateProvCode. </summary>
		StateProvCode,
		///<summary>PostalCode. </summary>
		PostalCode,
		///<summary>CountryCode. </summary>
		CountryCode,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Fax. </summary>
		Fax,
		///<summary>Email. </summary>
		Email,
		///<summary>Website. </summary>
		Website,
		///<summary>AutoDownload. </summary>
		AutoDownload,
		///<summary>AutoDownloadMinutes. </summary>
		AutoDownloadMinutes,
		///<summary>AutoDownloadOnlyAway. </summary>
		AutoDownloadOnlyAway,
		///<summary>AddressValidationSetting. </summary>
		AddressValidationSetting,
		///<summary>ComputerDownloadPolicy. </summary>
		ComputerDownloadPolicy,
		///<summary>DefaultEmailAccountID. </summary>
		DefaultEmailAccountID,
		///<summary>ManualOrderPrefix. </summary>
		ManualOrderPrefix,
		///<summary>ManualOrderPostfix. </summary>
		ManualOrderPostfix,
		///<summary>InitialDownloadDays. </summary>
		InitialDownloadDays,
		///<summary>InitialDownloadOrder. </summary>
		InitialDownloadOrder,
		///<summary>StoreID. </summary>
		StoreID,
		///<summary>YahooEmailAccountID. </summary>
		YahooEmailAccountID,
		///<summary>TrackingUpdatePassword. </summary>
		TrackingUpdatePassword,
		/// <summary></summary>
		AmountOfFields
	}





	/// <summary>
	/// Enum definition for all the entity types defined in this namespace. Used by the entityfields factory.
	/// </summary>
	[Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
	public enum EntityType:int
	{
		///<summary>Action</summary>
		ActionEntity,
		///<summary>ActionFilterTrigger</summary>
		ActionFilterTriggerEntity,
		///<summary>ActionQueue</summary>
		ActionQueueEntity,
		///<summary>ActionQueueSelection</summary>
		ActionQueueSelectionEntity,
		///<summary>ActionQueueStep</summary>
		ActionQueueStepEntity,
		///<summary>ActionTask</summary>
		ActionTaskEntity,
		///<summary>AmazonAccount</summary>
		AmazonAccountEntity,
		///<summary>AmazonASIN</summary>
		AmazonASINEntity,
		///<summary>AmazonOrder</summary>
		AmazonOrderEntity,
		///<summary>AmazonOrderItem</summary>
		AmazonOrderItemEntity,
		///<summary>AmazonStore</summary>
		AmazonStoreEntity,
		///<summary>AmeriCommerceStore</summary>
		AmeriCommerceStoreEntity,
		///<summary>Audit</summary>
		AuditEntity,
		///<summary>AuditChange</summary>
		AuditChangeEntity,
		///<summary>AuditChangeDetail</summary>
		AuditChangeDetailEntity,
		///<summary>BestRateProfile</summary>
		BestRateProfileEntity,
		///<summary>BestRateShipment</summary>
		BestRateShipmentEntity,
		///<summary>BigCommerceOrderItem</summary>
		BigCommerceOrderItemEntity,
		///<summary>BigCommerceStore</summary>
		BigCommerceStoreEntity,
		///<summary>BuyDotComOrderItem</summary>
		BuyDotComOrderItemEntity,
		///<summary>BuyDotComStore</summary>
		BuyDotComStoreEntity,
		///<summary>ChannelAdvisorOrder</summary>
		ChannelAdvisorOrderEntity,
		///<summary>ChannelAdvisorOrderItem</summary>
		ChannelAdvisorOrderItemEntity,
		///<summary>ChannelAdvisorStore</summary>
		ChannelAdvisorStoreEntity,
		///<summary>ClickCartProOrder</summary>
		ClickCartProOrderEntity,
		///<summary>CommerceInterfaceOrder</summary>
		CommerceInterfaceOrderEntity,
		///<summary>Computer</summary>
		ComputerEntity,
		///<summary>Configuration</summary>
		ConfigurationEntity,
		///<summary>Customer</summary>
		CustomerEntity,
		///<summary>DimensionsProfile</summary>
		DimensionsProfileEntity,
		///<summary>Download</summary>
		DownloadEntity,
		///<summary>DownloadDetail</summary>
		DownloadDetailEntity,
		///<summary>EbayCombinedOrderRelation</summary>
		EbayCombinedOrderRelationEntity,
		///<summary>EbayOrder</summary>
		EbayOrderEntity,
		///<summary>EbayOrderItem</summary>
		EbayOrderItemEntity,
		///<summary>EbayStore</summary>
		EbayStoreEntity,
		///<summary>EmailAccount</summary>
		EmailAccountEntity,
		///<summary>EmailOutbound</summary>
		EmailOutboundEntity,
		///<summary>EmailOutboundRelation</summary>
		EmailOutboundRelationEntity,
		///<summary>EndiciaAccount</summary>
		EndiciaAccountEntity,
		///<summary>EndiciaProfile</summary>
		EndiciaProfileEntity,
		///<summary>EndiciaScanForm</summary>
		EndiciaScanFormEntity,
		///<summary>EndiciaShipment</summary>
		EndiciaShipmentEntity,
		///<summary>EtsyOrder</summary>
		EtsyOrderEntity,
		///<summary>EtsyStore</summary>
		EtsyStoreEntity,
		///<summary>ExcludedPackageType</summary>
		ExcludedPackageTypeEntity,
		///<summary>ExcludedServiceType</summary>
		ExcludedServiceTypeEntity,
		///<summary>FedExAccount</summary>
		FedExAccountEntity,
		///<summary>FedExEndOfDayClose</summary>
		FedExEndOfDayCloseEntity,
		///<summary>FedExPackage</summary>
		FedExPackageEntity,
		///<summary>FedExProfile</summary>
		FedExProfileEntity,
		///<summary>FedExProfilePackage</summary>
		FedExProfilePackageEntity,
		///<summary>FedExShipment</summary>
		FedExShipmentEntity,
		///<summary>Filter</summary>
		FilterEntity,
		///<summary>FilterLayout</summary>
		FilterLayoutEntity,
		///<summary>FilterNode</summary>
		FilterNodeEntity,
		///<summary>FilterNodeColumnSettings</summary>
		FilterNodeColumnSettingsEntity,
		///<summary>FilterNodeContent</summary>
		FilterNodeContentEntity,
		///<summary>FilterNodeContentDetail</summary>
		FilterNodeContentDetailEntity,
		///<summary>FilterSequence</summary>
		FilterSequenceEntity,
		///<summary>FtpAccount</summary>
		FtpAccountEntity,
		///<summary>GenericFileStore</summary>
		GenericFileStoreEntity,
		///<summary>GenericModuleStore</summary>
		GenericModuleStoreEntity,
		///<summary>GridColumnFormat</summary>
		GridColumnFormatEntity,
		///<summary>GridColumnLayout</summary>
		GridColumnLayoutEntity,
		///<summary>GridColumnPosition</summary>
		GridColumnPositionEntity,
		///<summary>GrouponOrder</summary>
		GrouponOrderEntity,
		///<summary>GrouponOrderItem</summary>
		GrouponOrderItemEntity,
		///<summary>GrouponStore</summary>
		GrouponStoreEntity,
		///<summary>InfopiaOrderItem</summary>
		InfopiaOrderItemEntity,
		///<summary>InfopiaStore</summary>
		InfopiaStoreEntity,
		///<summary>InsurancePolicy</summary>
		InsurancePolicyEntity,
		///<summary>IParcelAccount</summary>
		IParcelAccountEntity,
		///<summary>IParcelPackage</summary>
		IParcelPackageEntity,
		///<summary>IParcelProfile</summary>
		IParcelProfileEntity,
		///<summary>IParcelProfilePackage</summary>
		IParcelProfilePackageEntity,
		///<summary>IParcelShipment</summary>
		IParcelShipmentEntity,
		///<summary>LabelSheet</summary>
		LabelSheetEntity,
		///<summary>MagentoOrder</summary>
		MagentoOrderEntity,
		///<summary>MagentoStore</summary>
		MagentoStoreEntity,
		///<summary>MarketplaceAdvisorOrder</summary>
		MarketplaceAdvisorOrderEntity,
		///<summary>MarketplaceAdvisorStore</summary>
		MarketplaceAdvisorStoreEntity,
		///<summary>MivaOrderItemAttribute</summary>
		MivaOrderItemAttributeEntity,
		///<summary>MivaStore</summary>
		MivaStoreEntity,
		///<summary>NetworkSolutionsOrder</summary>
		NetworkSolutionsOrderEntity,
		///<summary>NetworkSolutionsStore</summary>
		NetworkSolutionsStoreEntity,
		///<summary>NeweggOrder</summary>
		NeweggOrderEntity,
		///<summary>NeweggOrderItem</summary>
		NeweggOrderItemEntity,
		///<summary>NeweggStore</summary>
		NeweggStoreEntity,
		///<summary>Note</summary>
		NoteEntity,
		///<summary>ObjectLabel</summary>
		ObjectLabelEntity,
		///<summary>ObjectReference</summary>
		ObjectReferenceEntity,
		///<summary>OnTracAccount</summary>
		OnTracAccountEntity,
		///<summary>OnTracProfile</summary>
		OnTracProfileEntity,
		///<summary>OnTracShipment</summary>
		OnTracShipmentEntity,
		///<summary>Order</summary>
		OrderEntity,
		///<summary>OrderCharge</summary>
		OrderChargeEntity,
		///<summary>OrderItem</summary>
		OrderItemEntity,
		///<summary>OrderItemAttribute</summary>
		OrderItemAttributeEntity,
		///<summary>OrderMotionOrder</summary>
		OrderMotionOrderEntity,
		///<summary>OrderMotionStore</summary>
		OrderMotionStoreEntity,
		///<summary>OrderPaymentDetail</summary>
		OrderPaymentDetailEntity,
		///<summary>OtherProfile</summary>
		OtherProfileEntity,
		///<summary>OtherShipment</summary>
		OtherShipmentEntity,
		///<summary>PayPalOrder</summary>
		PayPalOrderEntity,
		///<summary>PayPalStore</summary>
		PayPalStoreEntity,
		///<summary>Permission</summary>
		PermissionEntity,
		///<summary>PostalProfile</summary>
		PostalProfileEntity,
		///<summary>PostalShipment</summary>
		PostalShipmentEntity,
		///<summary>PrintResult</summary>
		PrintResultEntity,
		///<summary>ProStoresOrder</summary>
		ProStoresOrderEntity,
		///<summary>ProStoresStore</summary>
		ProStoresStoreEntity,
		///<summary>Resource</summary>
		ResourceEntity,
		///<summary>ScanFormBatch</summary>
		ScanFormBatchEntity,
		///<summary>Search</summary>
		SearchEntity,
		///<summary>SearsOrder</summary>
		SearsOrderEntity,
		///<summary>SearsOrderItem</summary>
		SearsOrderItemEntity,
		///<summary>SearsStore</summary>
		SearsStoreEntity,
		///<summary>ServerMessage</summary>
		ServerMessageEntity,
		///<summary>ServerMessageSignoff</summary>
		ServerMessageSignoffEntity,
		///<summary>ServiceStatus</summary>
		ServiceStatusEntity,
		///<summary>Shipment</summary>
		ShipmentEntity,
		///<summary>ShipmentCustomsItem</summary>
		ShipmentCustomsItemEntity,
		///<summary>ShippingDefaultsRule</summary>
		ShippingDefaultsRuleEntity,
		///<summary>ShippingOrigin</summary>
		ShippingOriginEntity,
		///<summary>ShippingPrintOutput</summary>
		ShippingPrintOutputEntity,
		///<summary>ShippingPrintOutputRule</summary>
		ShippingPrintOutputRuleEntity,
		///<summary>ShippingProfile</summary>
		ShippingProfileEntity,
		///<summary>ShippingProviderRule</summary>
		ShippingProviderRuleEntity,
		///<summary>ShippingSettings</summary>
		ShippingSettingsEntity,
		///<summary>ShipSenseKnowledgebase</summary>
		ShipSenseKnowledgebaseEntity,
		///<summary>ShopifyOrder</summary>
		ShopifyOrderEntity,
		///<summary>ShopifyOrderItem</summary>
		ShopifyOrderItemEntity,
		///<summary>ShopifyStore</summary>
		ShopifyStoreEntity,
		///<summary>ShopSiteStore</summary>
		ShopSiteStoreEntity,
		///<summary>StatusPreset</summary>
		StatusPresetEntity,
		///<summary>Store</summary>
		StoreEntity,
		///<summary>SystemData</summary>
		SystemDataEntity,
		///<summary>Template</summary>
		TemplateEntity,
		///<summary>TemplateComputerSettings</summary>
		TemplateComputerSettingsEntity,
		///<summary>TemplateFolder</summary>
		TemplateFolderEntity,
		///<summary>TemplateStoreSettings</summary>
		TemplateStoreSettingsEntity,
		///<summary>TemplateUserSettings</summary>
		TemplateUserSettingsEntity,
		///<summary>ThreeDCartOrderItem</summary>
		ThreeDCartOrderItemEntity,
		///<summary>ThreeDCartStore</summary>
		ThreeDCartStoreEntity,
		///<summary>UpsAccount</summary>
		UpsAccountEntity,
		///<summary>UpsPackage</summary>
		UpsPackageEntity,
		///<summary>UpsProfile</summary>
		UpsProfileEntity,
		///<summary>UpsProfilePackage</summary>
		UpsProfilePackageEntity,
		///<summary>UpsShipment</summary>
		UpsShipmentEntity,
		///<summary>User</summary>
		UserEntity,
		///<summary>UserColumnSettings</summary>
		UserColumnSettingsEntity,
		///<summary>UserSettings</summary>
		UserSettingsEntity,
		///<summary>UspsAccount</summary>
		UspsAccountEntity,
		///<summary>UspsProfile</summary>
		UspsProfileEntity,
		///<summary>UspsScanForm</summary>
		UspsScanFormEntity,
		///<summary>UspsShipment</summary>
		UspsShipmentEntity,
		///<summary>ValidatedAddress</summary>
		ValidatedAddressEntity,
		///<summary>VersionSignoff</summary>
		VersionSignoffEntity,
		///<summary>VolusionStore</summary>
		VolusionStoreEntity,
		///<summary>WorldShipGoods</summary>
		WorldShipGoodsEntity,
		///<summary>WorldShipPackage</summary>
		WorldShipPackageEntity,
		///<summary>WorldShipProcessed</summary>
		WorldShipProcessedEntity,
		///<summary>WorldShipShipment</summary>
		WorldShipShipmentEntity,
		///<summary>YahooOrder</summary>
		YahooOrderEntity,
		///<summary>YahooOrderItem</summary>
		YahooOrderItemEntity,
		///<summary>YahooProduct</summary>
		YahooProductEntity,
		///<summary>YahooStore</summary>
		YahooStoreEntity
	}




	#region Custom ConstantsEnums Code
	
	// __LLBLGENPRO_USER_CODE_REGION_START CustomUserConstants
	// __LLBLGENPRO_USER_CODE_REGION_END
	#endregion

	#region Included code

	#endregion
}


