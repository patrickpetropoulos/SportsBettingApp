CREATE PROCEDURE [dbo].[SelectAllGamblingSessionsByUser]
	@userId UNIQUEIDENTIFIER
AS
	SELECT * from [dbo].[GamblingSession] where UserId = @userId
RETURN 0
