CREATE TABLE [dbo].[NHL_Penalties]
(
	[GameId] UNIQUEIDENTIFIER NOT NULL,
	[Period] INT NOT NULL,
	[DisplayTime] INT NOT NULL,
	[TeamId] INT NOT NULL,
	[SkaterId] UNIQUEIDENTIFIER NOT NULL,
	[PenaltyType] VARCHAR(200) NOT NULL,
	[PenaltyMinutes] INT NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL,

	CONSTRAINT PK_NHL_Penalties_PKTable PRIMARY KEY CLUSTERED ( [GameId], [Period], [DisplayTime], [SkaterId],[PenaltyType],[PenaltyMinutes] ),

	CONSTRAINT FK_NHL_Penalties_SkaterId_PenaltyTaker FOREIGN KEY ([SkaterId]) REFERENCES NHL_Skaters(Id),
	CONSTRAINT FK_NHL_Penalties_GameId FOREIGN KEY ([GameId]) REFERENCES NHL_Games(Id),
	CONSTRAINT FK_NHL_Penalties_TeamId FOREIGN KEY ([TeamId]) REFERENCES NHL_Teams(Id),

)
