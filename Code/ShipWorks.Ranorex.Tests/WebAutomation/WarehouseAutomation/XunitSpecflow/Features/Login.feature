Feature: Login	

Scenario Outline: Login with valid credentials
	Given the user is on login page on '<Browser>'
	And the user enters username and password	
	Then the user sees the dashboard

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	| Edge    |
