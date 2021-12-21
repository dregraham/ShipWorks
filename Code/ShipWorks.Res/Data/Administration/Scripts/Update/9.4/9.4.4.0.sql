PRINT N'Altering [dbo].[FilterNodeUpdatePending]'
GO
alter table [FilterNodeUpdatePending] alter column [ColumnMask] varbinary(150) null;
GO