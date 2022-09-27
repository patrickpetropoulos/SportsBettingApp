CREATE TABLE [dbo].[CasinoGames]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY,
	[Name] varchar(50) NOT NULL,
	[HasSubType] bit DEFAULT 0 
)
