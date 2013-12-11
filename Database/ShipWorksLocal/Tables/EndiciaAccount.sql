CREATE TABLE [dbo].[EndiciaAccount] (
    [EndiciaAccountID]      BIGINT         IDENTITY (1066, 1000) NOT NULL,
    [EndiciaReseller]       INT            NOT NULL,
    [AccountNumber]         NVARCHAR (50)  NULL,
    [SignupConfirmation]    NVARCHAR (30)  NOT NULL,
    [WebPassword]           NVARCHAR (250) NOT NULL,
    [ApiInitialPassword]    NVARCHAR (250) NOT NULL,
    [ApiUserPassword]       NVARCHAR (250) NOT NULL,
    [AccountType]           INT            NOT NULL,
    [TestAccount]           BIT            NOT NULL,
    [CreatedByShipWorks]    BIT            NOT NULL,
    [Description]           NVARCHAR (50)  NOT NULL,
    [FirstName]             NVARCHAR (30)  NOT NULL,
    [LastName]              NVARCHAR (30)  NOT NULL,
    [Company]               NVARCHAR (30)  NOT NULL,
    [Street1]               NVARCHAR (60)  NOT NULL,
    [Street2]               NVARCHAR (60)  NOT NULL,
    [Street3]               NVARCHAR (60)  NOT NULL,
    [City]                  NVARCHAR (50)  NOT NULL,
    [StateProvCode]         NVARCHAR (50)  NOT NULL,
    [PostalCode]            NVARCHAR (20)  NOT NULL,
    [CountryCode]           NVARCHAR (50)  NOT NULL,
    [Phone]                 NVARCHAR (25)  NOT NULL,
    [Fax]                   NVARCHAR (35)  NOT NULL,
    [Email]                 NVARCHAR (100) NOT NULL,
    [MailingPostalCode]     NVARCHAR (20)  NOT NULL,
    [ScanFormAddressSource] INT            NOT NULL,
    CONSTRAINT [PK_EndiciaAccount] PRIMARY KEY CLUSTERED ([EndiciaAccountID] ASC)
);


GO
ALTER TABLE [dbo].[EndiciaAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

