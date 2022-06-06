SELECT Cities.Name FROM Cities
WHERE NOT EXISTS (
	SELECT * FROM Restaurants
	WHERE Restaurants.CityID = Cities.CityID AND Restaurants.Name = X);