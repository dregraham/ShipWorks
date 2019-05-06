Feature: EditWarehouse

@mytag
Scenario Outline: User edits a warehouse
	Given the user wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the edit button
	Then the user enters new details
	Then the user saves the page
	#Given the user are on the warehouse list page
	#Then the user verifies the details


	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	| Edge    |