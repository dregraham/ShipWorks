CREATE TABLE [dbo].[ValidatedAddress] (
    [ValidatedAddressID] BIGINT IDENTITY (1100, 1000) NOT NULL,
    [ConsumerID]         BIGINT NOT NULL,
    [AddressID]          BIGINT NOT NULL,
    [IsOriginal]         BIT    NOT NULL,
    CONSTRAINT [PK_ValidatedAddress] PRIMARY KEY CLUSTERED ([ValidatedAddressID] ASC),
    CONSTRAINT [FK_ValidatedAddress_Address] FOREIGN KEY ([AddressID]) REFERENCES [dbo].[Address] ([AddressID])
);




