SET NOCOUNT ON
SELECT QUOTENAME(s.StoreName, '"') as 'Store Name',
       QUOTENAME(o.OrderNumberComplete, '"') as 'Order Number',
       QUOTENAME(CONVERT(varchar(50), o.OrderDate, 126) + 'Z', '"') as 'Order Date',
       QUOTENAME(o.ShipUnparsedName, '"') as 'Ship Name',
       QUOTENAME(o.ShipCompany, '"') as 'Ship Company',
       QUOTENAME(o.ShipPostalCode, '"') as 'Ship Postal Code',
       QUOTENAME(o.ShipCountryCode, '"') as 'Ship Country',
       QUOTENAME(CONVERT(varchar(50), ship.ProcessedDate, 126) + 'Z', '"') as 'Ship Date',
       QUOTENAME(shipType.Name, '"') as 'Carrier',
       QUOTENAME(ship.ShipmentCost, '"') as 'Carrier Fee',
       QUOTENAME(COALESCE(ST.Name, OS.Service, ASFPS.ShippingServiceName), '"') as 'Shipping Service',
       QUOTENAME(ship.TrackingNumber, '"') as 'Tracking Number',
       QUOTENAME(CONVERT(varchar(50), ship.ProcessedDate, 126) + 'Z', '"') as 'Label Create Date',
       QUOTENAME(CAST(ROUND(ship.TotalWeight * 16, 2) AS Decimal(22, 2)), '"') as 'Weight',
       -- This was originally called Weight Oz.
       QUOTENAME(CAST(ROUND(ship.BilledWeight * 16, 2) AS Decimal(22, 2)), '"') as 'Billed Weight',
       -- This was originally the same value as 'Weight'
       QUOTENAME(COALESCE(UP.DimsWidth, PS.DimsWidth, FP.DimsWidth, OTS.DimsWidth, IPP.DimsWidth, ASFPS.DimsWidth,
                          DP.DimsWidth, ASO.DimsWidth, ASWAS.DimsWidth, 0), '"') as 'Package Width',
       QUOTENAME(COALESCE(UP.DimsHeight, PS.DimsHeight, FP.DimsHeight, OTS.DimsHeight, IPP.DimsHeight, ASFPS.DimsHeight,
                          DP.DimsHeight, ASO.DimsHeight, ASWAS.DimsHeight, 0), '"') as 'Package Height',
       QUOTENAME(COALESCE(UP.DimsLength, PS.DimsLength, FP.DimsLength, OTS.DimsLength, IPP.DimsLength, ASFPS.DimsLength,
                          DP.DimsLength, ASO.DimsLength, ASWAS.DimsLength, 0), '"') as 'Package Length',
       QUOTENAME(COALESCE(PT.Name, 'PKG'), '"') AS 'Package Type',
       -- The template always used 'PKG' so PK may not care about this field
       QUOTENAME((CASE
                      WHEN ship.Insurance = 0 THEN 'None'
                      WHEN ship.InsuranceProvider = 1 THEN 'ShipWorks'
                      WHEN ship.InsuranceProvider = 2 THEN 'Carrier'
                      ELSE 'Unknown'
           END), '"') as 'Insurance Provider'
from [Order] o
         inner join Store s on o.StoreID = s.StoreID
         inner join Shipment ship on o.OrderID = ship.OrderID
         inner join ShipmentType shipType on shipType.ShipmentTypeID = ship.ShipmentType
         left outer join UpsShipment US on ship.ShipmentID = US.ShipmentID
         left outer join PostalShipment PS on ship.ShipmentID = PS.ShipmentID
         left outer join OtherShipment OS on ship.ShipmentID = OS.ShipmentID
         left outer join FedExShipment FS on ship.ShipmentID = FS.ShipmentID
         left outer join OnTracShipment OTS on ship.ShipmentID = OTS.ShipmentID
         left outer join iParcelShipment IPS on ship.ShipmentID = IPS.ShipmentID
         left outer join AmazonSFPShipment ASFPS ON ship.ShipmentID = ASFPS.ShipmentID
         left outer join DhlExpressShipment DS ON ship.ShipmentID = DS.ShipmentID
         left outer join AsendiaShipment ASO on ship.ShipmentID = ASO.ShipmentID
         left outer join AmazonSWAShipment ASWAS on ship.ShipmentID = ASWAS.ShipmentID
         left outer join UpsPackage UP on ship.ShipmentID = UP.ShipmentID
         left outer join FedExPackage FP on ship.ShipmentID = FP.ShipmentID
         left outer join iParcelPackage IPP on ship.ShipmentID = IPP.ShipmentID
         left outer join DhlExpressPackage DP on ship.ShipmentID = DP.ShipmentID
         left outer join ServiceType ST on ST.ShipmentTypeID = ship.ShipmentType
    AND ST.ServiceTypeID = COALESCE(
            US.Service,
            PS.Service,
            FS.Service,
            OTS.Service,
            IPS.Service,
            DS.Service,
            ASO.Service,
            ASWAS.Service
        )
         left outer join PackagingType PT on PT.ShipmentTypeID = ship.ShipmentType
    AND PT.PackagingTypeID = COALESCE(
            UP.PackagingType,
            PS.PackagingType,
            FS.PackagingType,
            OTS.PackagingType
        )
where ship.Processed = 1
  and ship.Voided = 0
  and s.StoreID <> 79005 -- Thracian Trading
  and ship.ShipDate >= DATEADD(day, -4, CAST(GETDATE() AS date))
  AND ship.ShipDate <= DATEADD(day, -3, CAST(GETDATE() AS date))