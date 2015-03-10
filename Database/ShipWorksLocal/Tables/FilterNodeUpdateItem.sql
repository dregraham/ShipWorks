CREATE TABLE [dbo].[FilterNodeUpdateItem] (
    [ObjectID]       BIGINT         NOT NULL,
    [ComputerID]     BIGINT         NOT NULL,
    [ColumnsUpdated] VARBINARY (100) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateItem]
    ON [dbo].[FilterNodeUpdateItem]([ObjectID] ASC, [ColumnsUpdated] ASC)
    INCLUDE([ComputerID]) WITH (IGNORE_DUP_KEY = ON);

