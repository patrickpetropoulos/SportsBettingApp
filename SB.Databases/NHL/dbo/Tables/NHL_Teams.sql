CREATE TABLE [dbo].[NHL_Teams]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[TeamName] NCHAR(100) NOT NULL,
	[TeamNameShort] NCHAR(3) NOT NULL,
	[TeamNameShortAlt] NCHAR(3) NOT NULL,
	[TeamNameLocation] NCHAR(100) NOT NULL,
	[TeamNameCommon] NCHAR(100) NOT NULL,
	[TeamHomeColorFill]  NCHAR(8),
	[TeamHomeColorBorder] NCHAR(8),
	[TeamAwayColorFill]  NCHAR(8),
	[TeamAwayColorBorder] NCHAR(8),
	[FromYear] int NOT NULL,
	[ToYear]  int NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL
)