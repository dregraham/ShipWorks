with shipments as (
select 
	'custtest' as 'AccountID', 
	s.TotalWeight,
	datename(dw, s.ShipDate) as ShipOnDay,
	s.Voided,
	s.OriginFirstName, s.OriginLastName, s.OriginCompany, s.OriginStreet1, s.OriginStreet2, s.OriginStreet3, s.OriginCity, s.OriginStateProvCode, s.OriginPostalCode, s.OriginCountryCode, s.OriginPhone, s.OriginEmail, 
	s.ShipFirstName, s.ShipLastName, s.ShipCompany, replace(s.ShipStreet1, 'fedex', 'i-Parcel') as ShipStreet1, s.ShipStreet2, s.ShipStreet3, s.ShipCity, s.ShipStateProvCode, s.ShipPostalCode, s.ShipCountryCode, s.ShipPhone, s.ShipEmail, 
	s.ReturnShipment, s.Insurance, s.InsuranceProvider, 
	case 
		when ips.[Service] <= 5 then '112'
		else '115'
	end as 'Service',
	ips.ReferenceCustomer, 
	ips.EmailNotifyRecipient as TrackByEmail, 
	ips.EmailNotifySender as TrackBySMS, 
	ips.EmailNotifyOther as IsDeliveryDutyPaid,
	COUNT(ipp.FedExPackageID) as NumberOfPackages,
	avg(ipp.[Weight]) as WeightPerPackage, 
	avg(ipp.DimsHeight) as HeightPerPackage, 
	avg(ipp.DimsLength) as LengthPerPackage, 
	avg(ipp.DimsWidth) as WidthPerPackage, 
	avg(ipp.InsuranceValue) as InsuranceValuePerPackage, 
	avg(cast(ipp.InsurancePennyOne as int)) as InsurancePennyOne, 
	avg(ipp.DeclaredValue) as DeclaredValue,
	'TRUE' as 'SaveLabel',
	RateRequestTypes = 'LIST',
	'TRUE' as 'Ship?'
FROM         
	Shipment AS s INNER JOIN
	FedExShipment AS ips ON s.ShipmentID = ips.ShipmentID INNER JOIN
	FedExAccount AS ipa ON ips.FedExAccountID = ipa.FedExAccountID LEFT OUTER JOIN
	FedExPackage AS ipp ON ips.ShipmentID = ipp.ShipmentID AND s.ShipmentID = ipp.ShipmentID

group by 
	ipa.FedExAccountID, 
	s.TotalWeight,
	datename(dw, s.ShipDate),
	s.Voided,
	s.ShipFirstName, s.ShipLastName, s.ShipCompany, s.ShipStreet1, s.ShipStreet2, s.ShipStreet3, s.ShipCity, s.ShipStateProvCode, s.ShipPostalCode, s.ShipCountryCode, s.ShipPhone, s.ShipEmail, 
	s.OriginFirstName, s.OriginLastName, s.OriginCompany, s.OriginStreet1, s.OriginStreet2, s.OriginStreet3, s.OriginCity, s.OriginStateProvCode, s.OriginPostalCode, s.OriginCountryCode, s.OriginPhone, s.OriginEmail, 
	s.ReturnShipment, s.Insurance, s.InsuranceProvider, 
	ips.[Service],
	ips.ReferenceCustomer, 
	ips.EmailNotifyRecipient, ips.EmailNotifySender, ips.EmailNotifyOther
having
	COUNT(ipp.FedExPackageID) = 1
)
select *
from shipments
where shipcountrycode in ('us', 'ca')
