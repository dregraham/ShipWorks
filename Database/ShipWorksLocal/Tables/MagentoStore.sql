CREATE TABLE [dbo].[MagentoStore] (
    [StoreID]               BIGINT NOT NULL,
    [MagentoTrackingEmails] BIT    NOT NULL,
    [MagentoConnect]        BIT    NOT NULL,
    CONSTRAINT [PK_MagentoStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_MagentoStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
);

