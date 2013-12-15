CREATE TABLE [dbo].[Rating] (
	[id] int NOT NULL PRIMARY KEY,
	[user_Id] int NOT NULL,
	[movie_id] int NOT NULL,
	[rating] int NOT NULL CHECK([rating] IN('1','2','3','4','5','6','7','8','9','10')), 
);