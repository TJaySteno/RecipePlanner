USE recipe_manager;

IF OBJECT_ID('app_user', 'U') IS NOT NULL DROP TABLE app_user;

CREATE TABLE app_user (
	app_user_id INT IDENTITY(1,1) PRIMARY KEY,
	
	first_name VARCHAR(100) NULL,
	middle_name VARCHAR(100) NULL,
	last_name VARCHAR(100) NULL,
	
	primary_email VARCHAR(255) NOT NULL UNIQUE,
	username VARCHAR(100) NOT NULL UNIQUE,
	password_hash VARCHAR(255) NOT NULL,

	created_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
	modified_on DATETIME NOT NULL DEFAULT GETUTCDATE(),
);

/*
Eventually:
	profile picture
	preferences
	dietary restrictions
	For email notifications, we'll need TZ info:
		gmt_offset INT NOT NULL DEFAULT 0, -- The user's timezone offset from GMT in hours. For example, -5 for EST (UTC-5).
		dst BIT NOT NULL DEFAULT 1, -- Whether the user observes Daylight Saving Time (DST). 1 for yes, 0 for no.
*/