PRINT N'Altering [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD
	[FreightGuaranteeType] [int] NOT NULL CONSTRAINT [DF_FedExShipment_FreightGuaranteeType] DEFAULT ((0)),
	[FreightGuaranteeDate] [datetime] NOT NULL CONSTRAINT [DF_FedExShipment_FreightGuaranteeDate] DEFAULT ((GetDate()))
GO

PRINT N'Dropping constraints from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_FreightGuaranteeType]
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [DF_FedExShipment_FreightGuaranteeDate]
GO
