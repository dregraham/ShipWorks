Feature: LogoutURLRedirectVerification


@Firefox, @Smoke
Scenario Outline: Make sure browser redirects to login page on Firefox
	Given the user is on login page on '<Browser>'
	Given the user enters username and password
	Then the user clicks logout
	Then the user validates the browser redirects to the login page from the dashboard, warehouse, settings, and warehouse add pages
	Then the user closes the browser

	Examples: 
		| Browser |
		| Firefox | 


@Chrome
Scenario Outline: Make sure browser redirects to login page on Chrome
	Given the user is on login page on '<Browser>'
	Given the user enters username and password	
	Then the user clicks logout
	Then the user validates the browser redirects to the login page from the dashboard, warehouse, settings, and warehouse add pages
	Then the user closes the browser

	Examples: 
		| Browser | 
		| Chrome  | 


@Edge
Scenario Outline: Make sure browser redirects to login page on Edge
	Given the user is on login page on '<Browser>'
	Given the user enters username and password
	Then the user clicks logout
	Then the user validates the browser redirects to the login page from the dashboard, warehouse, settings, and warehouse add pages
	Then the user closes the browser

	Examples: 
		| Browser |
		| Edge    |
		
