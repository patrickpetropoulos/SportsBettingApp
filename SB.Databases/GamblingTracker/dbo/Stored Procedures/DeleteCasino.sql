CREATE PROCEDURE [dbo].[DeleteCasino]
	@Id UNIQUEIDENTIFIER
AS
	DELETE FROM [dbo].[Casinos] where Id = @Id
RETURN 0
