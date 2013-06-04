update [order]
 set BillFirstName = 'Whatever',
     BillLastName = 'Cool',
     BillCity = 'Dont Know',
     BillStateProvCode = 'Whatever',
     BillPostalCode = 'Still',
     ShipLastName = 'Something',
     BillStreet3 = 'DontKnow',
     ShipCity = 'Dont Know',
     ShipStateProvCode = 'Whatever',
     ShipPostalCode = 'Still'
where orderid = ((ABS(CHECKSUM(NEWID())) % 100000) * 1000) + 6