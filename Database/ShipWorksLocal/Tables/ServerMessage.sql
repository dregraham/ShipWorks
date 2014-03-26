CREATE TABLE [dbo].[ServerMessage] (
    [ServerMessageID] BIGINT         IDENTITY (1036, 1000) NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    [Number]          INT            NOT NULL,
    [Published]       DATETIME       NOT NULL,
    [Active]          BIT            NOT NULL,
    [Dismissable]     BIT            NOT NULL,
    [Expires]         DATETIME       NULL,
    [ResponseTo]      INT            NULL,
    [ResponseAction]  INT            NULL,
    [EditTo]          INT            NULL,
    [Image]           NVARCHAR (350) NOT NULL,
    [PrimaryText]     NVARCHAR (30)  NOT NULL,
    [SecondaryText]   NVARCHAR (60)  NOT NULL,
    [Actions]         NTEXT          NOT NULL,
    [Stores]          NTEXT          NOT NULL,
    [Shippers]        NTEXT          NOT NULL,
    CONSTRAINT [PK_ServerMessage] PRIMARY KEY CLUSTERED ([ServerMessageID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessage_RowVersion]
    ON [dbo].[ServerMessage]([RowVersion] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessage_Number]
    ON [dbo].[ServerMessage]([Number] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ServerMessage_Expires]
    ON [dbo].[ServerMessage]([Expires] ASC);

