USE recipe_manager;

IF OBJECT_ID('recipe_group_direction', 'U') IS NOT NULL DROP TABLE recipe_group_direction;

CREATE TABLE recipe_group_direction (
	recipe_group_direction_id INT IDENTITY(1,1) PRIMARY KEY,
	recipe_group_id INT NOT NULL,
	display_order INT NOT NULL, -- The order in which the direction should be displayed in the recipe.
	
	direction_title VARCHAR(60) NULL, -- Optional title for the direction.
	direction_text VARCHAR(255) NOT NULL, -- The text of the direction.

	created_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
	modified_on DATETIME NOT NULL DEFAULT GETUTCDATE(),

	CONSTRAINT FK_recipe_group_direction_recipe_group_id FOREIGN KEY (recipe_group_id) REFERENCES recipe_group(recipe_group_id),
	CONSTRAINT UQ_recipe_group_direction_display_order UNIQUE (recipe_group_id, display_order),
	CONSTRAINT CK_recipe_group_direction_display_order CHECK(display_order > 0),
);