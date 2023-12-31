-- Update HidePostage to be true for default Stamps profiles if the carrier is NOT configured
declare @configuredCarriers nvarchar(100)
select @configuredCarriers = ',' + Configured + ',' from ShippingSettings

-- Stamps
update StampsProfile set HidePostage = 1
	where ShippingProfileID in
	(
		select ShippingProfileID 
		from ShippingProfile
		where ShipmentTypePrimary = 1 
		  and ShipmentType = 3
		  and charindex(',3,', @configuredCarriers) = 0
	)
	
-- USPS
update StampsProfile set HidePostage = 1
	where ShippingProfileID in
	(
		select ShippingProfileID 
		from ShippingProfile
		where ShipmentTypePrimary = 1 
		  and ShipmentType = 15
		  and charindex(',15,', @configuredCarriers) = 0
	)
	
-- Express1 Stamps
update StampsProfile set HidePostage = 1
	where ShippingProfileID in
	(
		select ShippingProfileID 
		from ShippingProfile
		where ShipmentTypePrimary = 1 
		  and ShipmentType = 13
		  and charindex(',13,', @configuredCarriers) = 0
	)
	

-- Update HidePostage to be true for unprocessed shipments if the carrier is NOT configured
-- Stamps
update StampsShipment set HidePostage = 1
	where ShipmentID in
	(
		select ShipmentID 
		from Shipment
		where ShipmentType = 3
		  and Processed = 0
		  and charindex(',3,', @configuredCarriers) = 0
	)
	
-- USPS
update StampsShipment set HidePostage = 1
	where ShipmentID in
	(
		select ShipmentID 
		from Shipment
		where ShipmentType = 15
		  and Processed = 0
		  and charindex(',15,', @configuredCarriers) = 0
	)

-- Express1 Stamps
update StampsShipment set HidePostage = 1
	where ShipmentID in
	(
		select ShipmentID 
		from Shipment
		where ShipmentType = 13
		  and Processed = 0
		  and charindex(',13,', @configuredCarriers) = 0
	)

