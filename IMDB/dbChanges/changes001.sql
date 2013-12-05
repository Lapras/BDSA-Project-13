CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [name] VARCHAR(50) NULL, 
    [email] VARCHAR(20) NULL, 
    [salt] VARCHAR(255) NULL, 
    [password] VARCHAR(255) NULL
)