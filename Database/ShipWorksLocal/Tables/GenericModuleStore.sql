CREATE TABLE [dbo].[GenericModuleStore] (
    [StoreID]                      BIGINT         NOT NULL,
    [ModuleUsername]               NVARCHAR (50)  NOT NULL,
    [ModulePassword]               NVARCHAR (80)  NOT NULL,
    [ModuleUrl]                    NVARCHAR (350) NOT NULL,
    [ModuleVersion]                VARCHAR (20)   NOT NULL,
    [ModulePlatform]               NVARCHAR (50)  NOT NULL,
    [ModuleDeveloper]              NVARCHAR (50)  NOT NULL,
    [ModuleOnlineStoreCode]        NVARCHAR (50)  NOT NULL,
    [ModuleStatusCodes]            XML            NOT NULL,
    [ModuleDownloadPageSize]       INT            NOT NULL,
    [ModuleRequestTimeout]         INT            NOT NULL,
    [ModuleDownloadStrategy]       INT            NOT NULL,
    [ModuleOnlineStatusSupport]    INT            NOT NULL,
    [ModuleOnlineStatusDataType]   INT            NOT NULL,
    [ModuleOnlineCustomerSupport]  BIT            NOT NULL,
    [ModuleOnlineCustomerDataType] INT            NOT NULL,
    [ModuleOnlineShipmentDetails]  BIT            NOT NULL,
    [ModuleHttpExpect100Continue]  BIT            NOT NULL,
    [ModuleResponseEncoding]       INT            NOT NULL,
    CONSTRAINT [PK_GenericModuleStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_GenericModuleStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

