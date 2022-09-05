CREATE TABLE [dbo].[Casinos]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] varchar(100) NOT NULL,
	[CountryCode] varchar(2) NOT NULL

	CONSTRAINT Unique_Casino UNIQUE ([Name], [CountryCode])
)
