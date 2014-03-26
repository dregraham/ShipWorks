CREATE TABLE [dbo].[UpsAccount] (
    [UpsAccountID]  BIGINT         IDENTITY (1056, 1000) NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    [Description]   NVARCHAR (50)  NOT NULL,
    [AccountNumber] NVARCHAR (10)  NOT NULL,
    [UserID]        NVARCHAR (25)  NOT NULL,
    [Password]      NVARCHAR (25)  NOT NULL,
    [RateType]      INT            NOT NULL,
    [InvoiceAuth]   BIT            NOT NULL,
    [FirstName]     NVARCHAR (30)  NOT NULL,
    [MiddleName]    NVARCHAR (30)  NOT NULL,
    [LastName]      NVARCHAR (30)  NOT NULL,
    [Company]       NVARCHAR (30)  NOT NULL,
    [Street1]       NVARCHAR (60)  NOT NULL,
    [Street2]       NVARCHAR (60)  NOT NULL,
    [Street3]       NVARCHAR (60)  NOT NULL,
    [City]          NVARCHAR (50)  NOT NULL,
    [StateProvCode] NVARCHAR (50)  NOT NULL,
    [PostalCode]    NVARCHAR (20)  NOT NULL,
    [CountryCode]   NVARCHAR (50)  NOT NULL,
    [Phone]         NVARCHAR (25)  NOT NULL,
    [Email]         NVARCHAR (100) NOT NULL,
    [Website]       NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_UpsAccount] PRIMARY KEY CLUSTERED ([UpsAccountID] ASC)
);


GO
ALTER TABLE [dbo].[UpsAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

