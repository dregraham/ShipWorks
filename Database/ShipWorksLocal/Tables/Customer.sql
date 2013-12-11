CREATE TABLE [dbo].[Customer] (
    [CustomerID]        BIGINT         IDENTITY (1012, 1000) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    [BillFirstName]     NVARCHAR (30)  NOT NULL,
    [BillMiddleName]    NVARCHAR (30)  NOT NULL,
    [BillLastName]      NVARCHAR (30)  NOT NULL,
    [BillCompany]       NVARCHAR (60)  NOT NULL,
    [BillStreet1]       NVARCHAR (60)  NOT NULL,
    [BillStreet2]       NVARCHAR (60)  NOT NULL,
    [BillStreet3]       NVARCHAR (60)  NOT NULL,
    [BillCity]          NVARCHAR (50)  NOT NULL,
    [BillStateProvCode] NVARCHAR (50)  NOT NULL,
    [BillPostalCode]    NVARCHAR (20)  NOT NULL,
    [BillCountryCode]   NVARCHAR (50)  NOT NULL,
    [BillPhone]         NVARCHAR (25)  NOT NULL,
    [BillFax]           NVARCHAR (35)  NOT NULL,
    [BillEmail]         NVARCHAR (100) NOT NULL,
    [BillWebsite]       NVARCHAR (50)  NOT NULL,
    [ShipFirstName]     NVARCHAR (30)  NOT NULL,
    [ShipMiddleName]    NVARCHAR (30)  NOT NULL,
    [ShipLastName]      NVARCHAR (30)  NOT NULL,
    [ShipCompany]       NVARCHAR (60)  NOT NULL,
    [ShipStreet1]       NVARCHAR (60)  NOT NULL,
    [ShipStreet2]       NVARCHAR (60)  NOT NULL,
    [ShipStreet3]       NVARCHAR (60)  NOT NULL,
    [ShipCity]          NVARCHAR (50)  NOT NULL,
    [ShipStateProvCode] NVARCHAR (50)  NOT NULL,
    [ShipPostalCode]    NVARCHAR (20)  NOT NULL,
    [ShipCountryCode]   NVARCHAR (50)  NOT NULL,
    [ShipPhone]         NVARCHAR (25)  NOT NULL,
    [ShipFax]           NVARCHAR (35)  NOT NULL,
    [ShipEmail]         NVARCHAR (100) NOT NULL,
    [ShipWebsite]       NVARCHAR (50)  NOT NULL,
    [RollupOrderCount]  INT            NOT NULL,
    [RollupOrderTotal]  MONEY          NOT NULL,
    [RollupNoteCount]   INT            NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);


GO
ALTER TABLE [dbo].[Customer] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName]
    ON [dbo].[Customer]([BillLastName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany]
    ON [dbo].[Customer]([BillCompany] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode]
    ON [dbo].[Customer]([BillStateProvCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode]
    ON [dbo].[Customer]([BillPostalCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode]
    ON [dbo].[Customer]([BillCountryCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail]
    ON [dbo].[Customer]([BillEmail] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName]
    ON [dbo].[Customer]([ShipLastName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany]
    ON [dbo].[Customer]([ShipCompany] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode]
    ON [dbo].[Customer]([ShipStateProvCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode]
    ON [dbo].[Customer]([ShipPostalCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode]
    ON [dbo].[Customer]([ShipCountryCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail]
    ON [dbo].[Customer]([ShipEmail] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderCount]
    ON [dbo].[Customer]([RollupOrderCount] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderTotal]
    ON [dbo].[Customer]([RollupOrderTotal] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount]
    ON [dbo].[Customer]([RollupNoteCount] ASC);


GO
CREATE TRIGGER [dbo].[CustomerAuditTrigger]
    ON [dbo].[Customer]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[CustomerAuditTrigger]


GO
CREATE TRIGGER [dbo].[CustomerLabelTrigger]
    ON [dbo].[Customer]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[CustomerLabelTrigger]


GO
CREATE TRIGGER [dbo].[FilterDirtyCustomer]
    ON [dbo].[Customer]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyCustomer]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'BillStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'BillState', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'BillStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'BillCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'BillCountry', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'BillCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'ShipStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'ShipState', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'ShipStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'ShipCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'ShipCountry', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'ShipCountryCode';

