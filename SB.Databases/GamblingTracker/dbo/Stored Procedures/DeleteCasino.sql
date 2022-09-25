CREATE PROCEDURE [dbo].[DeleteCasino]
	@id int
AS
	DELETE FROM [dbo].[Casinos] where id = @id
RETURN 0
