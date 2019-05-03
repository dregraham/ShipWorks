Feature: AddWarehouse

@mytag
Scenario Outline: User adds a warehouse
	Given the user wants to add a warehouse using '<Browser>'
	Then the user clicks on the Warehouses tab
	Then the user clicks the add button
	Then the user adds the Warehouse details




	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |
	| Edge    |