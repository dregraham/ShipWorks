PRINT N'Removing invalid shipping rules'
GO

select r.ShippingDefaultsRuleID
	into #RulesToDelete
	from ShippingProfile p, ShippingDefaultsRule r
	where p.ShippingProfileID = r.ShippingProfileID
		and r.ShipmentType != p.ShipmentType
	order by r.ShippingDefaultsRuleID, r.ShipmentType, p.ShipmentType, p.ShipmentTypePrimary

DELETE ObjectReference WHERE ReferenceKey in ('FilterNodeID', 'ShippingProfile') 
	AND ConsumerID in 
	(
		select ShippingDefaultsRuleID from #RulesToDelete
	)

DELETE FROM ShippingDefaultsRule WHERE ShippingDefaultsRuleID in 
	(
		select ShippingDefaultsRuleID from #RulesToDelete
	)
GO
