-- Modify the assembly in which the ActionJob class lives since we moved
-- it from ShipWorks to ShipWorks.Core
-- We have to do this again because the initial data script did not set this correctly
UPDATE Scheduling_JOB_DETAILS
	SET JOB_CLASS_NAME = JOB_CLASS_NAME + '.Core'
	WHERE JOB_CLASS_NAME LIKE '%, ShipWorks'