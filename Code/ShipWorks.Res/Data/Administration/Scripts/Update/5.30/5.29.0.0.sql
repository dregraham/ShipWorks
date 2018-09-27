SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Adding column EntityExistsQuery to [dbo].[FilterNodeContent]'
GO
IF COL_LENGTH(N'[dbo].[FilterNodeContent]', N'EntityExistsQuery') IS NULL
ALTER TABLE [dbo].[FilterNodeContent] ADD [EntityExistsQuery] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT DF_FilterNodeContent_EntityExistsQuery DEFAULT ''
GO 
PRINT N'Removing constraints'
GO
IF OBJECT_ID('[dbo].[DF_FilterNodeContent_EntityExistsQuery]', 'D') IS NOT NULL
ALTER TABLE [dbo].[FilterNodeContent] DROP CONSTRAINT DF_FilterNodeContent_EntityExistsQuery
GO
PRINT N'Adding FilterNodeSetSwFilterNodeID trigger'
GO
IF EXISTS (SELECT * FROM sys.objects WHERE [name] = N'FilterNodeSetSwFilterNodeID' AND [type] = 'TR')
BEGIN
      DROP TRIGGER [dbo].[FilterNodeSetSwFilterNodeID];
END;
GO

CREATE TRIGGER FilterNodeSetSwFilterNodeID 
   ON  FilterNode
   WITH ENCRYPTION
   AFTER INSERT,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

    declare @FilterNodeID bigint
    declare @FilterNodeContentID bigint
	select @FilterNodeID = FilterNodeID, @FilterNodeContentID = FilterNodeContentID from inserted

	update FilterNodeContent
	set EntityExistsQuery = REPLACE(EntityExistsQuery, '<SwFilterNodeID />', convert(nvarchar(40), @FilterNodeID))
	where FilterNodeContentID = @FilterNodeContentID
END
GO

PRINT 'Creating FilterInfo view.'
GO
IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[FilterInfo]'))
	DROP VIEW [dbo].[FilterInfo]
GO
CREATE VIEW FilterInfo WITH ENCRYPTION AS
	SELECT f.Name, f.FilterID, f.IsFolder, f.State, f.[Definition], n.FilterNodeID, c.*
		FROM FilterNode n INNER JOIN FilterSequence s ON n.FilterSequenceID = s.FilterSequenceID 
						  INNER JOIN Filter f ON s.FilterID = f.FilterID 
						  INNER JOIN FilterNodeContent c ON n.FilterNodeContentID = c.FilterNodeContentID
GO

PRINT 'Creating AreFilterCountsUpToDate.'
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AreFilterCountsUpToDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[AreFilterCountsUpToDate]
END
GO
CREATE FUNCTION [dbo].[AreFilterCountsUpToDate](@currentDbts binary(8))
RETURNS BIT
WITH ENCRYPTION
AS
BEGIN
	DECLARE @upToDate bit = 0
	DECLARE @rowVersion rowversion
	DECLARE @filterNodeContentDirtyMinRowVersion timestamp

	IF (@currentDbts IS NULL OR @currentDbts = CONVERT(VARBINARY, 0x0))
	BEGIN
		SELECT @rowVersion = @@DBTS
	END
	ELSE
	BEGIN
		SELECT @rowVersion = CONVERT(rowversion, @currentDbts)
	END

	SELECT @filterNodeContentDirtyMinRowVersion = MIN(RowVersion) FROM FilterNodeContentDirty WITH (NOLOCK)
	
	IF @filterNodeContentDirtyMinRowVersion is null or len(@filterNodeContentDirtyMinRowVersion) = 0
	BEGIN
		SET @upToDate = 1
	END
	ELSE
	BEGIN
		IF @filterNodeContentDirtyMinRowVersion > @rowVersion
		BEGIN
			SET @upToDate = 1
		END
	END
	
	RETURN(@upToDate);
END
GO

PRINT 'Creating AreQuickFilterCountsUpToDate.'
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AreQuickFilterCountsUpToDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[AreQuickFilterCountsUpToDate]
END
GO

CREATE FUNCTION dbo.AreQuickFilterCountsUpToDate(@currentDbts binary(8))
RETURNS BIT
WITH ENCRYPTION
AS
BEGIN
	DECLARE @upToDate bit = 0
	DECLARE @rowVersion rowversion
	DECLARE @quickFilterNodeContentDirtyMinRowVersion timestamp
	
	IF (@currentDbts IS NULL OR @currentDbts = CONVERT(VARBINARY, 0x0))
	BEGIN
		SELECT @rowVersion = @@DBTS
	END
	ELSE
	BEGIN
		SELECT @rowVersion = CONVERT(rowversion, @currentDbts)
	END

	SELECT @quickFilterNodeContentDirtyMinRowVersion = MIN(RowVersion) FROM QuickFilterNodeContentDirty WITH (NOLOCK)

	IF @quickFilterNodeContentDirtyMinRowVersion is null or len(@quickFilterNodeContentDirtyMinRowVersion) = 0
	BEGIN
		SET @upToDate = 1
	END
	ELSE
	BEGIN
		IF @quickFilterNodeContentDirtyMinRowVersion > @rowVersion
		BEGIN
			SET @upToDate = 1
		END
	END

	RETURN(@upToDate);
END
GO

PRINT N'Adding DoesFilterNodeApplyToEntity stored procedure'
GO
IF EXISTS ( SELECT  * FROM    sys.objects WHERE   object_id = OBJECT_ID(N'DoesFilterNodeApplyToEntity') AND type IN ( N'P', N'PC' ) ) 
BEGIN
	DROP PROCEDURE [dbo].[DoesFilterNodeApplyToEntity]
END
GO

CREATE PROCEDURE DoesFilterNodeApplyToEntity
(
	@entityID BIGINT,
	@filterNodeID BIGINT
)
WITH ENCRYPTION
AS
BEGIN

	DECLARE @upToDate bit
	SELECT @upToDate = dbo.AreFilterCountsUpToDate(NULL) & dbo.AreFilterCountsUpToDate(NULL)
	
	IF @upToDate = 1
	BEGIN
		SELECT fi.FilterNodeID 
		FROM FilterNodeContentDetail fncd, FilterInfo fi
		WHERE fncd.FilterNodeContentID = fi.FilterNodeContentID
		  AND fncd.ObjectID = @entityID
		  AND fi.FilterNodeID = @filterNodeID
	END
	ELSE
	BEGIN
		DECLARE @sql nvarchar(max)
		select @sql = fn.EntityExistsQuery
		from FilterInfo fn
		WHERE fn.FilterNodeID = @filterNodeID

		EXECUTE sp_executesql @sql, N'@ExistsQueryObjectID bigint, @filterNodeID bigint', @ExistsQueryObjectID = @entityID, @filterNodeID = @filterNodeID
	END
END
GO