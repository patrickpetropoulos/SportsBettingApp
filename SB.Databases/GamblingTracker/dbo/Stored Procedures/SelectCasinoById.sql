CREATE PROCEDURE [dbo].[SelectCasinoById]
	@Id UNIQUEIDENTIFIER
AS
	SELECT * from [dbo].[Casinos] where Id = @Id
RETURN 0