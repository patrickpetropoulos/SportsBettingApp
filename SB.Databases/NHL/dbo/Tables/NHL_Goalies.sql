CREATE TABLE [dbo].[NHL_Goalies]
(
	[Id]  UNIQUEIDENTIFIER NOT NULL,
	[NhlReferenceId] NCHAR(10) NOT NULL UNIQUE,
	[FullName] NCHAR(100) NOT NULL,
	[YearFrom] INT NOT NULL,
	[YearTo] INT NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ModifiedDate] datetime NOT NULL,
	CONSTRAINT PK_NHLGoalies PRIMARY KEY CLUSTERED ([ID]),
)
