Feature: WarehouseNotFound

@Firefox, @Smoke
Scenario Outline: Navigate to a nonexistent warehouse on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user navigates to a nonexistent warehouse
	Then the user checks to see if they are on the warehouse not found page
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Firefox | user-0801@example.com | GOOD     |

@Chrome
Scenario Outline: Navigate to a nonexistent warehouse on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user navigates to a nonexistent warehouse
	Then the user checks to see if they are on the warehouse not found page
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Chrome  | user-0801@example.com | GOOD     |

@Edge
Scenario Outline: Navigate to a nonexistent warehouse on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user navigates to a nonexistent warehouse
	Then the user checks to see if they are on the warehouse not found page
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Edge    | user-0801@example.com | GOOD     |