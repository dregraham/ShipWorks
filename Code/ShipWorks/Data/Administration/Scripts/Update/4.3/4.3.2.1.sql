-- Add extended properties for auditing Amazon shipments

EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'AmazonAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DimsAddWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'129', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DeliveryExperience'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'CarrierWillPickUp'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'AmazonShipment', 'COLUMN', N'DeclaredValue'
GO