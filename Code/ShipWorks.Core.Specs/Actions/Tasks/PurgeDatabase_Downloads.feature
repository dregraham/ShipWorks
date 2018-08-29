@tasks
@collection:database
Feature: PurgeDatabase Downloads
	In order to keep the size of my database small
	As an administrator
	I want to delete old download data when doing a purge

Scenario: Purge some data when some are older than cutoff and some is newer
	Given a task with purges (Downloads)
	And the following downloads
		| Download Start                  |
		| Now                     |
		| 1 day newer than cutoff      |
		| 1 day older than cutoff     |
		| 1000 days older than cutoff |
	When I run the purge
	Then download (0,1) should exist
	And download (2,3) should not exist 

Scenario: Purge all data when all data is older than cutoff
	Given a task with purges (Downloads)
	And the following downloads
		| Download Start                 |
		| 1 day older than cutoff      |
		| 2 days older than cutoff     |
		| 1000 days older than cutoff  |
	When I run the purge
	Then download (0,1,2) should not exist 

Scenario: Purge no data when all data is newer than cutoff
	Given a task with purges (Downloads)
	And the following downloads
		| Download Start                 |
		| 1 day newer than cutoff      |
		| 2 days newer than cutoff     |
		| 1000 days newer than cutoff  |
	When I run the purge
	Then download (0,1,2) should exist 
