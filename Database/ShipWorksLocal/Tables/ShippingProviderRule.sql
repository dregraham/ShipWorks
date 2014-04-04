CREATE TABLE [dbo].[ShippingProviderRule] (
    [ShippingProviderRuleID] BIGINT     IDENTITY (1060, 1000) NOT NULL,
    [RowVersion]             ROWVERSION NOT NULL,
    [FilterNodeID]           BIGINT     NOT NULL,
    [ShipmentType]           INT        NOT NULL,
    CONSTRAINT [PK_ShippingProviderRule] PRIMARY KEY CLUSTERED ([ShippingProviderRuleID] ASC)
);


GO
ALTER TABLE [dbo].[ShippingProviderRule] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

