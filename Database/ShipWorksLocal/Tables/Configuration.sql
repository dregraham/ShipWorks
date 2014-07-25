CREATE TABLE [dbo].[Configuration] (
    [ConfigurationID]                BIT        NOT NULL,
    [RowVersion]                     ROWVERSION NOT NULL,
    [LogOnMethod]                    INT        NOT NULL,
    [AddressCasing]                  BIT        NOT NULL,
    [CustomerCompareEmail]           BIT        NOT NULL,
    [CustomerCompareAddress]         BIT        NOT NULL,
    [CustomerUpdateBilling]          BIT        NOT NULL,
    [CustomerUpdateShipping]         BIT        NOT NULL,
    [CustomerUpdateModifiedBilling]  INT        NOT NULL,
    [CustomerUpdateModifiedShipping] INT        NOT NULL,
    [AuditNewOrders]                 BIT        NOT NULL,
    [AuditDeletedOrders]             BIT        NOT NULL,
    CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([ConfigurationID] ASC)
);

