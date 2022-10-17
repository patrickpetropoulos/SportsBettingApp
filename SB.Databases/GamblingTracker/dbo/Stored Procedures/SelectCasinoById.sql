CREATE PROCEDURE [dbo].[SelectCasinoById]
	@casinoId UNIQUEIDENTIFIER
AS
	SELECT * from [dbo].[Casinos] where Id = @casinoId
RETURN 0