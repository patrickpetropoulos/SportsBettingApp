CREATE PROCEDURE [dbo].[UpsertCasino]
	@Id UNIQUEIDENTIFIER,
    @Name varchar(100),
    @CountryCode varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here\
    UPDATE [dbo].[Casinos]
	set [Name] = @Name,
		[CountryCode] = @CountryCode
	where [Id] = @Id

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO  Casinos ([Id],[Name], [CountryCode])
		VALUES (@Id, @Name, @CountryCode)
		
	END
END
