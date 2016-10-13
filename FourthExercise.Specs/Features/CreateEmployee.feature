Feature: Create new employee
	Allow user to create a new employee

@mytag
Scenario: Successfully create new employee
	Given an user has created a new employee
	And he entered all required information
	When he completes the process
	Then that employee should be present within the system
