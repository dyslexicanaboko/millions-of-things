USE millions_of_things;

-- Default testing user for regression and postman tests
-- PW: emmC2YNvh%9LtNMHWo#T
INSERT INTO public.user(
	username,
	password,
	is_allowed)
	VALUES (
		'Default-test-user'
		,'$2a$12$3RBRZfnq45AHcEqQ3LKqXeAfYKZbT8zf7yfX0vvKHtFN0svlxCiGW'
		,true
	);

-- Second default user for testing ownership in regression
-- PW: 6Uh@16n%jLZKOXZO
INSERT INTO public.user(
	username,
	password,
	is_allowed)
	VALUES (
		'Other-test-user'
		,'$2a$12$pVj/eJ2C4k9p170oJz6rROxx.l0aM.uZwX0buVugIAUp6YD5Ze5Ym'
		,true
	); 

-- SELECT * FROM public.user
