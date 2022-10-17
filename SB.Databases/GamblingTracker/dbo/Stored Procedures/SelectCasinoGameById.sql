CREATE PROCEDURE [dbo].[SelectCasinoGameById]
	@casinoGameId UNIQUEIDENTIFIER
AS
	SELECT * from [dbo].[CasinoGames] where Id = @casinoGameId
RETURN 0
