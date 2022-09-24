CREATE TABLE [dbo].[NHL_Goals]
(
	[GameId] UNIQUEIDENTIFIER NOT NULL,
	[Period] INT NOT NULL,
	[DisplayTime] INT NOT NULL,
	[TeamId] INT NOT NULL,
	[GoalScorerSkaterId] UNIQUEIDENTIFIER NOT NULL,
	[AssistScorer1SkaterId] UNIQUEIDENTIFIER NULL,
	[AssistScorer2SkaterId] UNIQUEIDENTIFIER NULL,
	[GoalieId] UNIQUEIDENTIFIER NULL,
	[IsPowerPlayGoal] BIT NOT NULL,
	[IsShortHandedGoal] BIT NOT NULL,
	[IsPenaltyShotGoal] BIT NOT NULL,
	[IsEmptyNetGoal] BIT NOT NULL,
	[IsShootOutGoal] BIT NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL,

	CONSTRAINT PK_NHL_Goals_PKTable PRIMARY KEY CLUSTERED ( [GameId], [Period], [DisplayTime] ),

	CONSTRAINT FK_NHL_Goals_SkaterId_GoalScorer FOREIGN KEY ([GoalScorerSkaterId]) REFERENCES NHL_Skaters(Id),
	CONSTRAINT FK_NHL_Goals_GameId FOREIGN KEY ([GameId]) REFERENCES NHL_Games(Id),
	CONSTRAINT FK_NHL_Goals_TeamId FOREIGN KEY ([TeamId]) REFERENCES NHL_Teams(Id),
)
