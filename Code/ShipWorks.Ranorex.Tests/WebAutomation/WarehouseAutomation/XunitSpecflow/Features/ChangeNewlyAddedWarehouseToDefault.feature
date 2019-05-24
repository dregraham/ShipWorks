Feature: ChangeNewlyAddedWarehouseToDefault

@Firefox, @Smoke
Scenario Outline: User cancels edit warehouse on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user checks if the first warehouse is the default
	Then the user clicks the add button
	Then the user adds the following Warehouse details '<Name>' '<Code>' '<Street>' '<City>' '<State>' '<Zip>'
	Then the user clicks the add warehouse button
	Then the user checks if the newly added warehouse is not the default
	Then the user changes and validates the newly added warehouse is the default
	Then the makes the first warehouse the default and removes the newly added warehouse
	Then the user closes the warehouse page
		
	Examples: 
	| Browser | Name | Code   | Street           | City      | State | Zip   | Username              | Password |
	| Firefox | B    | Code 3 | 1 Memorial Drive | St. Louis | MO    | 63102 | user-9996@example.com | GOOD     |
	

	@Chrome
Scenario Outline: User cancels edit warehouse on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user checks if the first warehouse is the default
	Then the user clicks the add button
	Then the user adds the following Warehouse details '<Name>' '<Code>' '<Street>' '<City>' '<State>' '<Zip>'
	Then the user clicks the add warehouse button
	Then the user checks if the newly added warehouse is not the default
	Then the user changes and validates the newly added warehouse is the default
	Then the makes the first warehouse the default and removes the newly added warehouse
	Then the user closes the warehouse page
		
	Examples: 
	| Browser | Name | Code   | Street           | City      | State | Zip   | Username              | Password |
	| Chrome  | B    | Code 3 | 1 Memorial Drive | St. Louis | MO    | 63102 | user-9996@example.com | GOOD     |
	
	@Edge
Scenario Outline: User cancels edit warehouse on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user checks if the first warehouse is the default
	Then the user clicks the add button
	Then the user adds the following Warehouse details '<Name>' '<Code>' '<Street>' '<City>' '<State>' '<Zip>'
	Then the user clicks the add warehouse button
	Then the user checks if the newly added warehouse is not the default
	Then the user changes and validates the newly added warehouse is the default
	Then the makes the first warehouse the default and removes the newly added warehouse
	Then the user closes the warehouse page
		
	Examples: 
	| Browser | Name | Code   | Street           | City      | State | Zip   | Username              | Password |
	| Edge    | B    | Code 3 | 1 Memorial Drive | St. Louis | MO    | 63102 | user-9996@example.com | GOOD     |
	