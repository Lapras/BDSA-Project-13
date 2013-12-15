/*
	Create unique index on movie and user in ratins table.
*/
CREATE UNIQUE INDEX rating_user_movie
ON [Rating] ([user_id], [movie_id]);