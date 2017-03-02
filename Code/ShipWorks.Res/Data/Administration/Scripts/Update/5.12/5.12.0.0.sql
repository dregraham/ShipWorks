﻿PRINT N'Creating [dbo].[WalmartStore]'
GO
CREATE TABLE [dbo].[WalmartStore]
(
    [StoreID] [bigint] NOT NULL,
    [ConsumerID] [nvarchar](50) NOT NULL,
    [PrivateKey] [nvarchar](MAX) NOT NULL,
    [ChannelType] [nvarchar](50) NOT NULL,
)
GO
PRINT N'Creating primary key [PK_WalmartStore] on [dbo].[WalmartStore]'
GO
ALTER TABLE [dbo].WalmartStore ADD CONSTRAINT [PK_WalmartStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[WalmartStore]'
GO
ALTER TABLE [dbo].WalmartStore ADD CONSTRAINT [FK_WalmartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO