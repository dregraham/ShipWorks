SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Adding [dbo].[ProcessedShipmentsView]'
GO

IF EXISTS(SELECT * FROM sys.views WHERE [name] = 'ProcessedShipmentsView')
    DROP VIEW ProcessedShipmentsView
GO

create view ProcessedShipmentsView as
    WITH ProcessedShipments AS
    (
        SELECT ShipmentID, ShipmentType, ShipDate, Insurance, InsuranceProvider, ProcessedDate, ProcessedUserID, ProcessedComputerID,
			ProcessedWithUiMode, Voided, VoidedDate, VoidedUserID, VoidedComputerID, TotalWeight, TrackingNumber, ShipmentCost, 
			ShipSenseStatus, Shipment.ShipAddressValidationStatus, Shipment.ShipResidentialStatus, Shipment.ShipPOBox, 
			Shipment.ShipMilitaryAddress, Shipment.ShipUSTerritory, RequestedLabelFormat, ActualLabelFormat, 
			[Order].OrderID, [Order].OrderNumberComplete, [Order].CombineSplitStatus
		FROM Shipment
			INNER JOIN [Order] ON Shipment.OrderID = [Order].OrderID
        WHERE Processed = 1
    ),
    RegularShipments AS
    (
        SELECT s.ShipmentID, s.ShipmentType, s.ShipDate, s.Insurance, s.InsuranceProvider, s.ProcessedDate, s.ProcessedUserID, 
			s.ProcessedComputerID, s.ProcessedWithUiMode, s.Voided, s.VoidedDate, s.VoidedUserID, s.VoidedComputerID, s.TotalWeight, 
			s.TrackingNumber, s.ShipmentCost, s.ShipSenseStatus, s.ShipAddressValidationStatus, s.ShipResidentialStatus, s.ShipPOBox, 
			s.ShipMilitaryAddress, s.ShipUSTerritory, s.RequestedLabelFormat, s.ActualLabelFormat, s.OrderID, s.OrderNumberComplete, 
			s.CombineSplitStatus, CONVERT(NVARCHAR(50), carrierService.[Service]) AS [Service]
        FROM ProcessedShipments s
        CROSS APPLY
        (
            SELECT
                case
                    when s.ShipmentType in (0, 1) THEN (SELECT c.[Service] FROM upsshipment c WHERE c.ShipmentID = s.ShipmentID)
                    when s.ShipmentType IN (6   ) THEN (SELECT c.[Service] FROM FedExShipment c WHERE c.ShipmentID = s.ShipmentID)
                    when s.ShipmentType IN (2, 4, 9, 13, 15) THEN (SELECT c.[Service] FROM PostalShipment c WHERE c.ShipmentID = s.ShipmentID)
                    when s.ShipmentType IN (11  ) THEN (SELECT c.[Service] FROM OnTracShipment c WHERE c.ShipmentID = s.ShipmentID)
                    when s.ShipmentType IN (12  ) THEN (SELECT c.[Service] FROM iParcelShipment c WHERE c.ShipmentID = s.ShipmentID)
                    when s.ShipmentType IN (17  ) THEN (SELECT c.[Service] FROM DhlExpressShipment c WHERE c.ShipmentID = s.ShipmentID)
                    when s.ShipmentType IN (18  ) THEN (SELECT c.[Service] FROM AsendiaShipment c WHERE c.ShipmentID = s.ShipmentID)
                END AS [Service]
        ) AS carrierService
		WHERE s.ShipmentType NOT IN (5, 16)
    ),
    AmazonShipments as
    (
        SELECT s.ShipmentID, s.ShipmentType, s.ShipDate, s.Insurance, s.InsuranceProvider, s.ProcessedDate, s.ProcessedUserID, 
			s.ProcessedComputerID, s.ProcessedWithUiMode, s.Voided, s.VoidedDate, s.VoidedUserID, s.VoidedComputerID, s.TotalWeight, 
			s.TrackingNumber, s.ShipmentCost, s.ShipSenseStatus, s.ShipAddressValidationStatus, s.ShipResidentialStatus, s.ShipPOBox, 
			s.ShipMilitaryAddress, s.ShipUSTerritory, s.RequestedLabelFormat, s.ActualLabelFormat, s.OrderID, s.OrderNumberComplete, 
			s.CombineSplitStatus, c.ShippingServiceID 
		FROM AmazonShipment c, ProcessedShipments s WHERE c.ShipmentID = s.ShipmentID  AND s.ShipmentType = 16
    ),
    OtherShipments as
    (
        SELECT s.ShipmentID, s.ShipmentType, s.ShipDate, s.Insurance, s.InsuranceProvider, s.ProcessedDate, s.ProcessedUserID, 
			s.ProcessedComputerID, s.ProcessedWithUiMode, s.Voided, s.VoidedDate, s.VoidedUserID, s.VoidedComputerID, s.TotalWeight, 
			s.TrackingNumber, s.ShipmentCost, s.ShipSenseStatus, s.ShipAddressValidationStatus, s.ShipResidentialStatus, s.ShipPOBox, 
			s.ShipMilitaryAddress, s.ShipUSTerritory, s.RequestedLabelFormat, s.ActualLabelFormat, s.OrderID, s.OrderNumberComplete, 
			s.CombineSplitStatus, c.[Carrier] + ' ' + c.[Service] AS [Service]
		FROM OtherShipment c, ProcessedShipments s WHERE c.ShipmentID = s.ShipmentID AND s.ShipmentType = 5
    )
    SELECT * FROM RegularShipments
    UNION
    SELECT * FROM AmazonShipments
    UNION
    SELECT * FROM OtherShipments
GO