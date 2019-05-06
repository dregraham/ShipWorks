Feature: CancelAddWarehouse

@NotEdge
Scenario Outline: User cancels add warehouse
	Given the user wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user clicks the cancel button
	
	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |


@Edge
Scenario Outline: User cancels add warehouse for edge
	Given the user wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the add button
	Then the user clicks the cancel button
	
	Examples: 
	| Browser |
	| Edge    |