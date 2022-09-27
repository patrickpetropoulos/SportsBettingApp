CREATE PROCEDURE [dbo].[DeleteCasinoGame]
	@Id UNIQUEIDENTIFIER
AS
	DELETE FROM [dbo].[CasinoGames] where Id = @Id
RETURN 0
