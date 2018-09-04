@collection:database
Feature: AddItemUpdateRollup
    In order to learn SpecFlow
    As a QA person
    I want to try writing tests

Scenario: Order item rollup updates after adding item
    Given an order 
    And the order has items
        |Quantity				|
        |1						|
        |3						|
    When a new item is added to an order
        |Quantity				|
        |1						|
    Then item quantity rollup shows 5

@ignore
Scenario: Order item count rollup updates
    Given an order 
    And the order has items
         | Quantity |
         | 1        |
         | 3        |
    When a new item is added to an order
        | Quantity |
        | 2        |
    Then item count rollup shows 3

@ignore
Scenario: Order note count rollup updates
    Given an order
    And the order has notes
        | Text    |
        | Hello   |
        | Goodbye |
    When a new note is added to the order
        | Text |
        | Wow  |
    Then note count rollup shows 3
