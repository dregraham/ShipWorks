CREATE TABLE [dbo].[ThreeDCartStore] (
    [StoreID]                          BIGINT         NOT NULL,
    [StoreUrl]                         NVARCHAR (110) NOT NULL,
    [ApiUserKey]                       NVARCHAR (65)  NOT NULL,
    [TimeZoneID]                       NVARCHAR (100) NULL,
    [StatusCodes]                      XML            NULL,
    [DownloadModifiedNumberOfDaysBack] INT            NOT NULL,
    CONSTRAINT [PK_ThreeDCartStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_ThreeDCartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreeDCartStore', @level2type = N'COLUMN', @level2name = N'StoreUrl';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Store URL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreeDCartStore', @level2type = N'COLUMN', @level2name = N'StoreUrl';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreeDCartStore', @level2type = N'COLUMN', @level2name = N'ApiUserKey';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'User Key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreeDCartStore', @level2type = N'COLUMN', @level2name = N'ApiUserKey';

