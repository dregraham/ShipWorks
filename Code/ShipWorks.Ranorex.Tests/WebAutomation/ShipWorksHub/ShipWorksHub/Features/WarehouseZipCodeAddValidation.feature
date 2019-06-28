Feature: WarehouseZipCodeAddValidation

@Firefox, @Smoke
Scenario Outline: User validates add zip code on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds the following Warehouse details '<Street>' '<City>' '<State>' '<Zip>'
	Then the user clicks the add warehouse button
	Then the user verifies that they are back on the settings page
	Then the user closes the warehouse page

	Examples:
		| Browser | Street           | City      | State | Zip        | Username              | Password |
		| Firefox | 1 Memorial Drive | St. Louis | MO    | 63102      | user-0801@example.com | GOOD     |
		| Firefox | 1 Memorial Drive | St. Louis | MO    | 63102-3410 | user-0801@example.com | GOOD     |
		| Firefox | 1 Memorial Drive | St. Louis | MO    | 631023410  | user-0801@example.com | GOOD     |

@Chrome
Scenario Outline: User validates add zip code on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds the following Warehouse details '<Street>' '<City>' '<State>' '<Zip>'
	Then the user clicks the add warehouse button
	Then the user verifies that they are back on the settings page
	Then the user closes the warehouse page

	Examples:
		| Browser | Street           | City      | State | Zip        | Username              | Password |
		| Chrome  | 1 Memorial Drive | St. Louis | MO    | 63102      | user-0801@example.com | GOOD     |
		| Chrome  | 1 Memorial Drive | St. Louis | MO    | 63102-3410 | user-0801@example.com | GOOD     |
		| Chrome  | 1 Memorial Drive | St. Louis | MO    | 631023410  | user-0801@example.com | GOOD     |

@Edge
Scenario Outline: User validates add zip code on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds the following Warehouse details '<Street>' '<City>' '<State>' '<Zip>'
	Then the user clicks the add warehouse button
	Then the user verifies that they are back on the settings page
	Then the user closes the warehouse page

	Examples:
		| Browser | Street           | City      | State | Zip        | Username              | Password |
		| Edge    | 1 Memorial Drive | St. Louis | MO    | 63102      | user-0801@example.com | GOOD     |
		| Edge    | 1 Memorial Drive | St. Louis | MO    | 63102-3410 | user-0801@example.com | GOOD     |
		| Edge    | 1 Memorial Drive | St. Louis | MO    | 631023410  | user-0801@example.com | GOOD     |