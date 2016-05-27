IF object_id('dbo.v2m_MigrationPlan') IS NULL
	 CREATE TABLE [dbo].[v2m_MigrationPlan](
		[MigrationPlanId] [bigint] IDENTITY(1,1) NOT NULL,
		[TaskTypeCode] [int] NOT NULL,
		[TaskIdentifier] [nvarchar](255) NOT NULL,
		[DatabaseName] [nvarchar](255) NOT NULL,
		[IsArchiveDB] [bit] NOT NULL,
		[CreateTime] [datetime] NOT NULL,
		[StartTime] [datetime] NOT NULL,
		[LastUpdated] [datetime] NOT NULL,
		[Completed] [bit] NOT NULL,
		[EstimatedWork] [int] NOT NULL,
		[Progress] [int] NOT NULL,
	 CONSTRAINT [PK_MigrationPlan] PRIMARY KEY CLUSTERED ( [MigrationPlanId] ASC ))

IF object_id('dbo.v2m_PostMigrationProgress') IS NULL 
	CREATE TABLE [dbo].[v2m_PostMigrationProgress](
		[PostMigrationProgressID] [bigint] IDENTITY(1,1) NOT NULL,
		[Identifier] [int] NOT NULL,
	CONSTRAINT [PK_PostMigrationProgress] PRIMARY KEY CLUSTERED ([PostMigrationProgressID] ASC))