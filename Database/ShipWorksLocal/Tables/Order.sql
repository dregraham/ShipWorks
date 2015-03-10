CREATE TABLE [dbo].[Order] (
    [OrderID]               BIGINT         IDENTITY (1006, 1000) NOT NULL,
    [RowVersion]            ROWVERSION     NOT NULL,
    [StoreID]               BIGINT         NOT NULL,
    [CustomerID]            BIGINT         NOT NULL,
    [OrderNumber]           BIGINT         NOT NULL,
    [OrderNumberComplete]   NVARCHAR (50)  NOT NULL,
    [OrderDate]             DATETIME       NOT NULL,
    [OrderTotal]            MONEY          NOT NULL,
    [LocalStatus]           NVARCHAR (100) NOT NULL,
    [IsManual]              BIT            NOT NULL,
    [OnlineLastModified]    DATETIME       NOT NULL,
    [OnlineCustomerID]      SQL_VARIANT    NULL,
    [OnlineStatus]          NVARCHAR (100) NOT NULL,
    [OnlineStatusCode]      SQL_VARIANT    NULL,
    [RequestedShipping]     NVARCHAR (50)  NOT NULL,
    [BillFirstName]         NVARCHAR (30)  NOT NULL,
    [BillMiddleName]        NVARCHAR (30)  NOT NULL,
    [BillLastName]          NVARCHAR (30)  NOT NULL,
    [BillCompany]           NVARCHAR (60)  NOT NULL,
    [BillStreet1]           NVARCHAR (60)  NOT NULL,
    [BillStreet2]           NVARCHAR (60)  NOT NULL,
    [BillStreet3]           NVARCHAR (60)  NOT NULL,
    [BillCity]              NVARCHAR (50)  NOT NULL,
    [BillStateProvCode]     NVARCHAR (50)  NOT NULL,
    [BillPostalCode]        NVARCHAR (20)  NOT NULL,
    [BillCountryCode]       NVARCHAR (50)  NOT NULL,
    [BillPhone]             NVARCHAR (25)  NOT NULL,
    [BillFax]               NVARCHAR (35)  NOT NULL,
    [BillEmail]             NVARCHAR (100) NOT NULL,
    [BillWebsite]           NVARCHAR (50)  NOT NULL,
	[BillAddressValidationSuggestionCount] INT NOT NULL,
	[BillAddressValidationStatus] INT NOT NULL,
	[BillAddressValidationError] NVARCHAR(300) NOT NULL,
	[BillResidentialStatus] INT NOT NULL, 
    [BillPOBox] INT NOT NULL, 
    [BillUSTerritory] INT NOT NULL, 
    [BillMilitaryAddress] INT NOT NULL, 
    [ShipFirstName]         NVARCHAR (30)  NOT NULL,
    [ShipMiddleName]        NVARCHAR (30)  NOT NULL,
    [ShipLastName]          NVARCHAR (30)  NOT NULL,
    [ShipCompany]           NVARCHAR (60)  NOT NULL,
    [ShipStreet1]           NVARCHAR (60)  NOT NULL,
    [ShipStreet2]           NVARCHAR (60)  NOT NULL,
    [ShipStreet3]           NVARCHAR (60)  NOT NULL,
    [ShipCity]              NVARCHAR (50)  NOT NULL,
    [ShipStateProvCode]     NVARCHAR (50)  NOT NULL,
    [ShipPostalCode]        NVARCHAR (20)  NOT NULL,
    [ShipCountryCode]       NVARCHAR (50)  NOT NULL,
    [ShipPhone]             NVARCHAR (25)  NOT NULL,
    [ShipFax]               NVARCHAR (35)  NOT NULL,
    [ShipEmail]             NVARCHAR (100) NOT NULL,
    [ShipWebsite]           NVARCHAR (50)  NOT NULL,
	[ShipAddressValidationSuggestionCount] INT NOT NULL,
	[ShipAddressValidationStatus] INT NOT NULL,
	[ShipAddressValidationError] NVARCHAR(300) NOT NULL,
	[ShipResidentialStatus] INT NOT NULL, 
    [ShipPOBox] INT NOT NULL, 
    [ShipUSTerritory] INT NOT NULL, 
    [ShipMilitaryAddress] INT NOT NULL, 
    [RollupItemCount]       INT            NOT NULL,
    [RollupItemName]        NVARCHAR (300) NULL,
    [RollupItemCode]        NVARCHAR (300) NULL,
    [RollupItemSKU]         NVARCHAR (100) NULL,
    [RollupItemLocation]    NVARCHAR (255) NULL,
    [RollupItemQuantity]    FLOAT (53)     NULL,
    [RollupItemTotalWeight] FLOAT (53)     NOT NULL,
    [RollupNoteCount]       INT            NOT NULL,
    [BillNameParseStatus]   INT            NOT NULL,
    [BillUnparsedName]      NVARCHAR (100) NOT NULL,
    [ShipNameParseStatus]   INT            NOT NULL,
    [ShipUnparsedName]      NVARCHAR (100) NOT NULL,
    [ShipSenseHashKey]           NVARCHAR (64)  COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    [ShipSenseRecognitionStatus] INT            NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID]),
    CONSTRAINT [FK_Order_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);


GO
ALTER TABLE [dbo].[Order] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_OnlineLastModified_StoreID_IsManual]
    ON [dbo].[Order]([OnlineLastModified] DESC, [StoreID] ASC, [IsManual] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_StoreID]
    ON [dbo].[Order]([StoreID] ASC)
    INCLUDE([IsManual]);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_CustomerID]
    ON [dbo].[Order]([CustomerID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumber]
    ON [dbo].[Order]([OrderNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumberComplete]
    ON [dbo].[Order]([OrderNumberComplete] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderDate]
    ON [dbo].[Order]([OrderDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_OrderTotal]
    ON [dbo].[Order]([OrderTotal] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_LocalStatus]
    ON [dbo].[Order]([LocalStatus] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OnlineCustomerID]
    ON [dbo].[Order]([OnlineCustomerID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_OnlineStatus]
    ON [dbo].[Order]([OnlineStatus] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RequestedShipping]
    ON [dbo].[Order]([RequestedShipping] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName]
    ON [dbo].[Order]([BillLastName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany]
    ON [dbo].[Order]([BillCompany] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode]
    ON [dbo].[Order]([BillStateProvCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode]
    ON [dbo].[Order]([BillPostalCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode]
    ON [dbo].[Order]([BillCountryCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail]
    ON [dbo].[Order]([BillEmail] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName]
    ON [dbo].[Order]([ShipLastName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany]
    ON [dbo].[Order]([ShipCompany] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode]
    ON [dbo].[Order]([ShipStateProvCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode]
    ON [dbo].[Order]([ShipPostalCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode]
    ON [dbo].[Order]([ShipCountryCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail]
    ON [dbo].[Order]([ShipEmail] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCount]
    ON [dbo].[Order]([RollupItemCount] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName]
    ON [dbo].[Order]([RollupItemName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCode]
    ON [dbo].[Order]([RollupItemCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemSKU]
    ON [dbo].[Order]([RollupItemSKU] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount]
    ON [dbo].[Order]([RollupNoteCount] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipSenseHashKey] ON [dbo].[Order] ([ShipSenseHashKey])
GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipSenseRecognitionStatus] ON [dbo].[Order] ([ShipSenseRecognitionStatus])
GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'StoreID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Store', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'StoreID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'CustomerID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'CustomerID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'OrderNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Order Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'OrderNumberComplete';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'OrderTotal';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'OnlineCustomerID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'OnlineStatusCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'BillStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'BillState', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'BillStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'BillCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'BillCountry', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'BillCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'ShipStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'ShipState', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'ShipStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'ShipCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'ShipCountry', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'ShipCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'BillNameParseStatus';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order', @level2type = N'COLUMN', @level2name = N'ShipNameParseStatus';

