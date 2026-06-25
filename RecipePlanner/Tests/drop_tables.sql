USE recipe_manager;

IF OBJECT_ID('recipe_group_direction', 'U') IS NOT NULL DROP TABLE recipe_group_direction;
IF OBJECT_ID('recipe_group_ingredient', 'U') IS NOT NULL DROP TABLE recipe_group_ingredient;
IF OBJECT_ID('recipe_group', 'U') IS NOT NULL DROP TABLE recipe_group;
IF OBJECT_ID('recipe_tag', 'U') IS NOT NULL DROP TABLE recipe_tag;
IF OBJECT_ID('tag', 'U') IS NOT NULL DROP TABLE tag;
IF OBJECT_ID('recipe', 'U') IS NOT NULL DROP TABLE recipe;
IF OBJECT_ID('app_user', 'U') IS NOT NULL DROP TABLE app_user;