CREATE TABLE [dbo].[ShippingDefaultsRule] (
    [ShippingDefaultsRuleID] BIGINT     IDENTITY (1057, 1000) NOT NULL,
    [RowVersion]             ROWVERSION NOT NULL,
    [ShipmentType]           INT        NOT NULL,
    [FilterNodeID]           BIGINT     NOT NULL,
    [ShippingProfileID]      BIGINT     NOT NULL,
    [Position]               INT        NOT NULL,
    CONSTRAINT [PK_ShippingDefaultsRule] PRIMARY KEY CLUSTERED ([ShippingDefaultsRuleID] ASC)
);


GO
ALTER TABLE [dbo].[ShippingDefaultsRule] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

