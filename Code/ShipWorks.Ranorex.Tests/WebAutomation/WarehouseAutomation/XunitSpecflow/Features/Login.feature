Feature: Login	

@NotEdge
Scenario Outline: Login with valid credentials
	Given the user is on login page on '<Browser>'
	And the user enters username and password	
	Then the user sees the dashboard

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	

@NotEdge
Scenario Outline: Logout
	Given the user is on login page on '<Browser>'
	And the user enters username and password	
	Then the user sees the dashboard
	Then the user clicks logout

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |	


@Edge
Scenario Outline: Login with valid credentials for edge
	Given the user is on login page on '<Browser>'
	And the user enters username and password	
	Then the user sees the dashboard

	Examples: 
	| Browser |
	| Edge    |

@Edge
Scenario Outline: Logout for edge
	Given the user is on login page on '<Browser>'
	And the user enters username and password	
	Then the user sees the dashboard
	Then the user clicks logout

	Examples: 
	| Browser |
	| Edge    |
