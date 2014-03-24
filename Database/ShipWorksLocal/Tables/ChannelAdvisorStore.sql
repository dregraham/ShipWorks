CREATE TABLE [dbo].[ChannelAdvisorStore] (
    [StoreID]          BIGINT        NOT NULL,
    [AccountKey]       NVARCHAR (50) NOT NULL,
    [DownloadCriteria] SMALLINT      NOT NULL,
    [ProfileID]        INT           NOT NULL,
    CONSTRAINT [PK_ChannelAdvisorStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_ChannelAdvisorStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

