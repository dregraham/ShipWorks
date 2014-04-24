CREATE TABLE [dbo].[Address]
(
	[AddressID] BIGINT NOT NULL IDENTITY(1101, 1000),
	[Street1]                NVARCHAR (60)  NOT NULL,
    [Street2]                NVARCHAR (60)  NOT NULL,
    [Street3]                NVARCHAR (60)  NOT NULL,
    [City]                   NVARCHAR (50)  NOT NULL,
    [StateProvCode]          NVARCHAR (50)  NOT NULL,
    [PostalCode]             NVARCHAR (20)  NOT NULL,
    [CountryCode]            NVARCHAR (50)  NOT NULL,
	[ResidentialStatus]      INT            NOT NULL, 
    [POBox]                  INT            NOT NULL, 
    [InternationalTerritory] INT            NOT NULL, 
    [MilitaryAddress]        INT            NOT NULL, 
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressID] ASC)
)
