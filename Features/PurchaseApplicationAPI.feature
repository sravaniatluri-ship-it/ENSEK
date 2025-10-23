Feature: Purchase Application API Test Automation
  As a QA engineer
  I want to verify that ENSEK API endpoints behave correctly
  So that users can reset, buy, and view orders reliably

@api @reset
Scenario: Reset the test data successfully
    Given I reset the test data
    Then the response status should be 200

@api @buy
Scenario Outline: Purchase energy units
    When I buy <quantity> units of <energytype>
    Then the response status should be 200
    Examples:
      | energytype  | quantity |
      | 4          | 1        |

@api @orders
Scenario Outline: Verify orders listed correctly
	When I retrieve the list of orders
    Then the response status should be 200
    And the order should be listed with <fuelType> and <quantity>
    Examples:
      | fuelType  | quantity |
      | electric  | 23       |
      | oil       | 25       |

@api @date
Scenario: Count how many orders were created before the current date
    When I retrieve the list of orders
    Then the response status should be 200
    And I count how many orders were created before today

@api @buy 
Scenario Outline: Buy with invalid energy type & quantity type
    When I buy <quantity> units of <energytype>
    Then the response status should be 400
    And the response message should contain "Bad request"
    Examples:
      | energytype  | quantity |
      | 534354      | 53452    |

@api @unauthorized
Scenario: Try to access the API with invalid credentials
	When I access the API with invalid credentials "<username>" and "<password>"
	Then the response status should be 401
   Examples:
      | username  | password |
      | test1     | testing  |   

