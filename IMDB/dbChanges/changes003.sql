/*
	Create a trigger that updates the avg_ranking for a movie upon insert in the rating table.
*/
CREATE TRIGGER update_avg_rank
ON [Rating]
AFTER INSERT
AS
DECLARE @MovieId int, @Avg_rating float
SET @MovieId = (SELECT movie_id FROM inserted)
SET @Avg_rating = (
	SELECT
		CAST(AVG([Rating].[rating]) AS float) AS avg_rating
	FROM
		[Rating]
	WHERE
		[Rating].movie_id = @MovieId
	GROUP BY [Rating].movie_id)

UPDATE [Movies]
SET Avg_rating = @Avg_rating
WHERE Id = @MovieId;