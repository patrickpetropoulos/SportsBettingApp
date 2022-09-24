CREATE PROCEDURE [dbo].[Select_NHL_SeasonGame_BySeasonId]
	@SeasonId int,
	@GameType nchar(4),
	@IncludeGoalies bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if @IncludeGoalies = 1
	select 
	g.Id as GameId, g.NhlReferenceId as NhlReferenceId, g.GameDate as GameDate, g.GameResult as GameResult, 
	awayT.TeamNameShort as AwayTeamName, g.AwayTeamScore as AwayTeamScore,  
	homeT.TeamNameShort as HomeTeamName,g.HomeTeamScore as HomeTeamScore,
	ag.FullName as AwayTeamGoalie, ag.Id as AwayTeamGoalieId, hg.FullName as HomeTeamGoalie, hg.Id as HomeTeamGoalieId, 
	g.HomeTeamScore - g.AwayTeamScore as HomeScoreDiff
	from NHL_Games g
	join NHL_Seasons season on g.SeasonId = season.Id
	join NHL_GoalieGames agg on agg.GameId = g.Id and agg.TeamId = g.AwayTeamId and agg.Decision <> 'N'
	join NHL_Goalies ag on agg.GoalieId = ag.Id
	join NHL_Teams awayT on awayT.Id = g.AwayTeamId
	join NHL_GoalieGames hgg on hgg.GameId = g.Id and hgg.TeamId = g.HomeTeamId and hgg.Decision <> 'N'
	join NHL_Goalies hg on hgg.GoalieId = hg.Id
	join NHL_Teams homeT on homeT.Id = g.HomeTeamId
	where season.Id = @seasonId
	and g.GameType = @GameType

	order by g.GameDate asc
else
	select 
	g.Id as GameId,g.NhlReferenceId as NhlReferenceId, g.GameDate as GameDate, g.GameResult as GameResult, 
	awayT.TeamNameShort as AwayTeamName, g.AwayTeamScore as AwayTeamScore,  
	homeT.TeamNameShort as HomeTeamName,g.HomeTeamScore as HomeTeamScore,
	g.HomeTeamScore - g.AwayTeamScore as HomeScoreDiff
	from NHL_Games g
	join NHL_Seasons season on g.SeasonId = season.Id
	join NHL_Teams awayT on awayT.Id = g.AwayTeamId
	join NHL_Teams homeT on homeT.Id = g.HomeTeamId
	where season.Id = @seasonId
	and g.GameType = @GameType

	order by g.GameDate asc
END