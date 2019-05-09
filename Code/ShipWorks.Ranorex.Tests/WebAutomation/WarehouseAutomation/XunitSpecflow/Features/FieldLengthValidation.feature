Feature: FieldLengthValidation

@Firefox, @Smoke
Scenario Outline: User validates max of 500 characters in fields on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds more than five hundred characters
	And the user sees the field validation error messages 

	Examples: 
	| Browser | Username              | Password |
	| Firefox | user-0801@example.com | GOOD     |

	@Chrome
Scenario Outline: User validates max of 500 characters in fields on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds more than five hundred characters
	And the user sees the field validation error messages 

	Examples: 
	| Browser | Username              | Password |
	| Chrome  | user-0801@example.com | GOOD     |
	

@Edge
Scenario Outline: User validates max of 500 characters in fields on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds more than five hundred characters
	And the user sees the field validation error messages 

	Examples: 
	| Browser | Username              | Password |
	| Edge    | user-0801@example.com | GOOD     |
