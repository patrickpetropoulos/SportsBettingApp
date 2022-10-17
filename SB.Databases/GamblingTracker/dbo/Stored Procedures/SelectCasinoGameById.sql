CREATE PROCEDURE [dbo].[SelectCasinoGameById]
	@Id UNIQUEIDENTIFIER
AS
	SELECT * from [dbo].[CasinoGames] where Id = @Id
RETURN 0
