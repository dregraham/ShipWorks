CREATE TABLE [dbo].[FedExEndOfDayClose] (
    [FedExEndOfDayCloseID] BIGINT        IDENTITY (1065, 1000) NOT NULL,
    [FedExAccountID]       BIGINT        NOT NULL,
    [AccountNumber]        NVARCHAR (50) NOT NULL,
    [CloseDate]            DATETIME      NOT NULL,
    [IsSmartPost]          BIT           NOT NULL,
    CONSTRAINT [PK_FedExEndOfDayClose] PRIMARY KEY CLUSTERED ([FedExEndOfDayCloseID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_FedExEndOfDayClose_CloseDate]
    ON [dbo].[FedExEndOfDayClose]([CloseDate] ASC)
    INCLUDE([FedExAccountID]);

