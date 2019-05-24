Feature: WarehouseDeleteConfirmation

@Chrome
Scenario Outline: User cancels remove warehouse action on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the remove button
	Then the user cancels the remove warehouse action 
	Then the user checks to see if the warehouse is still on the list
	Then the user closes the warehouse page

	Examples: 
	| Browser | Username              | Password |
	| Chrome  | user-9997@example.com | GOOD     |


	@Firefox, @Smoke
Scenario Outline: User cancels remove warehouse action on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the remove button
	Then the user cancels the remove warehouse action 
	Then the user checks to see if the warehouse is still on the list
	Then the user closes the warehouse page

	Examples: 
	| Browser | Username              | Password |
	| Firefox | user-9997@example.com | GOOD     |


@Edge
Scenario Outline: User cancels remove warehouse action on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the remove button
	Then the user cancels the remove warehouse action 
	Then the user checks to see if the warehouse is still on the list
	Then the user closes the warehouse page

	Examples: 
	| Browser | Username              | Password |
	| Edge    | user-9997@example.com | GOOD     |
