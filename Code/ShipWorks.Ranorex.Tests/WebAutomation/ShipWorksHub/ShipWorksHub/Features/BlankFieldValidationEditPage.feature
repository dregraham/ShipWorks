Feature: BlankFieldLengthValidationEditPage

@Firefox, @Smoke
Scenario Outline: User validates blank fields on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user blanks out all fields on edit warehouse page
	Then the user clicks the save button
	And the user sees empty field error messages on edit warehouse page
	Then the user closes the warehouse page


		Examples: 
	| Browser | Username              | Password |
	| Firefox | user-0801@example.com | GOOD     |

	@Chrome
Scenario Outline: User validates blank fields on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user blanks out all fields on edit warehouse page
	Then the user clicks the save button
	And the user sees empty field error messages on edit warehouse page
	Then the user closes the warehouse page


		Examples: 
	| Browser | Username              | Password |
	| Chrome  | user-0801@example.com | GOOD     |


@Edge
Scenario Outline: User validates blank fields on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user blanks out all fields on edit warehouse page
	Then the user clicks the save button
	And the user sees empty field error messages on edit warehouse page
	Then the user closes the warehouse page


	Examples: 
	| Browser | Username              | Password |
	| Edge    | user-0801@example.com | GOOD     |
