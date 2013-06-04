insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('January', 0, NULL, '<filter />')
insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('ShipLast A-G', 0, NULL, '<filter />')
insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('US', 0, NULL, '<filter />')
insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('ShipLast Kan', 0, NULL, '<filter />')
insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('2007', 0, NULL, '<filter />')
insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('BillPostal 6', 0, NULL, '<filter />')
insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('BillPostal 1-8', 0, NULL, '<filter />')

insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('ParentNoRestriction', 0, NULL, NULL)
insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('ParentOnlyFebuary', 0, NULL, '<filter />')
insert into Filter (Name, AppliesTo, UserID, Definition) VALUES ('ParentShipLastB-D', 0, NULL, '<filter />')

insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = 'January'),      0 FROM [Order] where 1 = DATEPART(Month, OrderDate)
insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = 'ShipLast A-G'), 0 FROM [Order] where left(ShipLastName, 1) IN ('A', 'B', 'C', 'D', 'E', 'F', 'G')
insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = 'US'),           0 FROM [Order] where BillCountryCode = 'US'
insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = 'ShipLast Kan'), 0 FROM [Order] where ShipLastName LIKE 'Kan%'
insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = '2007'),         0 FROM [Order] where 2007 = DATEPART(Year, OrderDate)
insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = 'BillPostal 6'), 0 FROM [Order] where BillPostalCode LIKE '6%'
insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = 'BillPostal 1-8'), 0 FROM [Order] where left(BillPostalCode, 1) in ('1', '2', '3', '4', '5', '6', '7', '8')

insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = 'ParentOnlyFebuary'),      0 FROM [Order] where 2 = DATEPART(Month, OrderDate)
insert into [FilterOrderMatch] (OrderID, FilterDefinitionID, IsDirty) SELECT OrderID, (SELECT FilterID FROM Filter WHERE Name = 'ParentShipLastB-D'), 0 FROM [Order] where left(ShipLastName, 1) IN ('B', 'C', 'D')

insert into FilterChild (ParentFilterID, ChildFilterID, Position)
VALUES (
   (SELECT FilterID FROM Filter WHERE Name = 'ParentNoRestriction'),
   (SELECT FilterID FROM Filter WHERE Name = 'January'),
   0)

insert into FilterChild (ParentFilterID, ChildFilterID, Position)
VALUES (
   (SELECT FilterID FROM Filter WHERE Name = 'ParentNoRestriction'),
   (SELECT FilterID FROM Filter WHERE Name = 'BillPostal 6'),
   1)

insert into FilterChild (ParentFilterID, ChildFilterID, Position)
VALUES (
   (SELECT FilterID FROM Filter WHERE Name = 'ParentOnlyFebuary'),
   (SELECT FilterID FROM Filter WHERE Name = 'BillPostal 6'),
   0)

insert into FilterChild (ParentFilterID, ChildFilterID, Position)
VALUES (
   (SELECT FilterID FROM Filter WHERE Name = 'ParentShipLastB-D'),
   (SELECT FilterID FROM Filter WHERE Name = 'ParentOnlyFebuary'),
   0)

insert into FilterChild (ParentFilterID, ChildFilterID, Position)
VALUES (
   (SELECT FilterID FROM Filter WHERE Name = 'ParentShipLastB-D'),
   (SELECT FilterID FROM Filter WHERE Name = 'January'),
   1)

insert into FilterNode (ParentFilterNodeID, FilterID, CurrentCount, IsDirty) SELECT NULL, FilterID, 0, 0 FROM Filter
