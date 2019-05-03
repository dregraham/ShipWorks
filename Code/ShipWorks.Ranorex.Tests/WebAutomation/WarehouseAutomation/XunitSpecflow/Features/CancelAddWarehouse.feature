Feature: CancelAddWarehouse

@mytag
Scenario Outline: User cancels add warehouse
	Given the user wants to add a warehouse using '<Browser>'
	Then the user clicks on the Warehouses tab
	Then the user clicks the add button
	Then the user clicks the cancel button
	
	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	| Edge    |