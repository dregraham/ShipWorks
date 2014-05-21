CREATE TABLE [dbo].[FilterNodeUpdateCustomer] (
    [ObjectID]       BIGINT         NOT NULL,
    [ComputerID]     BIGINT         NOT NULL,
    [ColumnsUpdated] VARBINARY (100) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateCustomer]
    ON [dbo].[FilterNodeUpdateCustomer]([ObjectID] ASC, [ColumnsUpdated] ASC)
    INCLUDE([ComputerID]) WITH (IGNORE_DUP_KEY = ON);

