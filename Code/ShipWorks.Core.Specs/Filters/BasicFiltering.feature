@collection:database
Feature: BasicFiltering
	In order to see orders that are relevant to me
	As a customer
	I want to filter the list of orders

Scenario: Provider rule is applied when order is in filter
	Given a filter named "Rule Filter" with an order number condition that BeginsWith 567
	And a provider rule for Other for filter "Rule Filter"
	And a default provider of None
	And the following orders
		| Name   | Number |
		| First  | 1234   |
		| Second | 5678   |
	When a shipment is created for each order
	Then the shipment for order 5678 should use provider Other
	And the shipment for order 1234 should use provider None

Scenario: Shipping rule is applied when order is in filter
	Given the following "Other" profiles
		| Name             | Default | Carrier |
		| Defaults - Other | Yes     | Foo     |
		| For Rule         | No      | Bar     |
	And a filter named "Rule Filter" with an order number condition that BeginsWith 567
	And a shipping rule for "Other" that applies "For Rule" for filter "Rule Filter"
	And a default provider of Other
	And the following orders
		| Name   | Number |
		| First  | 1234   |
		| Second | 5678   |
	When a shipment is created for each order
	Then the shipment for order 5678 should have carrier "Bar"
	And the shipment for order 1234 should have carrier "Foo"

Scenario: Shipping rule is applied when order is in quick filter
	Given the following "Other" profiles
		| Name             | Default | Carrier |
		| Defaults - Other | Yes     | Foo     |
		| For Rule         | No      | Bar     |
	And a quick filter named "Rule Filter" with an order number condition that BeginsWith 567
	And a shipping rule for "Other" that applies "For Rule" for filter "Rule Filter"
	And a default provider of Other
	And the following orders
		| Name   | Number |
		| First  | 1234   |
		| Second | 5678   |
	When a shipment is created for each order
	Then the shipment for order 5678 should have carrier "Bar"
	And the shipment for order 1234 should have carrier "Foo"

Scenario: Correct templates are returned for a shipment based on filter inclusion
	Given a shipment quick filter named "Filter 1" with an order number condition that BeginsWith 567
	And a shipment quick filter named "Filter 2" with an order number condition that BeginsWith 678
	And 5 templates
	And a print group for Other with the following rules
		| Condition | Template   |
		| Filter 1  | Template 1 |
		| Always    | Template 2 |
	And a print group for Other with the following rules
		| Condition | Template   |
		| Filter 2  | Template 3 |
		| Always    | Template 4 |
	And a print group for Other with the following rules
		| Condition | Template   |
		| Filter 2  | Template 5 |
	And the following orders
		| Name   | Number |
		| Second | 5678   |
	And the following "Other" profiles
		| Default   | Carrier | Name |
		| Yes       | Other   | Other Default Profile |
	And a default provider of Other
	When a shipment is created for each order
	When templates are retrieved for the shipment for order 5678
	Then the following templates should be returned
		| Name       |
		| Template 1 |
		| Template 4 |
	
