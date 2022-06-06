SELECT Chefs.LastName, Chefs.FirstName, Chefs.MiddleName FROM Chefs
WHERE EXISTS (
	SELECT * FROM Restaurants AS R
	WHERE R.ChefID = Chefs.ChefID AND R.CityID IN (
		SELECT C.CityID FROM Cities AS C
		WHERE NOT EXISTS (
			SELECT * FROM Restaurants
			WHERE Restaurants.CityID NOT IN (
				SELECT Cities.CityID FROM Cities
				WHERE Cities.CountryID IN (
					SELECT Countries.CountryID FROM Countries
					WHERE Countries.Name = @)))));