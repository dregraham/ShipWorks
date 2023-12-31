﻿Feature: Logout

@Firefox, @Smoke
Scenario Outline: Logout on Firefox
	Given the user is on login page on '<Browser>'
	Given the user enters username and password
	Then the user clicks logout
	Then the user closes the browser

	Examples:
		| Browser |
		| Firefox |

@Chrome
Scenario Outline: Logout on Chrome
	Given the user is on login page on '<Browser>'
	Given the user enters username and password
	Then the user clicks logout
	Then the user closes the browser

	Examples:
		| Browser |
		| Chrome  |

@Edge
Scenario Outline: Logout on Edge
	Given the user is on login page on '<Browser>'
	Given the user enters username and password
	Then the user clicks logout
	Then the user closes the browser

	Examples:
		| Browser |
		| Edge    |