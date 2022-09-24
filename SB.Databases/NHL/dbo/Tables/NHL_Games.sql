CREATE TABLE [dbo].[NHL_Games]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[NhlReferenceId] NCHAR(20) NOT NULL UNIQUE,
	[ShiftChartId] NCHAR(100) NULL UNIQUE,
	[SeasonId] INT NOT NULL,
	[ShiftChartGameId] NCHAR(10) NULL,
	[GameDate] datetime NOT NULL,
	[GameType] NCHAR(4) NOT NULL,
	[GameResult] NCHAR(3) NOT NULL,
	[AwayTeamId] INT NOT NULL,
	[HomeTeamId] INT NOT NULL,
	[AwayTeamScore] INT NOT NULL,
	[HomeTeamScore] INT NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL,

	CONSTRAINT PK_NHLGames PRIMARY KEY CLUSTERED ([ID]),

	 CONSTRAINT FK_NHL_Games_HomeTeam_NHL_Teams FOREIGN KEY (HomeTeamId) REFERENCES NHL_Teams(Id),
	 CONSTRAINT FK_NHL_Games_AwayTeam_NHL_Teams FOREIGN KEY (AwayTeamId) REFERENCES NHL_Teams(Id),
	 CONSTRAINT FK_NHL_Games_Season_NHL_Seasons FOREIGN KEY (SeasonId) REFERENCES NHL_Seasons(Id)
)

GO

--CREATE NONCLUSTERED INDEX [IDX_NHL_HR_SC_Raw_GameData_NhlReferenceId]
--    ON [dbo].[NHL_HR_SC_Raw_GameData]([NhlReferenceId] ASC);

--GO

--[CREATE NONCLUSTERED INDEX [IDX_NHL_HR_SC_Raw_GameData_ShiftChartId]
   -- ON [dbo].[NHL_HR_SC_Raw_GameData]([ShiftChartId] ASC);
