Feature: EditWarehouse

@Firefox, @Smoke
Scenario Outline: User edits a warehouse on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user enters new details
	Then the user clicks the save button
	#Given the user are on the warehouse list page
	#Then the user verifies the details
	Then the user closes the warehouse page


	Examples: 
	| Browser | Username              | Password |
	| Firefox | user-0801@example.com | GOOD     |

	@Chrome
Scenario Outline: User edits a warehouse on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user enters new details
	Then the user clicks the save button
	#Given the user are on the warehouse list page
	#Then the user verifies the details
	Then the user closes the warehouse page


		Examples: 
	| Browser | Username              | Password |
	| Chrome  | user-0801@example.com | GOOD     |


@Edge
Scenario Outline: User edits a warehouse on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user enters new details
	Then the user clicks the save button
	#Given the user are on the warehouse list page
	#Then the user verifies the details
	Then the user closes the warehouse page


	Examples: 
	| Browser | Username              | Password |
	| Edge    | user-0801@example.com | GOOD     |
