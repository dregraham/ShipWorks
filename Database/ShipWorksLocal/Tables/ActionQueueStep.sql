CREATE TABLE [dbo].[ActionQueueStep] (
    [ActionQueueStepID]     BIGINT         IDENTITY (1043, 1000) NOT NULL,
    [RowVersion]            ROWVERSION     NOT NULL,
    [ActionQueueID]         BIGINT         NOT NULL,
    [StepStatus]            INT            NOT NULL,
    [StepIndex]             INT            NOT NULL,
    [StepName]              NVARCHAR (100) NOT NULL,
    [TaskIdentifier]        NVARCHAR (50)  NOT NULL,
    [TaskSettings]          XML            NOT NULL,
    [InputSource]           INT            NOT NULL,
    [InputFilterNodeID]     BIGINT         NOT NULL,
    [FilterCondition]       BIT            NOT NULL,
    [FilterConditionNodeID] BIGINT         NOT NULL,
    [FlowSuccess]           INT            NOT NULL,
    [FlowSkipped]           INT            NOT NULL,
    [FlowError]             INT            NOT NULL,
    [AttemptDate]           DATETIME       NOT NULL,
    [AttemptError]          NVARCHAR (500) NOT NULL,
    [AttemptCount]          INT            NOT NULL,
    CONSTRAINT [PK_QueueStep] PRIMARY KEY CLUSTERED ([ActionQueueStepID] ASC),
    CONSTRAINT [FK_ActionQueueStep_ActionQueue] FOREIGN KEY ([ActionQueueID]) REFERENCES [dbo].[ActionQueue] ([ActionQueueID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[ActionQueueStepDeleteTrigger]
    ON [dbo].[ActionQueueStep]
    AFTER DELETE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[ActionQueueStepDeleteTrigger]


GO
CREATE TRIGGER [dbo].[ActionQueueStepUpdateTrigger]
    ON [dbo].[ActionQueueStep]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[ActionQueueStepUpdateTrigger]

