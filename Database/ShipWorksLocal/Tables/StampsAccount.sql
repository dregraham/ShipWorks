CREATE TABLE [dbo].[StampsAccount] (
    [StampsAccountID]   BIGINT         IDENTITY (1052, 1000) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    [Username]          NVARCHAR (50)  NOT NULL,
    [Password]          NVARCHAR (100) NOT NULL,
    [FirstName]         NVARCHAR (30)  NOT NULL,
    [MiddleName]        NVARCHAR (30)  NOT NULL,
    [LastName]          NVARCHAR (30)  NOT NULL,
    [Company]           NVARCHAR (30)  NOT NULL,
    [Street1]           NVARCHAR (60)  NOT NULL,
    [Street2]           NVARCHAR (60)  NOT NULL,
    [Street3]           NVARCHAR (60)  NOT NULL,
    [City]              NVARCHAR (50)  NOT NULL,
    [StateProvCode]     NVARCHAR (50)  NOT NULL,
    [PostalCode]        NVARCHAR (20)  NOT NULL,
    [CountryCode]       NVARCHAR (50)  NOT NULL,
    [Phone]             NVARCHAR (25)  NOT NULL,
    [Email]             NVARCHAR (100) NOT NULL,
    [Website]           NVARCHAR (50)  NOT NULL,
    [MailingPostalCode] NVARCHAR (20)  NOT NULL,
    [IsExpress1]        BIT            NOT NULL,
    CONSTRAINT [PK_PostalStampsAccount] PRIMARY KEY CLUSTERED ([StampsAccountID] ASC)
);


GO
ALTER TABLE [dbo].[StampsAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

