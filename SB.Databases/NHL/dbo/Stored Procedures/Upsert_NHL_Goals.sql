CREATE PROCEDURE [dbo].[Upsert_NHL_Goals](
	@GameId UNIQUEIDENTIFIER  ,
	@Period INT  ,
	@DisplayTime INT  ,
	@TeamId INT  ,
	@GoalScorerSkaterId UNIQUEIDENTIFIER  ,
	@AssistScorer1SkaterId UNIQUEIDENTIFIER NULL ,
	@AssistScorer2SkaterId UNIQUEIDENTIFIER NULL ,
	@GoalieId UNIQUEIDENTIFIER NULL ,
	@IsPowerPlayGoal BIT  ,
	@IsShortHandedGoal BIT  ,
	@IsPenaltyShotGoal BIT  ,
	@IsEmptyNetGoal BIT  ,
	@IsShootOutGoal BIT  ,
	@CreatedDate datetime  ,
	@ModifiedDate datetime  
	)
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRAN T1

UPDATE [dbo].[NHL_Goals]
	SET [GoalScorerSkaterId]=@GoalScorerSkaterId
		,[AssistScorer1SkaterId]=@AssistScorer1SkaterId
		,[AssistScorer2SkaterId]=@AssistScorer2SkaterId
		,[GoalieId]=@GoalieId
		,[TeamId]=@TeamId
		,[IsPowerPlayGoal]=@IsPowerPlayGoal
		,[IsShortHandedGoal]=@IsShortHandedGoal
		,[IsPenaltyShotGoal]=@IsPenaltyShotGoal
		,[IsEmptyNetGoal]=@IsEmptyNetGoal
		,[IsShootOutGoal]=@IsShootOutGoal
		,[ModifiedDate]=@ModifiedDate

	 WHERE [GameId]=@GameId and [Period]=@Period and [DisplayTime]=@DisplayTime	
		
IF @@ROWCOUNT = 0
 BEGIN

	INSERT INTO [dbo].[NHL_Goals](
		[GameId]
		,[Period]
		,[DisplayTime]
		,[TeamId]
		,[GoalScorerSkaterId]
		,[AssistScorer1SkaterId]
		,[AssistScorer2SkaterId]
		,[GoalieId]
		,[IsPowerPlayGoal]
		,[IsShortHandedGoal]
		,[IsPenaltyShotGoal]
		,[IsEmptyNetGoal]
		,[IsShootOutGoal]
		,[CreatedDate]
		,[ModifiedDate]

		)
	VALUES
		(@GameId
		,@Period
		,@DisplayTime
		,@TeamId
		,@GoalScorerSkaterId
		,@AssistScorer1SkaterId
		,@AssistScorer2SkaterId
		,@GoalieId
		,@IsPowerPlayGoal
		,@IsShortHandedGoal
		,@IsPenaltyShotGoal
		,@IsEmptyNetGoal
		,@IsShootOutGoal
		,@CreatedDate
		,@ModifiedDate
		)	      
END
	COMMIT TRAN T1
END