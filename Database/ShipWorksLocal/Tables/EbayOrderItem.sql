CREATE TABLE [dbo].[EbayOrderItem] (
    [OrderItemID]              BIGINT        NOT NULL,
    [OrderID]                  BIGINT        NOT NULL,
    [EbayItemID]               BIGINT        NOT NULL,
    [EbayTransactionID]        BIGINT        NOT NULL,
    [SellingManagerRecord]     INT           NOT NULL,
    [EffectiveCheckoutStatus]  INT           NOT NULL,
    [EffectivePaymentMethod]   INT           NOT NULL,
    [PaymentStatus]            INT           NOT NULL,
    [PaymentMethod]            INT           NOT NULL,
    [CompleteStatus]           INT           NOT NULL,
    [FeedbackLeftType]         INT           NOT NULL,
    [FeedbackLeftComments]     NVARCHAR (80) NOT NULL,
    [FeedbackReceivedType]     INT           NOT NULL,
    [FeedbackReceivedComments] NVARCHAR (80) NOT NULL,
    [MyEbayPaid]               BIT           NOT NULL,
    [MyEbayShipped]            BIT           NOT NULL,
    [PayPalTransactionID]      VARCHAR (50)  NOT NULL,
    [PayPalAddressStatus]      INT           NOT NULL,
    CONSTRAINT [PK_EbayOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID] ASC),
    CONSTRAINT [FK_EbayOrderItem_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID]),
    CONSTRAINT [FK_EbayOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
);


GO
CREATE TRIGGER [dbo].[EbayOrderItemRollupTrigger]
    ON [dbo].[EbayOrderItem]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[EbayOrderItemRollupTrigger]


GO
CREATE TRIGGER [dbo].[FilterDirtyEbayOrderItem]
    ON [dbo].[EbayOrderItem]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyEbayOrderItem]

