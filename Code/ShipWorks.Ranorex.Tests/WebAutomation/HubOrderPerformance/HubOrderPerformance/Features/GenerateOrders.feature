Feature: GenerateOrders

@Chrome
Scenario Outline: Login to the Fake Stores site and generate orders
	Given the following user with '<Username>' and '<Password>' wants to navigate to the fake stores login page using '<Browser>'
	Then the user navigates to the generate page at '<StoreURL>'
	Then the user generates '<BatchSize>' number of orders for '<BatchIteration>' number of batches

	Examples:
		| Browser | Username | Password | BatchSize | BatchIteration | StoreURL                                                                                   |
		| Chrome  | gdeblois | bar7458  | 834       | 16              | https://master.fake-stores.warehouseapp.link/ui/stores/28/garrettgm11-admin-admin/generate |