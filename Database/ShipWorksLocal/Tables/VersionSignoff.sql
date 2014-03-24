CREATE TABLE [dbo].[VersionSignoff] (
    [VersionSignoffID] BIGINT        IDENTITY (1038, 1000) NOT NULL,
    [Version]          NVARCHAR (30) NOT NULL,
    [UserID]           BIGINT        NOT NULL,
    [ComputerID]       BIGINT        NOT NULL,
    CONSTRAINT [PK_VersionVerification] PRIMARY KEY CLUSTERED ([VersionSignoffID] ASC),
    CONSTRAINT [FK_VersionVerification_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_VersionSignoff]
    ON [dbo].[VersionSignoff]([ComputerID] ASC, [UserID] ASC);

