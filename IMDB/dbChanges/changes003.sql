CREATE TRIGGER update_avg_rank
ON [Rating]
AFTER INSERT
AS
DECLARE @MovieId int, @Avg_rating float
SET @MovieId = (SELECT movie_id FROM inserted)
SET @Avg_rating = (
	SELECT
		AVG([Rating].[rating]) AS avg_rating
	FROM
		[Rating]
	WHERE
		[Rating].movie_id = @MovieId
	GROUP BY [Rating].movie_id)

UPDATE [Movies]
SET Avg_rating = @Avg_rating
WHERE Id = @MovieId 
;