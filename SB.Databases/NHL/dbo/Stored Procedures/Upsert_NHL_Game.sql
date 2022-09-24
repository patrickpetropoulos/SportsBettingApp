CREATE PROCEDURE [dbo].[Upsert_NHL_Game](
	@Id UNIQUEIDENTIFIER  ,
	@NhlReferenceId NCHAR(20)  ,
	@ShiftChartId NCHAR(100) NULL ,
	@SeasonId INT  ,
	@ShiftChartGameId NCHAR(10) NULL ,
	@GameDate datetime  ,
	@GameType NCHAR(4)  ,
	@GameResult NCHAR(3)  ,
	@AwayTeamId INT  ,
	@HomeTeamId INT  ,
	@AwayTeamScore INT  ,
	@HomeTeamScore INT  ,
	@CreatedDate datetime  ,
	@ModifiedDate datetime
	)
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRAN T1

UPDATE [dbo].[NHL_Games]
	SET [NhlReferenceId]=@NhlReferenceId
		,[ShiftChartId]=@ShiftChartId
		,[SeasonId]=@SeasonId
		,[ShiftChartGameId]=@ShiftChartGameId
		,[GameDate]=@GameDate
		,[GameType]=@GameType
		,[GameResult]=@GameResult
		,[AwayTeamId]=@AwayTeamId
		,[HomeTeamId]=@HomeTeamId
		,[AwayTeamScore]=@AwayTeamScore
		,[HomeTeamScore]=@HomeTeamScore
		,[ModifiedDate]=@ModifiedDate

	 WHERE [Id] = @Id		
		
IF @@ROWCOUNT = 0
 BEGIN

	INSERT INTO [dbo].[NHL_Games](
		[Id]
		,[NhlReferenceId]
		,[ShiftChartId]
		,[SeasonId]
		,[ShiftChartGameId]
		,[GameDate]
		,[GameType]
		,[GameResult]
		,[AwayTeamId]
		,[HomeTeamId]
		,[AwayTeamScore]
		,[HomeTeamScore]
		,[CreatedDate]
		,[ModifiedDate]
		)
	VALUES
		(@Id
		,@NhlReferenceId
		,@ShiftChartId
		,@SeasonId
		,@ShiftChartGameId
		,@GameDate
		,@GameType
		,@GameResult
		,@AwayTeamId
		,@HomeTeamId
		,@AwayTeamScore
		,@HomeTeamScore
		,@CreatedDate
		,@ModifiedDate
		)	      
END
	COMMIT TRAN T1
END