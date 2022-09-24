CREATE PROCEDURE [dbo].[Select_NHL_Teams]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
	SELECT [Id] 
            ,[TeamName] 
            ,[TeamNameShort] 
            ,[TeamNameShortAlt] 
            ,[TeamNameLocation] 
            ,[TeamNameCommon] 
            ,[TeamHomeColorFill]  
            ,[TeamHomeColorBorder] 
            ,[TeamAwayColorFill]  
            ,[TeamAwayColorBorder] 
            ,[FromYear] 
            ,[ToYear]  
            ,[CreatedDate] 
            ,[ModifiedDate]
	  FROM [dbo].[NHL_Teams]

END