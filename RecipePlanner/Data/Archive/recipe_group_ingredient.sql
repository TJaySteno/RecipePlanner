USE recipe_manager;

IF OBJECT_ID('recipe_group_ingredient', 'U') IS NOT NULL DROP TABLE recipe_group_ingredient;

CREATE TABLE recipe_group_ingredient (
	recipe_group_ingredient_id INT IDENTITY(1,1) PRIMARY KEY,
	recipe_group_id INT NOT NULL,
	display_order INT NOT NULL,
	
	ingredient_name VARCHAR(100) NOT NULL,
	quantity DECIMAL(10,2) NOT NULL,
	unit VARCHAR(50) NOT NULL,
	notes VARCHAR(255) NULL,

	created_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
	modified_on DATETIME NOT NULL DEFAULT GETUTCDATE(),

	CONSTRAINT FK_recipe_group_ingredient_recipe_group_id FOREIGN KEY (recipe_group_id) REFERENCES recipe_group(recipe_group_id),
	CONSTRAINT UQ_recipe_group_ingredient_display_order UNIQUE (recipe_group_id, display_order),
	CONSTRAINT CK_recipe_group_ingredient_display_order CHECK(display_order > 0),
	CONSTRAINT CK_recipe_group_ingredient_quantity CHECK(quantity > 0),
);

/*
Todo: Is there a way to CHECK whether the owner recipe_group_id has type of 'Ingredients'?

Later:
	ingredient_id FK (nullable, for ingredients not in the ingredients table)
	store everything in metric and convert to imperial for display?
	verify how I want substitutions to work/display.
	separate table for units? (e.g., unit_id FK, unit_name, unit_type, conversion_factor_to_base_unit)
	substitutions table? (e.g., substitution_id FK, ingredient_id FK, substitution_ingredient_id FK, notes)
*/