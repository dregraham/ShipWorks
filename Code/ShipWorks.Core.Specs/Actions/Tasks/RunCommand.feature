@tasks
@collection:database
Feature: RunCommand
	In order to perform actions not native to ShipWorks
	As a customer
	I want to run custom commands from actions

Scenario: Command with no input logs output correctly
	Given the command "echo foo"
	When I run the task with no input
	Then the most recent log should contain "O> foo"

Scenario: Command with no input logs errors correctly
	Given the command "echo foo 1>&2"
	When I run the task with no input
	Then the most recent log should contain "E> foo"

Scenario: Command with one order logs output
	Given the command "echo Order #{//Order/Number}"
	And the step input source Selection
	And the following orders
		| Name  | Number |
		| First | 1234   |
	When I run the task with orders (First)
	Then the most recent log should contain "O> Order #1234"

Scenario: Command with two orders and OneTime cardinality logs output to single file
	Given the command "echo <xsl:for-each select="//Order"><xsl:text>Order #</xsl:text><xsl:value-of select="Number" /><xsl:text> </xsl:text></xsl:for-each>"
	And the step input source Selection
	And a run cardinality of OneTime
	And the following orders
		| Name   | Number |
		| First  | 1234   |
		| Second | 5678   |
	When I run the task with orders (First, Second)
	Then the most recent log should contain "O> Order #1234 Order #5678"

Scenario: Command with two orders and OncePerFilterEntry cardinality logs output to multiple files
	Given the command "echo <xsl:for-each select="//Order"><xsl:text>Order #</xsl:text><xsl:value-of select="Number" /><xsl:text> </xsl:text></xsl:for-each>"
	And the step input source Selection
	And a run cardinality of OncePerFilterEntry
	And the following orders
		| Name   | Number |
		| First  | 1234   |
		| Second | 5678   |
	When I run the task with orders (First, Second)
	Then the most recent log should contain "O> Order #5678"
	Then the 2nd most recent log should contain "O> Order #1234"

Scenario: Command finishes before timeout
	Given the command "ping localhost -n 3"
	And a command timeout of 4 seconds
	And should stop command on timeout is true
	When I run the task with no input
	Then the most recent log should contain "O> Approximate round trip times in milli-seconds:"
