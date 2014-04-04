CREATE TABLE [dbo].[GridColumnLayout] (
    [GridColumnLayoutID]    BIGINT           IDENTITY (1016, 1000) NOT NULL,
    [DefinitionSet]         INT              NOT NULL,
    [DefaultSortColumnGuid] UNIQUEIDENTIFIER NOT NULL,
    [DefaultSortOrder]      INT              NOT NULL,
    [LastSortColumnGuid]    UNIQUEIDENTIFIER NOT NULL,
    [LastSortOrder]         INT              NOT NULL,
    [DetailViewSettings]    XML              NULL,
    CONSTRAINT [PK_GridLayout] PRIMARY KEY CLUSTERED ([GridColumnLayoutID] ASC)
);

