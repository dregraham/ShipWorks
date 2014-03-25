CREATE TABLE [dbo].[Search] (
    [SearchID]     BIGINT   IDENTITY (1069, 1000) NOT NULL,
    [Started]      DATETIME NOT NULL,
    [Pinged]       DATETIME NOT NULL,
    [FilterNodeID] BIGINT   NOT NULL,
    [UserID]       BIGINT   NOT NULL,
    [ComputerID]   BIGINT   NOT NULL,
    CONSTRAINT [PK_Search] PRIMARY KEY CLUSTERED ([SearchID] ASC)
);

