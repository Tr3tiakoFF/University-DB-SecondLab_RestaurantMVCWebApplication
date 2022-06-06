SELECT Chefs.LastName, Chefs.FirstName, Chefs.MiddleName FROM Chefs
WHERE EXISTS (
	SELECT * FROM Dishes
	WHERE (
		SELECT COUNT(*) FROM Dishes_Products
		WHERE Dishes_Products.DishID = Dishes.DishID) >= X AND
	EXISTS (
		SELECT * FROM Restaurants
		WHERE Restaurants.ChefID = Chefs.ChefID AND Restaurants.IconicDishID = Dishes.DishID));