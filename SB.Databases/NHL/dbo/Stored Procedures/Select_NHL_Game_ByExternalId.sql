CREATE PROCEDURE [dbo].[Select_NHL_Game_ByExternalId]
	@GameId NCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT *
  FROM [dbo].[NHL_Games]
  WHERE [NhlReferenceId] = @GameId or [ShiftChartId] = @GameId

END