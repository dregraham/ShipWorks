Feature: RemoveWarehouse

@Chrome
Scenario Outline: User removes a warehouse for Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the remove button
	Then the user accepts the remove warehouse confirmation
	#We will want a success message to verify a warehouse has been removed

	Examples: 
		| Browser | Username              | Password |
		| Chrome  | user-0801@example.com | GOOD     |


	@Firefox, @Smoke
Scenario Outline: User removes a warehouse for Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the remove button
	Then the user accepts the remove warehouse confirmation
	#We will want a success message to verify a warehouse has been removed

	Examples: 
		| Browser | Username              | Password |
		| Firefox | user-0801@example.com | GOOD     |

@Edge
Scenario Outline: User removes a warehouse for Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the remove button
	Then the user accepts the remove warehouse confirmation
	#We will want a success message to verify a warehouse has been removed

	Examples: 
		| Browser | Username              | Password |
		| Edge    | user-0801@example.com | GOOD     |
