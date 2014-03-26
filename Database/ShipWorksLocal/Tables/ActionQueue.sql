CREATE TABLE [dbo].[ActionQueue] (
    [ActionQueueID]       BIGINT        IDENTITY (1041, 1000) NOT NULL,
    [RowVersion]          ROWVERSION    NOT NULL,
    [ActionID]            BIGINT        NOT NULL,
    [ActionName]          NVARCHAR (50) NOT NULL,
    [ActionQueueType]     INT           CONSTRAINT [DF_ActionQueue_ActionQueueType] DEFAULT ((0)) NOT NULL,
    [ActionVersion]       BINARY (8)    CONSTRAINT [DF_ActionQueue_ActionVersion] DEFAULT ((0)) NOT NULL,
    [QueueVersion]        BINARY (8)    CONSTRAINT [DF_ActionQueue_QueueVersion] DEFAULT (@@dbts) NOT NULL,
    [TriggerDate]         DATETIME      CONSTRAINT [DF_ActionQueue_QueuedDate] DEFAULT (getutcdate()) NOT NULL,
    [TriggerComputerID]   BIGINT        NOT NULL,
    [ComputerLimitedList] VARCHAR (150) NOT NULL,
    [ObjectID]            BIGINT        NULL,
    [Status]              INT           NOT NULL,
    [NextStep]            INT           NOT NULL,
    [ContextLock]         NVARCHAR (36) NULL,
    CONSTRAINT [PK_ActionQueue] PRIMARY KEY CLUSTERED ([ActionQueueID] ASC),
    CONSTRAINT [FK_ActionQueue_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ActionQueue_Computer] FOREIGN KEY ([TriggerComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
);


GO
ALTER TABLE [dbo].[ActionQueue] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_Search]
    ON [dbo].[ActionQueue]([ActionQueueID] ASC, [ActionQueueType] ASC, [Status] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_ContextLock]
    ON [dbo].[ActionQueue]([ContextLock] ASC);

