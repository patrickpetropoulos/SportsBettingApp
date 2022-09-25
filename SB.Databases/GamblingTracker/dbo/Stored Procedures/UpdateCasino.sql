CREATE PROCEDURE [dbo].[UpdateCasino]
	@id int,
	@Name varchar(100),
	@CountryCode varchar(2)
AS
	UPDATE [dbo].[Casinos]
	set [Name] = @Name,
		[CountryCode] = @CountryCode
	where [Id] = @id
GO
