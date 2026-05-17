USE millions_of_things;

-- To generate passwords or manage users use:
-- ./UserManagement.linq

-- Default testing user for regression and postman tests
-- PW: emmC2YNvh%9LtNMHWo#T
INSERT INTO public.user(
	username,
	password,
	is_allowed,
	firstname,
	lastname,
	emailaddress)
	VALUES (
		'Default-test-user'
		,'$2a$12$3RBRZfnq45AHcEqQ3LKqXeAfYKZbT8zf7yfX0vvKHtFN0svlxCiGW'
		,TRUE
		,'Default test user'
		,'Default test user'
		,'Default@testuser.com'
	);

-- Second default user for testing ownership in regression
-- PW: 6Uh@16n%jLZKOXZO
INSERT INTO public.user(
	username,
	password,
	is_allowed,
	firstname,
	lastname,
	emailaddress)
	VALUES (
		'Other-test-user'
		,'$2a$12$pVj/eJ2C4k9p170oJz6rROxx.l0aM.uZwX0buVugIAUp6YD5Ze5Ym'
		,TRUE
		,'Other test user'
		,'Other test user'
		,'Other@testuser.com'
	); 

-- SELECT * FROM public.user
