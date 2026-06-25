USE recipe_manager;

IF OBJECT_ID('recipe_tag', 'U') IS NOT NULL DROP TABLE recipe_tag;

CREATE TABLE recipe_tag (
	recipe_id INT NOT NULL,
	tag_id INT NOT NULL,

	created_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
	modified_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
	
	PRIMARY KEY (recipe_id, tag_id),
	CONSTRAINT FK_recipe_tag_recipe_id FOREIGN KEY (recipe_id) REFERENCES recipe(recipe_id),
	CONSTRAINT FK_recipe_tag_tag_id FOREIGN KEY (tag_id) REFERENCES tag(tag_id)
);