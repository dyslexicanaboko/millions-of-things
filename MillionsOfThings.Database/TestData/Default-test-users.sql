USE millions_of_things;

/* To generate passwords or manage users use:
 * ./UserManagement.linq
 * 
 * The `security_role_id`s shown below are from predetermined test data.
 * `8cf369f9-2da9-44eb-8c27-959ef824406c` is the `Standard` user role.
 * */

-- First standard testing user for regression and postman tests.
-- This is formerly known as the default test user.
-- PW: emmC2YNvh%9LtNMHWo#T
INSERT INTO public.user(
	username,
	password,
	security_role_id,
	is_allowed,
	firstname,
	lastname,
	emailaddress)
	VALUES (
		'Default-test-user'
		,'$2a$12$3RBRZfnq45AHcEqQ3LKqXeAfYKZbT8zf7yfX0vvKHtFN0svlxCiGW'
		,'8cf369f9-2da9-44eb-8c27-959ef824406c'::uuid
		,TRUE
		,'Default test user'
		,'Default test user'
		,'Default@testuser.com'
	);

-- Second standard testing user for testing ownership in regression
-- This was also formely known as a default user, but not anymore.
-- PW: 6Uh@16n%jLZKOXZO
INSERT INTO public.user(
	username,
	password,
	security_role_id,
	is_allowed,
	firstname,
	lastname,
	emailaddress)
	VALUES (
		'Other-test-user'
		,'$2a$12$pVj/eJ2C4k9p170oJz6rROxx.l0aM.uZwX0buVugIAUp6YD5Ze5Ym'
		,'8cf369f9-2da9-44eb-8c27-959ef824406c'::uuid
		,TRUE
		,'Other test user'
		,'Other test user'
		,'Other@testuser.com'
	); 

-- First administrative user
-- PW: tmyX1zySyOSTeLqhLKD2
INSERT INTO public.user(
	username,
	password,
	security_role_id,
	is_allowed,
	firstname,
	lastname,
	emailaddress)
	VALUES (
		'Admin-test-user'
		,'$2a$12$mwXaSE0JYT/G5Rfx4C2g5O7dWhj8TsYsZixMgUNlKHFcmUE.cpSgG'
		,'8cf369f9-2da9-44eb-8c27-959ef824406d'::uuid
		,TRUE
		,'Admin test user'
		,'Admin test user'
		,'admin@testuser.com'
	);

-- SELECT * FROM public.user
