CREATE TABLE [dbo].[OrderPaymentDetail] (
    [OrderPaymentDetailID] BIGINT         IDENTITY (1023, 1000) NOT NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    [OrderID]              BIGINT         NOT NULL,
    [Label]                NVARCHAR (100) NOT NULL,
    [Value]                NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_OrderPaymentDetail] PRIMARY KEY CLUSTERED ([OrderPaymentDetailID] ASC),
    CONSTRAINT [FK_OrderPaymentDetail_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
ALTER TABLE [dbo].[OrderPaymentDetail] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_OrderPaymentDetail_OrderID]
    ON [dbo].[OrderPaymentDetail]([OrderID] ASC);


GO
CREATE TRIGGER [dbo].[FilterDirtyOrderPaymentDetail]
    ON [dbo].[OrderPaymentDetail]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyOrderPaymentDetail]

