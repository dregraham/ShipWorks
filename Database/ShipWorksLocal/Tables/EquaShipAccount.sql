CREATE TABLE [dbo].[EquaShipAccount] (
    [EquaShipAccountID] BIGINT         IDENTITY (1067, 1000) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    [Username]          NVARCHAR (50)  NOT NULL,
    [Password]          NVARCHAR (255) NOT NULL,
    [Description]       NVARCHAR (50)  NOT NULL,
    [FirstName]         NVARCHAR (30)  NOT NULL,
    [MiddleName]        NVARCHAR (30)  NOT NULL,
    [LastName]          NVARCHAR (30)  NOT NULL,
    [Company]           NVARCHAR (35)  NOT NULL,
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
    CONSTRAINT [PK_EquahipAccount] PRIMARY KEY CLUSTERED ([EquaShipAccountID] ASC)
);


GO
ALTER TABLE [dbo].[EquaShipAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

