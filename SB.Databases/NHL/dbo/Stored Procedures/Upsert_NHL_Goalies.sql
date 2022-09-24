CREATE PROCEDURE [dbo].[Upsert_NHL_Goalies](
	@Id UNIQUEIDENTIFIER,
	@NhlReferenceId NCHAR(10)  ,
	@FullName NCHAR(100)  ,
	@YearFrom INT  ,
	@YearTo INT  ,
	@CreatedDate datetime  ,
	@ModifiedDate datetime  
	)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN T1

UPDATE [dbo].[NHL_Goalies]
	SET  [NhlReferenceId]=@NhlReferenceId
		,[FullName]=@FullName
		,[YearFrom]=@YearFrom
		,[YearTo]=@YearTo
		,[ModifiedDate]=@ModifiedDate
		WHERE [Id]=@Id

 IF @@ROWCOUNT = 0
 BEGIN

	INSERT INTO [dbo].[NHL_Goalies](
		[Id]
		,[NhlReferenceId]
		,[FullName]
		,[YearFrom]
		,[YearTo]
		,[CreatedDate]
		,[ModifiedDate]
		)
	VALUES
		(@Id
		,@NhlReferenceId
		,@FullName
		,@YearFrom
		,@YearTo
		,@CreatedDate
		,@ModifiedDate
		)
END
	COMMIT TRAN T1
END

