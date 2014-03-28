CREATE TABLE [dbo].[ValidatedAddress]
(
	[ValidatedAddressID] BIGINT NOT NULL IDENTITY(1100, 1000),
	[ConsumerID] BIGINT NOT NULL, 
    [IsOriginal] BIT NOT NULL, 
    CONSTRAINT [PK_ValidatedAddress] PRIMARY KEY CLUSTERED ([ValidatedAddressID] ASC)
)
