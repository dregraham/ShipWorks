CREATE TABLE [dbo].[OnTracAccount] (
    [OnTracAccountID] BIGINT        IDENTITY (1090, 1000) NOT NULL,
    [RowVersion]      ROWVERSION    NOT NULL,
    [AccountNumber]   INT           NOT NULL,
    [Password]        NVARCHAR (50) NOT NULL,
    [Description]     NVARCHAR (50) NOT NULL,
    [FirstName]       NVARCHAR (30) NOT NULL,
    [MiddleName]      NVARCHAR (30) NOT NULL,
    [LastName]        NVARCHAR (30) NOT NULL,
    [Company]         NVARCHAR (30) NOT NULL,
    [Street1]         NVARCHAR (43) NOT NULL,
    [City]            NVARCHAR (25) NOT NULL,
    [StateProvCode]   NVARCHAR (50) NOT NULL,
    [PostalCode]      NVARCHAR (10) NOT NULL,
    [CountryCode]     NVARCHAR (50) NOT NULL,
    [Email]           NVARCHAR (50) NOT NULL,
    [Phone]           NVARCHAR (15) NOT NULL,
    CONSTRAINT [PK_OnTracAccount] PRIMARY KEY CLUSTERED ([OnTracAccountID] ASC)
);


GO
ALTER TABLE [dbo].[OnTracAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

