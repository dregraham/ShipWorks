-- Modify the assembly in which the ActionJob class lives since we moved
-- it from ShipWorks to ShipWorks.Core
UPDATE Scheduling_JOB_DETAILS
	SET JOB_CLASS_NAME = JOB_CLASS_NAME + '.Core'
	WHERE JOB_CLASS_NAME LIKE '%, ShipWorks'