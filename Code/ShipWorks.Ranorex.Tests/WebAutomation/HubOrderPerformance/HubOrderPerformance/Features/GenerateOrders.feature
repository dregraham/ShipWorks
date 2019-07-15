Feature: GenerateOrders	

@Chrome
Scenario Outline: Login to the Fake Stores site and generate orders
	Given the following user with '<Username>' and '<Password>' wants to navigate to the fake stores login page using '<Browser>'
	Then the user navigates to the generate page at '<StoreURL>'
	Then the user generates '<Number>' of orders 

	Examples: 
	| Browser | Username | Password | Number | StoreURL                                                 |
	| Chrome  | gdeblois | bar7458  | 5     | http://localhost:4004/ui/stores/10/julysecond-1/generate |