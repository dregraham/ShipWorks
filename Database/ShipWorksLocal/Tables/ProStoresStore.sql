CREATE TABLE [dbo].[ProStoresStore] (
    [StoreID]             BIGINT        NOT NULL,
    [ShortName]           VARCHAR (30)  NOT NULL,
    [Username]            VARCHAR (50)  NOT NULL,
    [LoginMethod]         INT           NOT NULL,
    [ApiEntryPoint]       VARCHAR (300) NOT NULL,
    [ApiToken]            TEXT          NOT NULL,
    [ApiStorefrontUrl]    VARCHAR (300) NOT NULL,
    [ApiTokenLogonUrl]    VARCHAR (300) NOT NULL,
    [ApiXteUrl]           VARCHAR (300) NOT NULL,
    [ApiRestSecureUrl]    VARCHAR (300) NOT NULL,
    [ApiRestNonSecureUrl] VARCHAR (300) NOT NULL,
    [ApiRestScriptSuffix] VARCHAR (50)  NOT NULL,
    [LegacyAdminUrl]      VARCHAR (300) NOT NULL,
    [LegacyXtePath]       VARCHAR (75)  NOT NULL,
    [LegacyPrefix]        VARCHAR (30)  NOT NULL,
    [LegacyPassword]      VARCHAR (150) NOT NULL,
    [LegacyCanUpgrade]    BIT           NOT NULL,
    CONSTRAINT [PK_ProStoresStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_ProStoresStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

