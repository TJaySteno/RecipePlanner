USE recipe_manager;

IF OBJECT_ID('recipe_group', 'U') IS NOT NULL DROP TABLE recipe_group;

CREATE TABLE recipe_group (
	recipe_group_id INT IDENTITY(1,1) PRIMARY KEY,
	
	parent_recipe_group_id INT NULL, -- NULL for top-level groups.
	recipe_id INT NOT NULL,
	display_order INT NOT NULL,
	group_name VARCHAR(20) NOT NULL, -- Top-level: 'Ingredients' or 'Directions'. Next-level, e.g.: 'Cake', 'Frosting', 'Toppings'.

	created_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
	modified_on DATETIME NOT NULL DEFAULT GETUTCDATE(),

	CONSTRAINT FK_recipe_group_recipe_id FOREIGN KEY (recipe_id) REFERENCES recipe(recipe_id),
	CONSTRAINT FK_recipe_group_parent_recipe_group_id FOREIGN KEY (parent_recipe_group_id) REFERENCES recipe_group(recipe_group_id),
	
	CONSTRAINT UQ_recipe_group_display_order UNIQUE (recipe_id, parent_recipe_group_id, display_order),
	
	CONSTRAINT CK_recipe_group_group_name
		CHECK( (parent_recipe_group_id IS NULL AND group_name IN ('Ingredients', 'Directions'))
			OR (parent_recipe_group_id IS NOT NULL AND group_name NOT IN ('Ingredients', 'Directions'))),
	
	INDEX IX_group_name (group_name),
);

/*
Later:
	Use application logic to enforce 1 'Ingredients' and 1 'Directions' group per recipe.
*/