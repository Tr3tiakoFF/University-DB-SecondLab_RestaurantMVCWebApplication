SELECT COUNT(Restaurants.RestaurantID) FROM Restaurants
WHERE Restaurants.CityID IN (
	SELECT Cities.CityID FROM Cities
	WHERE Cities.CountryID IN (
		SELECT Countries.CountryID FROM Countries
		WHERE Countries.Name = X));