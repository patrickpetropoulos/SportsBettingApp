CREATE PROCEDURE [dbo].[Upsert_NHL_Skaters](
	@Id UNIQUEIDENTIFIER,
	@NhlReferenceId NCHAR(10)  ,
	@FullName NCHAR(100)  ,
	@YearFrom INT  ,
	@YearTo INT  ,
	@Position NCHAR(1)  ,
	@PositionFull NCHAR(10),
	@CreatedDate datetime  ,
	@ModifiedDate datetime  
	)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN T1

UPDATE [dbo].[NHL_Skaters]
	SET  [NhlReferenceId]=@NhlReferenceId
		,[FullName]=@FullName
		,[YearFrom]=@YearFrom
		,[YearTo]=@YearTo
		,[Position]=@Position
		,[PositionFull]=@PositionFull
		,[ModifiedDate]=@ModifiedDate
		WHERE [Id]=@Id

 IF @@ROWCOUNT = 0
 BEGIN

	INSERT INTO [dbo].[NHL_Skaters](
		[Id]
		,[NhlReferenceId]
		,[FullName]
		,[YearFrom]
		,[YearTo]
		,[Position]
		,[PositionFull]
		,[CreatedDate]
		,[ModifiedDate]
		)
	VALUES
		(@Id
		,@NhlReferenceId
		,@FullName
		,@YearFrom
		,@YearTo
		,@Position
		,@PositionFull
		,@CreatedDate
		,@ModifiedDate
		)
END
	COMMIT TRAN T1
END
