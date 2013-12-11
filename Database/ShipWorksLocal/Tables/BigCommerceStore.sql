CREATE TABLE [dbo].[BigCommerceStore] (
    [StoreID]                          BIGINT         NOT NULL,
    [ApiUrl]                           NVARCHAR (110) NOT NULL,
    [ApiUserName]                      NVARCHAR (65)  NOT NULL,
    [ApiToken]                         NVARCHAR (100) NULL,
    [StatusCodes]                      XML            NULL,
    [WeightUnitOfMeasure]              INT            NOT NULL,
    [DownloadModifiedNumberOfDaysBack] INT            NOT NULL,
    CONSTRAINT [PK_BigCommerceStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_BigCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BigCommerceStore', @level2type = N'COLUMN', @level2name = N'ApiUrl';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Api Url', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BigCommerceStore', @level2type = N'COLUMN', @level2name = N'ApiUrl';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BigCommerceStore', @level2type = N'COLUMN', @level2name = N'ApiUserName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Api User Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BigCommerceStore', @level2type = N'COLUMN', @level2name = N'ApiUserName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BigCommerceStore', @level2type = N'COLUMN', @level2name = N'ApiToken';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Api Token', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BigCommerceStore', @level2type = N'COLUMN', @level2name = N'ApiToken';

