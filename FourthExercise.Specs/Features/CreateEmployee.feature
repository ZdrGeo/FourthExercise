Feature: Create Employee
	Allow user to create an employee

@mytag
Scenario: Successfully create employee
	Given an user has entered information about an employee
	When he completes entering information
	Then that employee should be present in the system
