# MovieRecs
Recommend movies and stuff

Current dependencies:
http://json.codeplex.com/

Json:

GET: No params

Result: Feature names, then a list of movies. Movies has a crapload of single entities, and a list of Feature values Sample:

{"FeatureNames":["Feature1","Feature2","Feature3","Feature4","Feature5","Feature6","Feature7","Feature8","Feature9","Feature10"],"Movies":[{"Title":"Toy Story","Year":"1995","Poster":"http://ia.media-imdb.com/images/M/MV5BMTgwMjI4MzU5N15BMl5BanBnXkFtZTcwMTMyNTk3OA@@._V1_SX300.jpg","Plot":"A cowboy doll is profoundly threatened and jealous when a new spaceman figure supplants him as top toy in a boy's room.","Released":"22 Nov 1995","imdbID":"tt0114709","Genre":"Animation, Adventure, Comedy","Director":"John Lasseter","Id":1,"Popularity":53059,"Features":[0.049364659958223187,0.21736293715302038,0.33440119276493846,0.070964491493517759,0.98832382587172274,0.37899766134982821,0.35741794638215468,0.59267524145202488,0.85783326153542527,0.617342683774113]},{"Title":"Jumanji","Year":"1995","Poster":"http://ia.media-imdb.com/images/M/MV5BMTk5MjAyNTM4Ml5BMl5BanBnXkFtZTgwMjY0MDI0MjE@._V1_SX300.jpg","Plot":"When two kids find and play a magical board game, they release a man trapped for decades in it and a host of dangers that can only be stopped by finishing the game.","Released":"15 Dec 1995","imdbID":"tt0113497","Genre":"Adventure, Family, Fantasy","Director":"Joe Johnston","Id":2,"Popularity":22466,"Features":[0.23449978289869605,0.16523108871897269,0.92358385348859418,0.2977653887578125,0.12278608052189745,0.9891190440343316,0.95402229575161934,0.041083477456627174,0.31861465252871374,0.70016391794204891]}]}

POST: Params:

nresults: number of results to get. (int)
features: value of each feature in order. (list<double>)
weights: value of weight of each feature in order (list<double>)

Result: List of movies, following same schema as get
