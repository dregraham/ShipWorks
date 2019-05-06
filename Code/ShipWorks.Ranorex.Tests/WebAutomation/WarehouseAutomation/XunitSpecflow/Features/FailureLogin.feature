﻿Feature: FailureLogin	

@NotEdge
Scenario Outline: Login with invalid credentials
	Given the user is on login page on '<Browser>'
	And the user enters invalid username and password	
	Then the user sees the error message

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	| Edge    |

@Edge
Scenario Outline: Login with invalid credentials for edge
	Given the user is on login page on '<Browser>'
	And the user enters invalid username and password	
	Then the user sees the error message

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	| Edge    |