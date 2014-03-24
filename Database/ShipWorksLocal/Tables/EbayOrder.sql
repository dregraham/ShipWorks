CREATE TABLE [dbo].[EbayOrder] (
    [OrderID]                        BIGINT         NOT NULL,
    [EbayOrderID]                    BIGINT         NOT NULL,
    [EbayBuyerID]                    NVARCHAR (50)  NOT NULL,
    [CombinedLocally]                BIT            NOT NULL,
    [SelectedShippingMethod]         INT            CONSTRAINT [DF__EbayOrder__Selec__2B203F5D] DEFAULT ((0)) NOT NULL,
    [SellingManagerRecord]           INT            NULL,
    [GspEligible]                    BIT            NOT NULL,
    [GspFirstName]                   NVARCHAR (128) NOT NULL,
    [GspLastName]                    NVARCHAR (128) NOT NULL,
    [GspStreet1]                     NVARCHAR (512) NOT NULL,
    [GspStreet2]                     NVARCHAR (512) NOT NULL,
    [GspCity]                        NVARCHAR (128) NOT NULL,
    [GspStateProvince]               NVARCHAR (128) NOT NULL,
    [GspPostalCode]                  NVARCHAR (9)   NOT NULL,
    [GspCountryCode]                 NVARCHAR (2)   NOT NULL,
    [GspReferenceID]                 NVARCHAR (128) NOT NULL,
    [RollupEbayItemCount]            INT            NOT NULL,
    [RollupEffectiveCheckoutStatus]  INT            NULL,
    [RollupEffectivePaymentMethod]   INT            NULL,
    [RollupFeedbackLeftType]         INT            NULL,
    [RollupFeedbackLeftComments]     VARCHAR (80)   NULL,
    [RollupFeedbackReceivedType]     INT            NULL,
    [RollupFeedbackReceivedComments] VARCHAR (80)   NULL,
    [RollupPayPalAddressStatus]      INT            NULL,
    CONSTRAINT [PK_EbayOrder] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_EbayOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);


GO
CREATE TRIGGER [dbo].[EbayOrderAuditTrigger]
    ON [dbo].[EbayOrder]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[EbayOrderAuditTrigger]


GO
CREATE TRIGGER [dbo].[FilterDirtyEbayOrder]
    ON [dbo].[EbayOrder]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyEbayOrder]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'CombinedLocally';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'128', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'SelectedShippingMethod';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Shipping Method', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'SelectedShippingMethod';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'SellingManagerRecord';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'RollupEbayItemCount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'RollupEffectiveCheckoutStatus';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'RollupEffectivePaymentMethod';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'RollupFeedbackLeftType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'RollupFeedbackLeftComments';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'RollupFeedbackReceivedType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'RollupFeedbackReceivedComments';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EbayOrder', @level2type = N'COLUMN', @level2name = N'RollupPayPalAddressStatus';

