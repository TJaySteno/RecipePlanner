USE recipe_manager;

IF OBJECT_ID('tag', 'U') IS NOT NULL DROP TABLE tag;

CREATE TABLE tag (
	tag_id INT IDENTITY(1,1) PRIMARY KEY,

	display_value VARCHAR(50) NOT NULL UNIQUE,
	category VARCHAR(50) NULL, -- e.g., 'Dietary', 'Cuisine', 'Meal Type', 'Season/Holiday', 'Cooking Method'

	created_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
	modified_on DATETIME NOT NULL DEFAULT GETUTCDATE(),

	CONSTRAINT UQ_tag_display_value_category UNIQUE (display_value, category)
);

INSERT INTO tag (display_value, category) VALUES
	('Gluten-Free', 'Dietary'),
	('Dairy-Free', 'Dietary'),
	('Nut-Free', 'Dietary'),
	('Low-Carb', 'Dietary'),
	('Low-Sodium', 'Dietary'),
	('High-Protein', 'Dietary'),
	('Low-Fat', 'Dietary'),
	('Sugar-Free', 'Dietary'),
	('Diabetic-Friendly', 'Dietary'),
	('Heart-Healthy', 'Dietary'),
	('Vegetarian', 'Dietary'),
	('Vegan', 'Dietary'),
	('Plant-Based', 'Dietary'),
	('Italian', 'Cuisine'),
	('Mexican', 'Cuisine'),
	('Indian', 'Cuisine'),
	('Chinese', 'Cuisine'),
	('Japanese', 'Cuisine'),
	('Mediterranean', 'Cuisine'),
	('Stir-Fry', 'Cuisine'),
	('Casserole', 'Cuisine'),
	('Pasta', 'Cuisine'),
	('Breakfast', 'Meal Type'),
	('Lunch', 'Meal Type'),
	('Dinner', 'Meal Type'),
	('Appetizer', 'Meal Type'),
	('Snack', 'Meal Type'),
	('Dessert', 'Meal Type'),
	('Soup', 'Meal Type'),
	('Salad', 'Meal Type'),
	('Beverage', 'Meal Type'),
	('Christmas', 'Season/Holiday'),
	('Thanksgiving', 'Season/Holiday'),
	('Easter', 'Season/Holiday'),
	('Halloween', 'Season/Holiday'),
	('Summer', 'Season/Holiday'),
	('Winter', 'Season/Holiday'),
	('Spring', 'Season/Holiday'),
	('Fall', 'Season/Holiday'),
	('Quick & Easy', 'Cooking Method'),
	('Slow Cooker', 'Cooking Method'),
	('One-Pot', 'Cooking Method'),
	('Grilling', 'Cooking Method'),
	('Baking', 'Cooking Method'),
	('Roasting', 'Cooking Method');