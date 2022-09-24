CREATE PROCEDURE [dbo].[Upsert_NHL_GoalieGames](
	@GoalieId UNIQUEIDENTIFIER  ,
	@GameId UNIQUEIDENTIFIER  ,
	@TeamId INT  ,
	@Number INT NULL ,
	@Decision NCHAR(1)  ,
	@GoalsAgainst INT  ,
	@ShotsAgainst INT  ,
	@Saves INT  ,
	@IsShutout BIT  ,
	@PenaltyMinutes INT  ,
	@TimeOnIce  INT  ,
	@CreatedDate datetime  ,
	@ModifiedDate datetime
	)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN T1

UPDATE [dbo].[NHL_GoalieGames]
	SET [GoalieId]=@GoalieId
		,[GameId]=@GameId
		,[TeamId]=@TeamId
		,[Number]=@Number
		,[Decision]=@Decision
		,[GoalsAgainst]=@GoalsAgainst
		,[ShotsAgainst]=@ShotsAgainst
		,[Saves]=@Saves
		,[IsShutout]=@IsShutout
		,[PenaltyMinutes]=@PenaltyMinutes
		,[TimeOnIce]=@TimeOnIce 
		,[ModifiedDate]=@ModifiedDate

	WHERE [GoalieId] = @GoalieId AND [GameId] = @GameId

 IF @@ROWCOUNT = 0
 BEGIN

	INSERT INTO [dbo].[NHL_GoalieGames](
		[GoalieId]
		,[GameId]
		,[TeamId]
		,[Number]
		,[Decision]
		,[GoalsAgainst]
		,[ShotsAgainst]
		,[Saves]
		,[IsShutout]
		,[PenaltyMinutes]
		,[TimeOnIce]
		,[CreatedDate]
		,[ModifiedDate]
		)
	VALUES
		(@GoalieId
		,@GameId
		,@TeamId
		,@Number
		,@Decision
		,@GoalsAgainst
		,@ShotsAgainst
		,@Saves
		,@IsShutout
		,@PenaltyMinutes
		,@TimeOnIce 
		,@CreatedDate
		,@ModifiedDate
		)
END
	COMMIT TRAN T1

END