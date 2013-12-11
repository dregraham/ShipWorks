CREATE TABLE [dbo].[FilterNodeUpdateCheckpoint] (
    [CheckpointID] BIGINT IDENTITY (1070, 1000) NOT NULL,
    [MaxDirtyID]   BIGINT NOT NULL,
    [DirtyCount]   INT    NOT NULL,
    [State]        INT    NOT NULL,
    [Duration]     INT    NOT NULL,
    CONSTRAINT [PK_FilterNodeUpdateCheckpoint] PRIMARY KEY CLUSTERED ([CheckpointID] ASC)
);

