Feature: HubOrderCount	

@Chrome
Scenario Outline: Login to Hub and check number of Orders
	Given the following user with '<Username>' and '<Password>' wants to navigate to the Hub login page using '<Browser>'
	Then the user navigates to the orders page
	Then the user gets the number of orders

	Examples: 
	| Browser | Username             | Password  |
	| Chrome  | user-212@example.com | password1 |