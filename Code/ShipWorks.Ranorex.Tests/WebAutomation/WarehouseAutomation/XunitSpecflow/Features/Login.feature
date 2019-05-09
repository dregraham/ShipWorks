Feature: Login	

@Firefox, @Smoke
Scenario Outline: Login with valid credentials on Firefox
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'

	Examples: 
		| Browser | Username              | Password |
		| Firefox | user-0801@example.com | GOOD     |
	

	@Chrome
Scenario Outline: Login with valid credentials on Chrome
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'

	Examples: 
		| Browser | Username              | Password |
		| Chrome  | user-0801@example.com | GOOD     |
	


@Edge
Scenario Outline: Login with valid credentials on Edge
	Given the following user with '<Username>' and '<Password>' wants to navigate to the warehouse page using '<Browser>'

	Examples: 
		| Browser | Username              | Password |
		| Edge    | user-0801@example.com | GOOD     |

