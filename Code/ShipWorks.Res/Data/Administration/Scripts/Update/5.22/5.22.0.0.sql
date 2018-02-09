CREATE TABLE [dbo].[Shortcut](
	[ShortcutID] [bigint] NOT NULL,
	[Barcode] [nvarchar](50) NOT NULL,
	[Hotkey] [int] NOT NULL,
	[Action] [int] NOT NULL,
	[ObjectID] [bigint] NULL,
 CONSTRAINT [PK_Shortcut] PRIMARY KEY CLUSTERED 
(
	[ShortcutID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO