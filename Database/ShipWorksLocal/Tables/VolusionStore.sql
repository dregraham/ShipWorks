CREATE TABLE [dbo].[VolusionStore] (
    [StoreID]           BIGINT        NOT NULL,
    [StoreUrl]          VARCHAR (255) NOT NULL,
    [WebUserName]       VARCHAR (50)  NOT NULL,
    [WebPassword]       VARCHAR (70)  NOT NULL,
    [ApiPassword]       VARCHAR (100) NOT NULL,
    [PaymentMethods]    XML           NOT NULL,
    [ShipmentMethods]   XML           NOT NULL,
    [ServerTimeZone]    VARCHAR (30)  NOT NULL,
    [ServerTimeZoneDST] BIT           NOT NULL,
    CONSTRAINT [PK_VolusionStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_VolusionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

