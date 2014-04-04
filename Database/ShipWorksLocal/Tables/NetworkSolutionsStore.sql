CREATE TABLE [dbo].[NetworkSolutionsStore] (
    [StoreID]               BIGINT        NOT NULL,
    [UserToken]             VARCHAR (50)  NOT NULL,
    [DownloadOrderStatuses] VARCHAR (50)  NOT NULL,
    [StatusCodes]           XML           NOT NULL,
    [StoreUrl]              VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_NetworkSolutionsStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_NetworkSolutionsStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

