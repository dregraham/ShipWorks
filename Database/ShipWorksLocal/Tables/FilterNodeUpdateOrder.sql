CREATE TABLE [dbo].[FilterNodeUpdateOrder] (
    [ObjectID]       BIGINT         NOT NULL,
    [ComputerID]     BIGINT         NOT NULL,
    [ColumnsUpdated] VARBINARY (100) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateOrder]
    ON [dbo].[FilterNodeUpdateOrder]([ObjectID] ASC, [ColumnsUpdated] ASC)
    INCLUDE([ComputerID]) WITH (IGNORE_DUP_KEY = ON);

