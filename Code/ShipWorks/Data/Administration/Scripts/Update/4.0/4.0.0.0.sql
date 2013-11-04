-- Add Best Rate Shipment Type to the activated and configured shipment types
update ShippingSettings 
	set Activated = 
		  CASE 
			 WHEN len(Activated) > 0 THEN Activated + ',14'
			 ELSE '14'
		  END,
	Configured = 
		  CASE 
			 WHEN len(Configured) > 0 THEN Configured + ',14'
			 ELSE '14'
		  END