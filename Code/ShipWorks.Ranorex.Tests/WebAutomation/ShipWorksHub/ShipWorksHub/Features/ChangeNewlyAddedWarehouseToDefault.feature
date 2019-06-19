Feature: ChangeNewlyAddedWarehouseToDefault

@Firefox, @Smoke
Scenario Outline: change newly added warehouse to default for Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user gets the list of all the warehouses
	Then the user checks if the first warehouse is the default
	Then the user clicks the add button
	Then the user adds a warehouse to be sorted
	Then the user clicks the add warehouse button	
	Then the user checks if the newly added warehouse is not the default
	Then the user changes and validates the newly added warehouse is the default
	Then the makes the first warehouse the default and removes the newly added warehouse
	Then the user closes the warehouse page
		
	Examples: 
	| Browser | Username              | Password |
	| Firefox | user-9990@example.com | GOOD     |
	

	@Chrome
Scenario Outline: change newly added warehouse to default for Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user gets the list of all the warehouses
	Then the user checks if the first warehouse is the default
	Then the user clicks the add button
	Then the user adds a warehouse to be sorted
	Then the user clicks the add warehouse button	
	Then the user checks if the newly added warehouse is not the default
	Then the user changes and validates the newly added warehouse is the default
	Then the makes the first warehouse the default and removes the newly added warehouse
	Then the user closes the warehouse page
		
	Examples: 
	| Browser | Username              | Password |
	| Chrome  | user-9993@example.com | GOOD     |
	
	@Edge
Scenario Outline: change newly added warehouse to default for Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user gets the list of all the warehouses
	Then the user checks if the first warehouse is the default
	Then the user clicks the add button
	Then the user adds a warehouse to be sorted
	Then the user clicks the add warehouse button	
	Then the user checks if the newly added warehouse is not the default
	Then the user changes and validates the newly added warehouse is the default
	Then the makes the first warehouse the default and removes the newly added warehouse
	Then the user closes the warehouse page
		
	Examples: 
	| Browser | Username              | Password |
	| Edge    | user-9989@example.com | GOOD     |
	