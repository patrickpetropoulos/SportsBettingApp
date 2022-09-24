CREATE TABLE [dbo].[NHL_SkaterGames]
(
	[SkaterId]  UNIQUEIDENTIFIER NOT NULL,
	[GameId]  UNIQUEIDENTIFIER NOT NULL,
	[TeamId] INT NOT NULL,
	[Number] INT,
	[Goals] INT NOT NULL,
	[Assist] INT NOT NULL,
	[Points] INT NOT NULL,
	[PlusMinus] INT NOT NULL,
	[PenaltyMinutes] INT NOT NULL,
	[EvenStrengthGoals] INT NOT NULL,
	[PowerplayGoals] INT NOT NULL,
	[ShorthandedGoals] INT NOT NULL,
	[GameWinningGoals] INT NOT NULL,
	[EvenStrengthAssists] INT NOT NULL,
	[PowerplayAssists] INT NOT NULL,
	[ShorthandedAssists] INT NOT NULL,
	[Shots] INT NOT NULL,
	[NumberOfShifts] INT NOT NULL,
	[TimeOnIce] int NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL,

	CONSTRAINT PK_NHL_SkaterGames_PKTable PRIMARY KEY CLUSTERED ([SkaterId], [GameId] ),

	CONSTRAINT FK_NHL_SkaterGame_TeamId_NHL_Teams FOREIGN KEY ([TeamId]) REFERENCES NHL_Teams(Id),
	CONSTRAINT FK_NHL_SkaterGame_GameId_NHL_Games FOREIGN KEY ([GameId]) REFERENCES NHL_Games(Id),
	CONSTRAINT FK_NHL_SkaterGame_SkaterId_NHL_Goalies FOREIGN KEY ([SkaterId]) REFERENCES NHL_Skaters(Id),

)
