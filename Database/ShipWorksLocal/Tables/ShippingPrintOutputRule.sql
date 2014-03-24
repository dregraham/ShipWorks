CREATE TABLE [dbo].[ShippingPrintOutputRule] (
    [ShippingPrintOutputRuleID] BIGINT IDENTITY (1059, 1000) NOT NULL,
    [ShippingPrintOutputID]     BIGINT NOT NULL,
    [FilterNodeID]              BIGINT NOT NULL,
    [TemplateID]                BIGINT NOT NULL,
    CONSTRAINT [PK_ShippingPrintOutputRule] PRIMARY KEY CLUSTERED ([ShippingPrintOutputRuleID] ASC),
    CONSTRAINT [FK_ShippingPrintOutputRule_ShippingPrintOutput] FOREIGN KEY ([ShippingPrintOutputID]) REFERENCES [dbo].[ShippingPrintOutput] ([ShippingPrintOutputID]) ON DELETE CASCADE
);

