CREATE TABLE [dbo].[OrderMotionStore] (
    [StoreID]                   BIGINT NOT NULL,
    [OrderMotionEmailAccountID] BIGINT NOT NULL,
    [OrderMotionBizID]          TEXT   NOT NULL,
    CONSTRAINT [PK_OrderMotionStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_OrderMotionStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

