CREATE TABLE [dbo].[Store] (
    [StoreID]                BIGINT         IDENTITY (1005, 1000) NOT NULL,
    [RowVersion]             ROWVERSION     NOT NULL,
    [License]                NVARCHAR (150) NOT NULL,
    [Edition]                NVARCHAR (MAX) NOT NULL,
    [TypeCode]               INT            NOT NULL,
    [Enabled]                BIT            NOT NULL,
    [SetupComplete]          BIT            NOT NULL,
    [StoreName]              NVARCHAR (75)  NOT NULL,
    [Company]                NVARCHAR (60)  NOT NULL,
    [Street1]                NVARCHAR (60)  NOT NULL,
    [Street2]                NVARCHAR (60)  NOT NULL,
    [Street3]                NVARCHAR (60)  NOT NULL,
    [City]                   NVARCHAR (50)  NOT NULL,
    [StateProvCode]          NVARCHAR (50)  NOT NULL,
    [PostalCode]             NVARCHAR (20)  NOT NULL,
    [CountryCode]            NVARCHAR (50)  NOT NULL,
    [Phone]                  NVARCHAR (25)  NOT NULL,
    [Fax]                    NVARCHAR (35)  NOT NULL,
    [Email]                  NVARCHAR (100) NOT NULL,
    [Website]                NVARCHAR (50)  NOT NULL,
    [AutoDownload]           BIT            NOT NULL,
    [AutoDownloadMinutes]    INT            NOT NULL,
    [AutoDownloadOnlyAway]   BIT            NOT NULL,
	[AddressValidationSetting]  INT			NOT NULL,
    [ComputerDownloadPolicy] NVARCHAR (MAX) NOT NULL,
    [DefaultEmailAccountID]  BIGINT         NOT NULL,
    [ManualOrderPrefix]      NVARCHAR (10)  NOT NULL,
    [ManualOrderPostfix]     NVARCHAR (10)  NOT NULL,
    [InitialDownloadDays]    INT            NULL,
    [InitialDownloadOrder]   BIGINT         NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ([StoreID] ASC)
);


GO
ALTER TABLE [dbo].[Store] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Store_StoreName]
    ON [dbo].[Store]([StoreName] ASC);


GO
