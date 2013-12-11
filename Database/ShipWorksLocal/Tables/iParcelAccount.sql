CREATE TABLE [dbo].[iParcelAccount] (
    [iParcelAccountID] BIGINT         IDENTITY (1091, 1000) NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    [Username]         NVARCHAR (50)  NOT NULL,
    [Password]         NVARCHAR (50)  NOT NULL,
    [Description]      NVARCHAR (50)  NOT NULL,
    [FirstName]        NVARCHAR (30)  NOT NULL,
    [MiddleName]       NVARCHAR (30)  NOT NULL,
    [LastName]         NVARCHAR (30)  NOT NULL,
    [Company]          NVARCHAR (30)  NOT NULL,
    [Street1]          NVARCHAR (50)  NOT NULL,
    [Street2]          NVARCHAR (50)  NOT NULL,
    [City]             NVARCHAR (30)  NOT NULL,
    [StateProvCode]    NVARCHAR (30)  NOT NULL,
    [PostalCode]       NVARCHAR (20)  NOT NULL,
    [CountryCode]      NVARCHAR (50)  NOT NULL,
    [Phone]            NVARCHAR (25)  NOT NULL,
    [Email]            NVARCHAR (100) NOT NULL,
    [Website]          NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_iParcelAccount] PRIMARY KEY CLUSTERED ([iParcelAccountID] ASC)
);


GO
ALTER TABLE [dbo].[iParcelAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

