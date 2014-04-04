CREATE TABLE [dbo].[ServiceStatus] (
    [ServiceStatusID]     BIGINT         IDENTITY (1096, 1000) NOT NULL,
    [RowVersion]          ROWVERSION     NOT NULL,
    [ComputerID]          BIGINT         NOT NULL,
    [ServiceType]         INT            NOT NULL,
    [LastStartDateTime]   DATETIME       NULL,
    [LastStopDateTime]    DATETIME       NULL,
    [LastCheckInDateTime] DATETIME       NULL,
    [ServiceFullName]     NVARCHAR (256) NOT NULL,
    [ServiceDisplayName]  NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_ServiceStatus] PRIMARY KEY CLUSTERED ([ServiceStatusID] ASC),
    CONSTRAINT [FK_ServiceStatus_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]),
    CONSTRAINT [IX_ServiceStatus] UNIQUE NONCLUSTERED ([ComputerID] ASC, [ServiceType] ASC)
);


GO
ALTER TABLE [dbo].[ServiceStatus] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

