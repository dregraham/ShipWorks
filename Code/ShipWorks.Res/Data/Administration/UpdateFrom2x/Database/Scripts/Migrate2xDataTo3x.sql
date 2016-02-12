-----------------------------------------------------------------
-- Users
--
INSERT INTO [User] (Username, Password, Email, IsAdmin, IsDeleted)
           SELECT Username, Password, '',    IsAdmin, Deleted
             FROM v2m_Users