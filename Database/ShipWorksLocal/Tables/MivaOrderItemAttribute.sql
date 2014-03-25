CREATE TABLE [dbo].[MivaOrderItemAttribute] (
    [OrderItemAttributeID] BIGINT         NOT NULL,
    [MivaOptionCode]       NVARCHAR (300) NOT NULL,
    [MivaAttributeID]      INT            NOT NULL,
    [MivaAttributeCode]    NVARCHAR (300) NOT NULL,
    CONSTRAINT [PK_MivaOrderItemAttributes] PRIMARY KEY CLUSTERED ([OrderItemAttributeID] ASC),
    CONSTRAINT [FK_MivaOrderItemAttribute_OrderItemAttribute] FOREIGN KEY ([OrderItemAttributeID]) REFERENCES [dbo].[OrderItemAttribute] ([OrderItemAttributeID])
);

