CREATE TABLE [dbo].[ActionTask] (
    [ActionTaskID]          BIGINT        IDENTITY (1042, 1000) NOT NULL,
    [ActionID]              BIGINT        NOT NULL,
    [TaskIdentifier]        NVARCHAR (50) NOT NULL,
    [TaskSettings]          XML           NOT NULL,
    [StepIndex]             INT           NOT NULL,
    [InputSource]           INT           NOT NULL,
    [InputFilterNodeID]     BIGINT        NOT NULL,
    [FilterCondition]       BIT           NOT NULL,
    [FilterConditionNodeID] BIGINT        NOT NULL,
    [FlowSuccess]           INT           NOT NULL,
    [FlowSkipped]           INT           NOT NULL,
    [FlowError]             INT           NOT NULL,
    CONSTRAINT [PK_ActionTask] PRIMARY KEY CLUSTERED ([ActionTaskID] ASC),
    CONSTRAINT [FK_ActionTask_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID])
);


GO
ALTER TABLE [dbo].[ActionTask] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

