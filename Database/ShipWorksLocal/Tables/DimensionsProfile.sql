CREATE TABLE [dbo].[DimensionsProfile] (
    [DimensionsProfileID] BIGINT        IDENTITY (1049, 1000) NOT NULL,
    [Name]                NVARCHAR (50) NOT NULL,
    [Length]              FLOAT (53)    NOT NULL,
    [Width]               FLOAT (53)    NOT NULL,
    [Height]              FLOAT (53)    NOT NULL,
    [Weight]              FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_PackagingProfile] PRIMARY KEY CLUSTERED ([DimensionsProfileID] ASC)
);


GO
ALTER TABLE [dbo].[DimensionsProfile] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_DimensionsProfile_Name]
    ON [dbo].[DimensionsProfile]([Name] ASC);

