Feature: WarehouseNameSort

@Firefox, @Smoke
Scenario Outline: The user checks the sorting of the warehouse names on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds the Warehouse details for name sorting
	Then the user checks the sorting of the warehouse
	Then the user deletes all but the first warehouse
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Firefox | user-9995@example.com | GOOD     |

@Chrome
Scenario Outline: The user checks the sorting of the warehouse names on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds the Warehouse details for name sorting
	Then the user checks the sorting of the warehouse
	Then the user deletes all but the first warehouse
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Chrome  | user-9995@example.com | GOOD     |

@Edge
Scenario Outline: The user checks the sorting of the warehouse names on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds the Warehouse details for name sorting
	Then the user checks the sorting of the warehouse
	Then the user deletes all but the first warehouse
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Edge    | user-9995@example.com | GOOD     |