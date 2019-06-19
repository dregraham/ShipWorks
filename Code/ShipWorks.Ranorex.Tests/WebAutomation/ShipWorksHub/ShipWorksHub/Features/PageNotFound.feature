Feature: PageNotFound

@Firefox, @Smoke
Scenario Outline: Navigate to a nonexistent page on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user navigates to a nonexistent page
	Then the user verifies they are on the not found page
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Firefox | user-0801@example.com | GOOD     |

@Chrome
Scenario Outline: Navigate to a nonexistent page on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user navigates to a nonexistent page
	Then the user verifies they are on the not found page
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Chrome  | user-0801@example.com | GOOD     |

@Edge
Scenario Outline: Navigate to a nonexistent page on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user navigates to a nonexistent page
	Then the user verifies they are on the not found page
	Then the user closes the warehouse page

	Examples:
		| Browser | Username              | Password |
		| Edge    | user-0801@example.com | GOOD     |