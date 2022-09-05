CREATE TABLE [dbo].[GamblingSession]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[UserId] UNIQUEIDENTIFIER,
	[CasinoId] INT,
	[CasinoGameId] INT,
	[GameSubTypeId] INT,
	[StartAmount] NUMERIC(18, 2),
	[EndAmount] NUMERIC(18, 2)

	--CONSTRAINT FK_GamblingSession_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
	CONSTRAINT FK_GamblingSession_Casinos FOREIGN KEY (CasinoId) REFERENCES Casinos(Id),
	CONSTRAINT FK_GamblingSession_CasinoGame FOREIGN KEY (CasinoGameId) REFERENCES CasinoGames(Id),
	--add game subtype constraint to that table, have sub types for slots, video keno, video poker

)
