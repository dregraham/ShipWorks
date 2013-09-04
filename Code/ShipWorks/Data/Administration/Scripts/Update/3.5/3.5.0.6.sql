
-- set input source to 'Nothing' for existing tasks and queue steps not requiring input
UPDATE ActionTask
SET InputSource = -1
WHERE TaskIdentifier IN ('PlaySound');

UPDATE ActionQueueStep
SET InputSource = -1
WHERE TaskIdentifier IN ('PlaySound');
