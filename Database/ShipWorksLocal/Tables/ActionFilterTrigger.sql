CREATE TABLE [dbo].[ActionFilterTrigger] (
    [ActionID]            BIGINT        NOT NULL,
    [FilterNodeID]        BIGINT        NOT NULL,
    [Direction]           INT           NOT NULL,
    [ComputerLimitedType] INT           NOT NULL,
    [ComputerLimitedList] VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_ActionFilterTrigger] PRIMARY KEY CLUSTERED ([ActionID] ASC),
    CONSTRAINT [FK_ActionFilterTrigger_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID])
);

