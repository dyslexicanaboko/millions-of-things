USE millions_of_things;

INSERT INTO public.user(
	username,
	password )
	VALUES (
		'Default-test-User'
		,'$2a$12$3RBRZfnq45AHcEqQ3LKqXeAfYKZbT8zf7yfX0vvKHtFN0svlxCiGW'
	);

-- SELECT * FROM public.user

return

-- Added password property at a later time
-- Unhashed pw: emmC2YNvh%9LtNMHWo#T
update public.user set password = '$2a$12$3RBRZfnq45AHcEqQ3LKqXeAfYKZbT8zf7yfX0vvKHtFN0svlxCiGW' where user_id = 1

