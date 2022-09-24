CREATE PROCEDURE [dbo].[Upsert_NHL_Penalties](
	@GameId UNIQUEIDENTIFIER  ,
	@Period INT  ,
	@DisplayTime INT  ,
	@TeamId INT  ,
	@SkaterId UNIQUEIDENTIFIER  ,
	@PenaltyType VARCHAR(200)  ,
	@PenaltyMinutes INT  ,
	@CreatedDate datetime  ,
	@ModifiedDate datetime 
	)
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRAN T1

UPDATE [dbo].[NHL_Penalties]
	SET [GameId]=@GameId
		,[Period]=@Period
		,[DisplayTime]=@DisplayTime
		,[TeamId]=@TeamId
		,[SkaterId]=@SkaterId
		,[PenaltyType]=@PenaltyType
		,[PenaltyMinutes]=@PenaltyMinutes
		,[ModifiedDate]=@ModifiedDate


	 WHERE [GameId]=@GameId and [Period]=@Period and [DisplayTime]=@DisplayTime	and [SkaterId]=@SkaterId and [PenaltyType]=@PenaltyType and [PenaltyMinutes]=@PenaltyMinutes
		
IF @@ROWCOUNT = 0
 BEGIN

	INSERT INTO [dbo].[NHL_Penalties](
		[GameId]
		,[Period]
		,[DisplayTime]
		,[TeamId]
		,[SkaterId]
		,[PenaltyType]
		,[PenaltyMinutes]
		,[CreatedDate]
		,[ModifiedDate]
		)
	VALUES
		(@GameId
		,@Period
		,@DisplayTime
		,@TeamId
		,@SkaterId
		,@PenaltyType
		,@PenaltyMinutes
		,@CreatedDate
		,@ModifiedDate
		)	      
END
	COMMIT TRAN T1
END