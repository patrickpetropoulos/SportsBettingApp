CREATE PROCEDURE [dbo].[UpsertCasinoGame]
      @Id UNIQUEIDENTIFIER,
      @Name varchar(100),
      @HasSubType bit NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE [dbo].[CasinoGames]
	set [Name] = @Name,
		[HasSubType] = @HasSubType
	where [Id] = @id

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO  CasinoGames([Id],[Name], [HasSubType])
		VALUES (@Id, @Name, @HasSubType)
		
	END
END
