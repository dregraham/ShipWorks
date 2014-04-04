CREATE TABLE [dbo].[StatusPreset] (
    [StatusPresetID] BIGINT         IDENTITY (1022, 1000) NOT NULL,
    [RowVersion]     ROWVERSION     NOT NULL,
    [StoreID]        BIGINT         NULL,
    [StatusTarget]   INT            NOT NULL,
    [StatusText]     NVARCHAR (300) NOT NULL,
    [IsDefault]      BIT            NOT NULL,
    CONSTRAINT [PK_StatusPreset] PRIMARY KEY CLUSTERED ([StatusPresetID] ASC),
    CONSTRAINT [FK_StatusPreset_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE
);


GO
ALTER TABLE [dbo].[StatusPreset] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

