Feature: SuccessfulLogin
	If a correct username and password are entered, the user should successfully log in

@mytag
Scenario Outline: the user logs in successfully to the warehouse
	Given the user is on warehouse login page using the web browser '<Browser>'
	When the user enters username
	When the user enters password
	When the user clicks login
	Then the user sees the home page

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	| Edge    |
