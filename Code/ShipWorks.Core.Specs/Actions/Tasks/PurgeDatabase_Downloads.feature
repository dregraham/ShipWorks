@tasks
@collection:database
Feature: PurgeDatabase Downloads
	In order to keep the size of my database small
	As an administrator
	I want to delete old download data when doing a purge

Scenario: Purge some data when some is older than cutoff and some is newer
	Given a task with purges (Downloads)
	And the following downloads
		| Started                 |
		| Now                     |
		| 1 day after cutoff      |
		| 1 day before cutoff     |
		| 1000 days before cutoff |
	When I run the purge
	Then download (0,1) should exist
	And download (2,3) should not exist
