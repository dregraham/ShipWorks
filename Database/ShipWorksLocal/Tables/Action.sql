CREATE TABLE [dbo].[Action] (
    [ActionID]            BIGINT         IDENTITY (1040, 1000) NOT NULL,
    [RowVersion]          ROWVERSION     NOT NULL,
    [Name]                NVARCHAR (50)  NOT NULL,
    [Enabled]             BIT            NOT NULL,
    [ComputerLimitedType] INT            NOT NULL,
    [ComputerLimitedList] VARCHAR (150)  NOT NULL,
    [StoreLimited]        BIT            NOT NULL,
    [StoreLimitedList]    NVARCHAR (150) NOT NULL,
    [TriggerType]         INT            NOT NULL,
    [TriggerSettings]     XML            NOT NULL,
    [TaskSummary]         NVARCHAR (150) NOT NULL,
    [InternalOwner]       VARCHAR (50)   NULL,
    CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED ([ActionID] ASC)
);


GO
ALTER TABLE [dbo].[Action] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER [dbo].[ActionDeleteTrigger]
    ON [dbo].[Action]
    AFTER DELETE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[ActionDeleteTrigger]

