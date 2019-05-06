Feature: RemoveWarehouse

@NotEdge
Scenario Outline: User removes a warehouse
	Given the user wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the remove button

	Examples: 
	| Browser |
	| Chrome  |
	| Firefox |


@Edge
Scenario Outline: User removes a warehouse for edge
	Given the user wants to navigate to the warehouse page using '<Browser>'
	Then the user clicks the remove button

	Examples: 
	| Browser |
	| Edge    |