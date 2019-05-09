Feature: FailureLogin	

@Firefox, @Smoke
Scenario Outline: Login with invalid credentials on Firefox
	Given the user is on login page on '<Browser>'
	Given the user enters invalid username and password
	Then the user sees the error message

			Examples: 
	| Browser | 
	| Firefox | 

	@Chrome
Scenario Outline: Login with invalid credentials on Chrome
	Given the user is on login page on '<Browser>'
	Given the user enters invalid username and password
	Then the user sees the error message

			Examples: 
	| Browser | 
	| Chrome  | 

@Edge
Scenario Outline: Login with invalid credentials on Edge
	Given the user is on login page on '<Browser>'
	Given the user enters invalid username and password
	Then the user sees the error message

			Examples: 
	| Browser | 
	| Edge    | 
