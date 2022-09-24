CREATE PROCEDURE [dbo].[Select_NHL_Goalies]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
	SELECT   [Id]
            ,[NhlReferenceId]
            ,[FullName]
            ,[YearFrom]
            ,[YearTo]
            ,[CreatedDate]
            ,[ModifiedDate]

	  FROM [dbo].[NHL_Goalies]

END

