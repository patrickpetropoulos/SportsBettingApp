CREATE PROCEDURE [dbo].[InsertCasino]
      @Name varchar(100),
      @CountryCode varchar(2),
      @id int output
AS
BEGIN
      SET NOCOUNT ON;
      INSERT INTO  Casinos ([Name], [CountryCode])
      VALUES (@Name, @CountryCode)
      SET @id=SCOPE_IDENTITY()
      RETURN  @id
END
