Feature: CancelAddWarehouse

@Firefox, @Smoke
Scenario Outline: User cancels add warehouse for Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user clicks the cancel button
	Then the user verifies that no warehouse was added
	Then the user closes the warehouse page
	
		Examples: 
	| Browser | Username              | Password |
	| Firefox | user-0801@example.com | GOOD     |

	@Chrome
Scenario Outline: User cancels add warehouse for Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user clicks the cancel button
	Then the user verifies that no warehouse was added
	Then the user closes the warehouse page
	
	Examples: 
	| Browser | Username              | Password |
	| Chrome  | user-0801@example.com | GOOD     |

@Edge
Scenario Outline: User cancels add warehouse for Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user clicks the cancel button
	Then the user verifies that no warehouse was added
	Then the user closes the warehouse page
	
		Examples: 
	| Browser | Username              | Password |
	| Edge    | user-0801@example.com | GOOD     |
