CREATE TABLE [dbo].[FilterNodeUpdateShipment] (
    [ObjectID]       BIGINT         NOT NULL,
    [ComputerID]     BIGINT         NOT NULL,
    [ColumnsUpdated] VARBINARY (75) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateShipment]
    ON [dbo].[FilterNodeUpdateShipment]([ObjectID] ASC, [ColumnsUpdated] ASC)
    INCLUDE([ComputerID]) WITH (IGNORE_DUP_KEY = ON);

