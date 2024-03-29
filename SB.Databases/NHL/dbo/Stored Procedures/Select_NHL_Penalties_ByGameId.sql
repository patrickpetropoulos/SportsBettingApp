﻿CREATE PROCEDURE [dbo].[Select_NHL_Penalties_ByGameId](        
        @GameId UNIQUEIDENTIFIER
        )
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
	SELECT   *
	  FROM [dbo].[NHL_Penalties]
      WHERE [GameId] = @GameId
END

