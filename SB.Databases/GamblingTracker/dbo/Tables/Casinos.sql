CREATE TABLE [dbo].[Casinos]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY,
	[Name] varchar(100) NOT NULL,
	[CountryCode] varchar(2) NOT NULL

	CONSTRAINT Unique_Casino UNIQUE ([Name], [CountryCode])
)
