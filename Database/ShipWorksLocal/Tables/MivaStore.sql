CREATE TABLE [dbo].[MivaStore] (
    [StoreID]                       BIGINT        NOT NULL,
    [EncryptionPassphrase]          NVARCHAR (50) NOT NULL,
    [LiveManualOrderNumbers]        BIT           NOT NULL,
    [SebenzaCheckoutDataEnabled]    BIT           NOT NULL,
    [OnlineUpdateStrategy]          INT           NOT NULL,
    [OnlineUpdateStatusChangeEmail] BIT           NOT NULL,
    CONSTRAINT [PK_MivaStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_MivaStore_GenericModuleStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[GenericModuleStore] ([StoreID])
);

