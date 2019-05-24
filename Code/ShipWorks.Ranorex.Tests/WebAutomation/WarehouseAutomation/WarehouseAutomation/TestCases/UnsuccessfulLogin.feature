Feature: UnsuccessfulLogin
	If an  incorrect username and password are entered, the user should not log in

@mytag
Scenario Outline: the login for the user is unsuccessful 
	Given the user is on warehouse login page using the web browser '<Browser>'
	When the user enters a bad username
	When the user enters a bad password
	When the user clicks login
	Then the user sees an error message that their credentials are invalid

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	| Edge    |
