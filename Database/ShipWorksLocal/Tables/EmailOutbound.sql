CREATE TABLE [dbo].[EmailOutbound] (
    [EmailOutboundID]      BIGINT         IDENTITY (1035, 1000) NOT NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    [ContextID]            BIGINT         NULL,
    [ContextType]          INT            NULL,
    [TemplateID]           BIGINT         NULL,
    [AccountID]            BIGINT         NOT NULL,
    [Visibility]           INT            NOT NULL,
    [FromAddress]          NVARCHAR (200) NOT NULL,
    [ToList]               NVARCHAR (MAX) NOT NULL,
    [CcList]               NVARCHAR (MAX) NOT NULL,
    [BccList]              NVARCHAR (MAX) NOT NULL,
    [Subject]              NVARCHAR (300) NOT NULL,
    [HtmlPartResourceID]   BIGINT         NULL,
    [PlainPartResourceID]  BIGINT         NOT NULL,
    [Encoding]             VARCHAR (20)   NULL,
    [ComposedDate]         DATETIME       NOT NULL,
    [SentDate]             DATETIME       NOT NULL,
    [DontSendBefore]       DATETIME       NULL,
    [SendStatus]           INT            NOT NULL,
    [SendAttemptCount]     INT            NOT NULL,
    [SendAttemptLastError] NVARCHAR (300) NOT NULL,
    CONSTRAINT [PK_EmailOutbound] PRIMARY KEY CLUSTERED ([EmailOutboundID] ASC)
);


GO
ALTER TABLE [dbo].[EmailOutbound] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound]
    ON [dbo].[EmailOutbound]([SendStatus] ASC, [AccountID] ASC, [DontSendBefore] ASC, [SentDate] ASC, [ComposedDate] ASC);


GO
CREATE TRIGGER [dbo].[EmailOutboundDeleteTrigger]
    ON [dbo].[EmailOutbound]
    AFTER DELETE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[EmailOutboundDeleteTrigger]


GO
CREATE TRIGGER [dbo].[FilterDirtyEmailOutbound]
    ON [dbo].[EmailOutbound]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyEmailOutbound]

