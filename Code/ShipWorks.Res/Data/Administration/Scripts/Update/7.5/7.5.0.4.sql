PRINT N'Altering [dbo].[Shipment]'
GO
If(select COL_LENGTH('Shipment','CarrierAccount')) IS NULL
ALTER TABLE Shipment ADD CarrierAccount NVARCHAR(25) NULL
GO
PRINT N'Creating index [IX_SWDefault_Shipment_CarrierAccount] on [dbo].[Shipment]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Shipment]')
                                 AND name = N'IX_SWDefault_Shipment_CarrierAccount')
BEGIN
CREATE NONCLUSTERED INDEX [IX_SWDefault_Shipment_CarrierAccount] ON [dbo].[Shipment] ([CarrierAccount])
END
GO