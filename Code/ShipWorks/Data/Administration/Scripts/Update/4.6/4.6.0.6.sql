PRINT N'Altering [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ADD
[AmazonShippingToken] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (N'hlkH7XeEA5GJOefdipC2s6DY+ZF7GWI3nazovu5UYESp9FqfeIiKcfyOzL9Mdsy0')
GO