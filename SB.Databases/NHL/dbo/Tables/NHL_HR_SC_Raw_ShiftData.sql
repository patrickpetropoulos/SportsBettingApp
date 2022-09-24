CREATE TABLE [dbo].[NHL_HR_SC_Raw_ShiftData]
(
	[NhlReferenceId] NCHAR(100) NOT NULL,
	[SkaterId] NCHAR(100) NOT NULL, 
    [Start] FLOAT (53) NOT NULL,
    [Duration] FLOAT (53) NOT NULL,
	[Event] NCHAR(10) NOT NULL, 
	[Period] int NOT NULL

     CONSTRAINT PK_NHL_HR_SC_Raw_ShiftData_PKTable PRIMARY KEY CLUSTERED ([NhlReferenceId] , [SkaterId])
)
