

-- Update the SellerVantage ModuleUrl
update GenericModuleStore set ModuleUrl = 'http://app.sellervantage.com/shipworksv3/' where StoreID in (select StoreID from Store where TypeCode = 23)

