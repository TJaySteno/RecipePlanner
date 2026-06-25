use recipe_manager

truncate table recipe_group_direction;
truncate table recipe_group_ingredient;
truncate table recipe_group;
truncate table recipe_tag;
delete from recipe;
delete from app_user;

-- APP_USER
insert into app_user (first_name, middle_name, last_name, primary_email, username, password_hash)
values
	('Luke', NULL, 'Skywalker', 'lastjedi@lucasfilms.com', 'lskywalker', 'placeholder'),
	('Sansa', NULL, 'Stark', 'sstark@winterfell.com', 'sstark', 'placeholder'),
	('Jon', NULL, 'Snow', 'jsnow998@thewall.com', 'jsnow998', 'placeholder');
/*
select * from app_user;
*/


-- RECIPE
insert into recipe (app_user_id, recipe_name, description, url, rating, difficulty, prep_time_in_minutes, cook_time_in_minutes, cool_time_in_minutes, servings, calories, fat_in_grams, carbohydrates_in_grams, protein_in_grams)
values
	((select app_user_id from app_user where username = 'lskywalker'), 'Blue Milk', NULL, 'https://www.starwars.com/blue_milk', 5.0, 3, 30, NULL, NULL, 4, 500, 20, 50, 30),
	((select app_user_id from app_user where username = 'sstark'), 'Lemon Cakes', NULL, 'https://www.tasteofthenorth.com/lemon_cakes', 4.5, 2, 45, 15, NULL, 6, 300, 10, 40, 20),
	((select app_user_id from app_user where username = 'jsnow998'), 'Jon''s Wildling Soup', NULL, 'https://www.tasteofthenorth.com/wildling_soup', 4.8, 4, 90, 45, NULL, 10, 700, 30, 80, 40);
/*
select * from recipe;
*/


-- RECIPE TAGS
-- Blue Milk
insert into recipe_tag (recipe_id, tag_id)
select top 1 (select recipe_id from recipe where recipe_name = 'Blue Milk'), tag_id from tag where display_value = 'Beverage' union all
-- Lemon Cakes
select top 1 (select recipe_id from recipe where recipe_name = 'Lemon Cakes'), tag_id from tag where display_value = 'Dessert' union all
select top 1 (select recipe_id from recipe where recipe_name = 'Lemon Cakes'), tag_id from tag where display_value = 'Baking' union all
-- Wildling Soup
select top 1 (select recipe_id from recipe where recipe_name = 'Jon''s Wildling Soup'), tag_id from tag where display_value = 'Soup' union all
select top 1 (select recipe_id from recipe where recipe_name = 'Jon''s Wildling Soup'), tag_id from tag where display_value = 'Dinner' union all
select top 1 (select recipe_id from recipe where recipe_name = 'Jon''s Wildling Soup'), tag_id from tag where display_value = 'Winter';
/*
select r.recipe_name, t.display_value tag, rt.* from recipe r
join recipe_tag rt on rt.recipe_id = r.recipe_id
join tag t on t.tag_id = rt.tag_id
order by r.recipe_id, t.display_value;
*/


-- TOP LEVEL GROUPS
insert into recipe_group (parent_recipe_group_id, recipe_id, display_order, group_name)
select NULL, recipe_id, 1, 'Ingredients' from recipe
union all
select NULL, recipe_id, 2, 'Directions' from recipe;
/*
select r.recipe_name, g.*
from recipe r
join recipe_group g on g.recipe_id = r.recipe_id
where g.parent_recipe_group_id is null
order by r.recipe_id, g.display_order;
*/


-- NEXT LEVEL GROUPS
-- Luke's Blue Milk
insert into recipe_group (parent_recipe_group_id, recipe_id, display_order, group_name)
select distinct rg.recipe_group_id, r.recipe_id, 1, 'Blue Milk'
from recipe r
join recipe_group rg on rg.recipe_id = r.recipe_id and rg.parent_recipe_group_id is null
where r.recipe_name = 'Blue Milk';
-- Sansa's Lemon Cakes
insert into recipe_group (parent_recipe_group_id, recipe_id, display_order, group_name)
select distinct rg.recipe_group_id, r.recipe_id, 1, 'Cake'
from recipe r
join recipe_group rg on rg.recipe_id = r.recipe_id and rg.parent_recipe_group_id is null
where r.recipe_name = 'Lemon Cakes';
insert into recipe_group (parent_recipe_group_id, recipe_id, display_order, group_name)
select distinct rg.recipe_group_id, r.recipe_id, 2, 'Frosting'
from recipe r
join recipe_group rg on rg.recipe_id = r.recipe_id and rg.parent_recipe_group_id is null
where r.recipe_name = 'Lemon Cakes';
-- Jon's Wildling Soup
insert into recipe_group (parent_recipe_group_id, recipe_id, display_order, group_name)
select distinct rg.recipe_group_id, r.recipe_id, 1, 'Soup'
from recipe r
join recipe_group rg on rg.recipe_id = r.recipe_id and rg.parent_recipe_group_id is null
where r.recipe_name = 'Jon''s Wildling Soup';
insert into recipe_group (parent_recipe_group_id, recipe_id, display_order, group_name)
select distinct rg.recipe_group_id, r.recipe_id, 2, 'Sides'
from recipe r
join recipe_group rg on rg.recipe_id = r.recipe_id and rg.parent_recipe_group_id is null
where r.recipe_name = 'Jon''s Wildling Soup';
/*
select r.recipe_name, g2.group_name parent_group_name, g.group_name, g.*
from recipe r
join recipe_group g on g.recipe_id = r.recipe_id
join recipe_group g2 on g2.recipe_group_id = g.parent_recipe_group_id
order by r.recipe_id, g2.display_order, g.display_order;
*/


-- RECIPE GROUP INGREDIENTS
declare @recipe_group_ingredient table (recipe_name varchar(60), group_name varchar(60), display_order int, ingredient_name varchar(100), quantity decimal(10,2), unit varchar(50), notes varchar(255));
insert into @recipe_group_ingredient (recipe_name, group_name, display_order, ingredient_name, quantity, unit, notes)
values
	('Blue Milk', 'Blue Milk', 1, 'Blue Milk', 500, 'ml', 'Freshly squeezed.'),
	('Lemon Cakes', 'Cake', 1, 'Lemon Juice', 50, 'ml', 'Freshly squeezed.'),
	('Lemon Cakes', 'Cake', 2, 'Sugar', 100, 'g', 'Granulated sugar for sweetness.'),
	('Lemon Cakes', 'Cake', 3, 'Graham Crackers', 200, 'g', 'Crushed for the crust.'),
	('Lemon Cakes', 'Frosting', 1, 'Butter', 100, 'g', 'Unsalted butter for the frosting.'),
	('Lemon Cakes', 'Frosting', 2, 'Powdered Sugar', 150, 'g', 'For a smooth frosting texture.'),
	('Jon''s Wildling Soup', 'Soup', 1, 'Water', 500, 'ml', 'For the soup base.'),
	('Jon''s Wildling Soup', 'Soup', 2, 'Game Meat', 200, 'g', 'Freshly hunted game meat.'),
	('Jon''s Wildling Soup', 'Soup', 3, 'Wildling Herbs', 30, 'g', 'A mix of herbs found in the North.'),
	('Jon''s Wildling Soup', 'Sides', 1, 'Bread', 100, 'g', 'To accompany the soup.'),
	('Jon''s Wildling Soup', 'Sides', 2, 'Cheese', 50, 'g', 'Aged cheese for topping.');

insert into recipe_group_ingredient (recipe_group_id, display_order, ingredient_name, quantity, unit, notes)
select distinct gb.recipe_group_id, i.display_order, i.ingredient_name, i.quantity, i.unit, i.notes
from @recipe_group_ingredient i
join recipe r on r.recipe_name = i.recipe_name
join recipe_group gt on gt.recipe_id = r.recipe_id and gt.group_name = 'Ingredients'
join recipe_group gb on gb.parent_recipe_group_id = gt.recipe_group_id and gb.recipe_id = r.recipe_id and gb.group_name = i.group_name;
/*
select distinct r.recipe_name, gt.group_name parent_group_name, gb.group_name group_name, gi.*
from recipe r
join recipe_group gt on gt.recipe_id = r.recipe_id
join recipe_group gb on gb.parent_recipe_group_id = gt.recipe_group_id and gb.recipe_id = r.recipe_id
join recipe_group_ingredient gi on gi.recipe_group_id = gb.recipe_group_id;
*/


-- RECIPE GROUP DIRECTIONS
declare @recipe_group_direction table (recipe_name varchar(60), group_name varchar(60), display_order int, direction_title varchar(60), direction_text varchar(255));
insert into @recipe_group_direction (recipe_name, group_name, display_order, direction_title, direction_text) values
	('Blue Milk', 'Blue Milk', 1, 'Serve', 'Pour Blue Milk into a cup.'),
	('Lemon Cakes', 'Cake', 1, 'Preparation', 'Preheat the oven to 350°F (175°C). Grease the baking pan.'),
	('Lemon Cakes', 'Cake', 2, 'Mix Batter', 'In a bowl, combine lemon juice and sugar. Mix until well combined.'),
	('Lemon Cakes', 'Cake', 3, 'Build Cake', 'Line baking pan with crust. Pour the batter into the pan and bake for 25-30 minutes.'),
	('Lemon Cakes', 'Cake', 4, 'Cool', 'Allow the cake to cool completely before frosting.'),
	('Lemon Cakes', 'Frosting', 1, 'Frosting', 'While the cake bakes, mix butter and powdered sugar in a bowl until creamy.'),
	('Lemon Cakes', 'Frosting', 2, 'Decorating', 'Once the cake is cool, spread the frosting over the cake and decorate as desired.'),
	('Jon''s Wildling Soup', 'Soup', 1, 'Cooking Soup', 'In a large pot, combine herbs and game meat. Simmer for 45 minutes.'),
	('Jon''s Wildling Soup', 'Soup', 2, 'Serving', 'Serve hot with bread and cheese on the side.');

insert into recipe_group_direction (recipe_group_id, display_order, direction_title, direction_text)
select distinct gb.recipe_group_id, d.display_order, d.direction_title, d.direction_text
from @recipe_group_direction d
join recipe r on r.recipe_name = d.recipe_name
join recipe_group gt on gt.recipe_id = r.recipe_id and gt.group_name = 'Directions'
join recipe_group gb on gb.parent_recipe_group_id = gt.recipe_group_id and gb.recipe_id = r.recipe_id and gb.group_name = d.group_name;
/*
select distinct r.recipe_name, gt.group_name parent_group_name, gb.group_name group_name, gi.*
from recipe r
join recipe_group gt on gt.recipe_id = r.recipe_id
join recipe_group gb on gb.parent_recipe_group_id = gt.recipe_group_id and gb.recipe_id = r.recipe_id
join recipe_group_direction gi on gi.recipe_group_id = gb.recipe_group_id;
*/