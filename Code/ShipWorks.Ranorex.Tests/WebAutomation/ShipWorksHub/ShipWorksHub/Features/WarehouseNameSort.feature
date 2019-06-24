Feature: WarehouseNameSort

@Firefox, @Smoke
Scenario Outline: The user checks the sorting of the warehouse names on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user gets the list of all the warehouses
	Then the user clicks the add button
	Then the user adds a warehouse to be sorted
	Then the user clicks the add warehouse button
	Then the user verifies that they are back on the settings page
	Then the user verifies that the warehouses are sorted correctly
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Firefox | user-9998@example.com | GOOD     |

@Chrome
Scenario Outline: The user checks the sorting of the warehouse names on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user gets the list of all the warehouses
	Then the user clicks the add button
	Then the user adds a warehouse to be sorted
	Then the user clicks the add warehouse button
	Then the user verifies that they are back on the settings page
	Then the user verifies that the warehouses are sorted correctly
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Chrome  | user-9996@example.com | GOOD     |

@Edge
Scenario Outline: The user checks the sorting of the warehouse names on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user gets the list of all the warehouses
	Then the user clicks the add button
	Then the user adds a warehouse to be sorted
	Then the user clicks the add warehouse button
	Then the user verifies that they are back on the settings page
	Then the user verifies that the warehouses are sorted correctly
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Edge    | user-9994@example.com | GOOD     |