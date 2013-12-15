/*
	Create a trigger that updates the avg_ranking for a movie upon updating a row in the rating table.
*/
CREATE TRIGGER update_avg_rank_on_update
ON [Rating]
AFTER UPDATE
AS
DECLARE @MovieId int, @Avg_rating float
SET @MovieId = (SELECT movie_id FROM inserted)
SET @Avg_rating = (
	SELECT
		AVG([Rating].[rating] + 0.0) AS avg_rating
	FROM
		[Rating]
	WHERE
		[Rating].movie_id = @MovieId
	GROUP BY [Rating].movie_id)

UPDATE [Movies]
SET Avg_rating = @Avg_rating
WHERE Id = @MovieId ;