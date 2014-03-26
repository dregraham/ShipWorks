CREATE TABLE [dbo].[Computer] (
    [ComputerID] BIGINT           IDENTITY (1001, 1000) NOT NULL,
    [RowVersion] ROWVERSION       NOT NULL,
    [Identifier] UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_Computer] PRIMARY KEY CLUSTERED ([ComputerID] ASC),
    CONSTRAINT [UK_Computer_Identifier] UNIQUE NONCLUSTERED ([Identifier] ASC)
);


GO
ALTER TABLE [dbo].[Computer] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

