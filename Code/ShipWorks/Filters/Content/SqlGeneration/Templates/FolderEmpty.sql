UPDATE FilterNodeContent
  SET [Status] = {0},
      [Cost] = 0,
      [Count] = 0,
      [CountVersion] = 1
  WHERE FilterNodeContentID = @filterNodeContentID
