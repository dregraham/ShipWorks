Feature: CancelEditWarehouse

@Firefox, @Smoke
Scenario Outline: User cancels edit warehouse for Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user clicks the cancel button on edit warehouse page
	Then the user verifies that no fields were updated
	Then the user closes the warehouse page
	
	Examples: 
	| Username              | Password | Browser |
	| user-9997@example.com | GOOD     | Firefox | 

	@Chrome
Scenario Outline: User cancels edit warehouse for Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user clicks the cancel button on edit warehouse page
	Then the user verifies that no fields were updated
	Then the user closes the warehouse page
	
	Examples: 
	| Username              | Password | Browser |
	| user-9997@example.com | GOOD     | Chrome  |

@Edge
Scenario Outline: User cancels edit warehouse for Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user clicks the cancel button on edit warehouse page
	Then the user verifies that no fields were updated
	Then the user closes the warehouse page
	
	Examples: 
	| Username              | Password | Browser |
	| user-9997@example.com | GOOD     | Edge    |
