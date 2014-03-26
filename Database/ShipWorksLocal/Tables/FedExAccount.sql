CREATE TABLE [dbo].[FedExAccount] (
    [FedExAccountID]   BIGINT         IDENTITY (1055, 1000) NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    [Description]      NVARCHAR (50)  NOT NULL,
    [AccountNumber]    NVARCHAR (12)  NOT NULL,
    [SignatureRelease] VARCHAR (10)   NOT NULL,
    [MeterNumber]      NVARCHAR (50)  NOT NULL,
    [SmartPostHubList] XML            NOT NULL,
    [FirstName]        NVARCHAR (30)  NOT NULL,
    [MiddleName]       NVARCHAR (30)  NOT NULL,
    [LastName]         NVARCHAR (30)  NOT NULL,
    [Company]          NVARCHAR (35)  NOT NULL,
    [Street1]          NVARCHAR (60)  NOT NULL,
    [Street2]          NVARCHAR (60)  NOT NULL,
    [City]             NVARCHAR (50)  NOT NULL,
    [StateProvCode]    NVARCHAR (50)  NOT NULL,
    [PostalCode]       NVARCHAR (20)  NOT NULL,
    [CountryCode]      NVARCHAR (50)  NOT NULL,
    [Phone]            NVARCHAR (25)  NOT NULL,
    [Email]            NVARCHAR (100) NOT NULL,
    [Website]          NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_FedExAccount] PRIMARY KEY CLUSTERED ([FedExAccountID] ASC)
);


GO
ALTER TABLE [dbo].[FedExAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

