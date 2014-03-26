CREATE TABLE [dbo].[EmailAccount] (
    [EmailAccountID]                     BIGINT         IDENTITY (1034, 1000) NOT NULL,
    [RowVersion]                         ROWVERSION     NOT NULL,
    [AccountName]                        NVARCHAR (50)  NOT NULL,
    [DisplayName]                        NVARCHAR (50)  NOT NULL,
    [EmailAddress]                       NVARCHAR (100) NOT NULL,
    [IncomingServer]                     NVARCHAR (100) NOT NULL,
    [IncomingServerType]                 INT            NOT NULL,
    [IncomingPort]                       INT            NOT NULL,
    [IncomingSecurityType]               INT            NOT NULL,
    [IncomingUsername]                   NVARCHAR (50)  NOT NULL,
    [IncomingPassword]                   NVARCHAR (150) NOT NULL,
    [OutgoingServer]                     NVARCHAR (100) NOT NULL,
    [OutgoingPort]                       INT            NOT NULL,
    [OutgoingSecurityType]               INT            NOT NULL,
    [OutgoingCredentialSource]           INT            NOT NULL,
    [OutgoingUsername]                   NVARCHAR (50)  NOT NULL,
    [OutgoingPassword]                   NVARCHAR (150) NOT NULL,
    [AutoSend]                           BIT            NOT NULL,
    [AutoSendMinutes]                    INT            NOT NULL,
    [AutoSendLastTime]                   DATETIME       NOT NULL,
    [LimitMessagesPerConnection]         BIT            NOT NULL,
    [LimitMessagesPerConnectionQuantity] INT            NOT NULL,
    [LimitMessagesPerHour]               BIT            NOT NULL,
    [LimitMessagesPerHourQuantity]       INT            NOT NULL,
    [LimitMessageInterval]               BIT            NOT NULL,
    [LimitMessageIntervalSeconds]        INT            NOT NULL,
    [InternalOwnerID]                    BIGINT         NULL,
    CONSTRAINT [PK_EmailAccount] PRIMARY KEY CLUSTERED ([EmailAccountID] ASC)
);


GO
ALTER TABLE [dbo].[EmailAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

