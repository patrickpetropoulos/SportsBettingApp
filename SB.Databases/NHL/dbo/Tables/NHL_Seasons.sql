CREATE TABLE [dbo].[NHL_Seasons]
(
	[Id]  INT IDENTITY(1,1) PRIMARY KEY, 
	[Year] INT NOT NULL UNIQUE,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL,

)
