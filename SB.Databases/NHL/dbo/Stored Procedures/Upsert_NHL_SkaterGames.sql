CREATE PROCEDURE [dbo].[Upsert_NHL_SkaterGames](
	@SkaterId  UNIQUEIDENTIFIER  ,
	@GameId  UNIQUEIDENTIFIER  ,
	@TeamId  INT  ,
	@Number  INT  NULL,
	@Goals  INT  ,
	@Assist  INT  ,
	@Points  INT  ,
	@PlusMinus  INT  ,
	@PenaltyMinutes  INT  ,
	@EvenStrengthGoals  INT  ,
	@PowerplayGoals  INT  ,
	@ShorthandedGoals  INT  ,
	@GameWinningGoals  INT  ,
	@EvenStrengthAssists  INT  ,
	@PowerplayAssists  INT  ,
	@ShorthandedAssists  INT  ,
	@Shots  INT  ,
	@NumberOfShifts  INT  ,
	@TimeOnIce  int  ,
	@CreatedDate datetime  ,
	@ModifiedDate datetime
	)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN T1

UPDATE [dbo].[NHL_SkaterGames]
	SET [Number]=@Number 
		,[Goals]=@Goals 
		,[Assist]=@Assist 
		,[Points]=@Points 
		,[PlusMinus]=@PlusMinus 
		,[PenaltyMinutes]=@PenaltyMinutes 
		,[EvenStrengthGoals]=@EvenStrengthGoals 
		,[PowerplayGoals]=@PowerplayGoals 
		,[ShorthandedGoals]=@ShorthandedGoals 
		,[GameWinningGoals]=@GameWinningGoals 
		,[EvenStrengthAssists]=@EvenStrengthAssists 
		,[PowerplayAssists]=@PowerplayAssists 
		,[ShorthandedAssists]=@ShorthandedAssists 
		,[Shots]=@Shots 
		,[NumberOfShifts]=@NumberOfShifts 
		,[TimeOnIce]=@TimeOnIce
		,[ModifiedDate]=@ModifiedDate

	WHERE [SkaterId] = @SkaterId AND [GameId] = @GameId

 IF @@ROWCOUNT = 0
 BEGIN

	INSERT INTO [dbo].[NHL_SkaterGames](
		[SkaterId]
		,[GameId]
		,[TeamId]
		,[Number]
		,[Goals]
		,[Assist]
		,[Points]
		,[PlusMinus]
		,[PenaltyMinutes]
		,[EvenStrengthGoals]
		,[PowerplayGoals]
		,[ShorthandedGoals]
		,[GameWinningGoals]
		,[EvenStrengthAssists]
		,[PowerplayAssists]
		,[ShorthandedAssists]
		,[Shots]
		,[NumberOfShifts]
		,[TimeOnIce]
		,[CreatedDate]
		,[ModifiedDate]
		)
	VALUES
		(@SkaterId 
		,@GameId 
		,@TeamId 
		,@Number 
		,@Goals 
		,@Assist 
		,@Points 
		,@PlusMinus 
		,@PenaltyMinutes 
		,@EvenStrengthGoals 
		,@PowerplayGoals 
		,@ShorthandedGoals 
		,@GameWinningGoals 
		,@EvenStrengthAssists 
		,@PowerplayAssists 
		,@ShorthandedAssists 
		,@Shots 
		,@NumberOfShifts 
		,@TimeOnIce 
		,@CreatedDate
		,@ModifiedDate
		)
END
	COMMIT TRAN T1

END