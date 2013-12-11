CREATE TABLE [dbo].[FtpAccount] (
    [FtpAccountID]    BIGINT         IDENTITY (1071, 1000) NOT NULL,
    [Host]            NVARCHAR (100) NOT NULL,
    [Username]        NVARCHAR (50)  NOT NULL,
    [Password]        NVARCHAR (50)  NOT NULL,
    [Port]            INT            NOT NULL,
    [SecurityType]    INT            NOT NULL,
    [Passive]         BIT            NOT NULL,
    [InternalOwnerID] BIGINT         NULL,
    CONSTRAINT [PK_FtpAccount] PRIMARY KEY CLUSTERED ([FtpAccountID] ASC)
);


GO
ALTER TABLE [dbo].[FtpAccount] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

