CREATE TABLE [dbo].[SystemData] (
    [SystemDataID]          BIT              NOT NULL,
    [RowVersion]            ROWVERSION       NOT NULL,
    [DatabaseID]            UNIQUEIDENTIFIER NOT NULL,
    [DateFiltersLastUpdate] DATETIME         NOT NULL,
    [TemplateVersion]       VARCHAR (30)     NOT NULL,
    CONSTRAINT [PK_SystemData] PRIMARY KEY CLUSTERED ([SystemDataID] ASC)
);

