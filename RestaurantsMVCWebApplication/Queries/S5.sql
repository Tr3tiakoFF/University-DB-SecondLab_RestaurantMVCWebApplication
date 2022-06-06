SELECT Restaurants.Name FROM Restaurants
WHERE Restaurants.ChefID IN (
	SELECT Chefs.ChefID FROM Chefs
	WHERE Chefs.BirthCityID NOT IN (
		SELECT Cities.CityID FROM Cities
		WHERE Cities.Name = X));