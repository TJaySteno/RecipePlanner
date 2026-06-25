USE recipe_manager;

IF OBJECT_ID('recipe', 'U') IS NOT NULL DROP TABLE recipe;

CREATE TABLE recipe (
	recipe_id INT IDENTITY(1,1) PRIMARY KEY,

	app_user_id INT NOT NULL,
	recipe_name VARCHAR(50) NOT NULL,
	description VARCHAR(255) NULL,
	url VARCHAR(255) NULL,
	rating DECIMAL(2,1) NULL,
	difficulty INT NULL, -- 1-5 scale, 1 being easiest, 5 being hardest.
	prep_time_in_minutes INT NULL,
	cook_time_in_minutes INT NULL,
	cool_time_in_minutes INT NULL,	
	servings INT NULL,
	calories INT NULL,
	carbohydrates_in_grams INT NULL,
	protein_in_grams INT NULL,
	fat_in_grams INT NULL,

	created_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
	modified_on DATETIME NOT NULL DEFAULT GETUTCDATE(),

	CONSTRAINT FK_recipe_app_user FOREIGN KEY (app_user_id) REFERENCES app_user(app_user_id),
	CONSTRAINT CK_recipe_rating CHECK(rating between 0.0 AND 5.0),
	INDEX IX_recipe_recipe_name (recipe_name),
	INDEX IX_recipe_url (url),
);

/*
Eventually:
	favorite BIT DEFAULT 0,
	private_recipe BIT DEFAULT 0,
*/