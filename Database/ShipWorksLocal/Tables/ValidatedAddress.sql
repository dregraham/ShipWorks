CREATE TABLE [dbo].[ValidatedAddress] (
    [ValidatedAddressID]     BIGINT IDENTITY (1100, 1000) NOT NULL,
    [ConsumerID]             BIGINT NOT NULL,
	[AddressPrefix]			 NVARCHAR(10)   NOT NULL,
    [IsOriginal]             BIT    NOT NULL,
	[Street1]                NVARCHAR (60)  NOT NULL,
    [Street2]                NVARCHAR (60)  NOT NULL,
    [Street3]                NVARCHAR (60)  NOT NULL,
    [City]                   NVARCHAR (50)  NOT NULL,
    [StateProvCode]          NVARCHAR (50)  NOT NULL,
    [PostalCode]             NVARCHAR (20)  NOT NULL,
    [CountryCode]            NVARCHAR (50)  NOT NULL,
	[ResidentialStatus]      INT            NOT NULL, 
    [POBox]                  INT            NOT NULL, 
    [USTerritory] INT            NOT NULL, 
    [MilitaryAddress]        INT            NOT NULL, 
    CONSTRAINT [PK_ValidatedAddress] PRIMARY KEY CLUSTERED ([ValidatedAddressID] ASC)
);





GO

CREATE INDEX [IX_ValidatedAddress_ConsumerIDAddressPrefix] ON [dbo].[ValidatedAddress] ([ConsumerID], [AddressPrefix])
