CREATE PROCEDURE [dbo].[InsertCasinoGame]
      @Name varchar(100),
      @HasSubType bit NULL,
      @id int output
AS
BEGIN
      SET NOCOUNT ON;
      INSERT INTO  CasinoGames([Name], [HasSubType])
      VALUES (@Name, @HasSubType)
      SET @id=SCOPE_IDENTITY()
      RETURN  @id
END
