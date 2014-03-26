CREATE TABLE [dbo].[PayPalStore] (
    [StoreID]                  BIGINT           NOT NULL,
    [ApiCredentialType]        SMALLINT         NOT NULL,
    [ApiUserName]              NVARCHAR (255)   NOT NULL,
    [ApiPassword]              NVARCHAR (80)    NOT NULL,
    [ApiSignature]             NVARCHAR (80)    NOT NULL,
    [ApiCertificate]           VARBINARY (2048) NULL,
    [LastTransactionDate]      DATETIME         NOT NULL,
    [LastValidTransactionDate] DATETIME         NOT NULL,
    CONSTRAINT [PK_PayPalStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_PayPalStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

