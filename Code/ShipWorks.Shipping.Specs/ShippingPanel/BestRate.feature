@collection:database
Feature: BestRate
	In order to use Best Rate in the shipping panel
	as a Legacy account shipper
	ShipWorks allows user access to best rate.

Scenario: User does not have access to Best Rate in Shipping Panel
	Given a Legacy Tango account
	And Best Rate is off in Tango
	When a shipment is loaded
	Then the user can not access Best Rate

Scenario: User has access to Best Rate in Shipping Panel
	Given a Legacy Tango account
	And Best Rate is on in Tango
	When a shipment is loaded
	Then the user can access Best Rate