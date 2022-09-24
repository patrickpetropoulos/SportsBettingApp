﻿CREATE PROCEDURE [dbo].[Select_NHL_Games_BySeasonId]
	@SeasonId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT *
  FROM [dbo].[NHL_Games]
  WHERE [SeasonId] = @SeasonId

END