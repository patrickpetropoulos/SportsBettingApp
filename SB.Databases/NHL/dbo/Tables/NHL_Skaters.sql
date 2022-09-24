CREATE TABLE [dbo].[NHL_Skaters]
(	[Id]  UNIQUEIDENTIFIER NOT NULL,
	[NhlReferenceId] NCHAR(10) NOT NULL UNIQUE,
	[FullName] NCHAR(100) NOT NULL,
	[YearFrom] INT NOT NULL,
	[YearTo] INT NOT NULL,
	[Position] NCHAR(1) NOT NULL,
	[PositionFull] NCHAR(10) NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL,
	CONSTRAINT PK_NHLSkaters PRIMARY KEY CLUSTERED ([ID]),
)
