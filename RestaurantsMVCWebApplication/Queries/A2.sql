SELECT R.Name FROM Restaurants AS R
WHERE R.IconicDishID IN (
	SELECT Dishes.DishID FROM Dishes
	WHERE NOT EXISTS (
		SELECT * FROM Products
		WHERE EXISTS (
			SELECT * FROM Dishes_Products
			WHERE Dishes_Products.ProductID = Products.ProductID AND Dishes_Products.DishID IN (
				SELECT Restaurants.IconicDishID FROM Restaurants
				WHERE Restaurants.Name = @)) AND
		NOT EXISTS (
			SELECT * FROM Dishes_Products
			WHERE Dishes_Products.ProductID = Products.ProductID AND Dishes_Products.DishID = Dishes.DishID)));