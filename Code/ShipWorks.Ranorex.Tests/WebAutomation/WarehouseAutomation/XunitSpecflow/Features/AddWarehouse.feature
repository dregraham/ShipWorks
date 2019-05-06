Feature: AddWarehouse

@mytag
Scenario Outline: User adds a warehouse
	Given the user wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user adds the Warehouse details


	Examples: 
	| Browser |
	#| Chrome  |
	#| Firefox |
	| Edge    |