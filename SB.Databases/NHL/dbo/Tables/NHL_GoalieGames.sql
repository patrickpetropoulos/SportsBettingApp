CREATE TABLE [dbo].[NHL_GoalieGames]
(	
	[GoalieId]  UNIQUEIDENTIFIER NOT NULL,
	[GameId]  UNIQUEIDENTIFIER NOT NULL,
	[TeamId] INT NOT NULL,
	[Number] INT NULL,
	[Decision] NCHAR(1) NOT NULL,
	[GoalsAgainst] INT NOT NULL,
	[ShotsAgainst] INT NOT NULL,
	[Saves] INT NOT NULL,
	[IsShutout] BIT NOT NULL,
	[PenaltyMinutes] INT NOT NULL,
	[TimeOnIce] INT NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL,

	CONSTRAINT PK_NHL_GoalieGames_PKTable PRIMARY KEY CLUSTERED ([GoalieId], [GameId] ),

	 CONSTRAINT FK_NHL_GoalieGames_TeamId_NHL_Teams FOREIGN KEY ([TeamId]) REFERENCES NHL_Teams(Id),
	 CONSTRAINT FK_NHL_GoalieGames_GameId_NHL_Games FOREIGN KEY ([GameId]) REFERENCES NHL_Games(Id),
	 CONSTRAINT FK_NHL_GoalieGames_GoalieId_NHL_Goalies FOREIGN KEY ([GoalieId]) REFERENCES NHL_Goalies(Id),
	)