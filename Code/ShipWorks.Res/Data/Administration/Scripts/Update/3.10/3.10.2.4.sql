UPDATE ShippingDefaultsRule
	SET ShipmentType = ShippingProfile.ShipmentType
	FROM ShippingProfile
	WHERE ShippingDefaultsRule.ShippingProfileID = ShippingProfile.ShippingProfileID
		AND ShippingDefaultsRule.ShipmentType = 15
		AND ShippingDefaultsRule.ShipmentType <> ShippingProfile.ShipmentType