Feature: FieldLengthValidation

@NotEdge
Scenario Outline: User validates max of 500 characters in fields
	Given the user wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds more than five hundred characters
	And the user sees the field validation error messages 

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	

@Edge
Scenario Outline: User validates max of 500 characters in fields for edge
	Given the user wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds more than five hundred characters
	And the user sees the field validation error messages 

	Examples: 
	| Browser |
	| Edge    |