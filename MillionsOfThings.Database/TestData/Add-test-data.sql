INSERT INTO public.user(
	username)
	VALUES ('Default-test-User');

INSERT INTO public.category(user_id, name)
	VALUES 
	(1, 'Cat A'),
	(1, 'Cat B'),
	(1, 'Cat C');

SELECT * FROM public.category;
	
INSERT INTO public.task(
	user_id, 
	category_id, 
	description)
VALUES 
	(1, null, 'Uncategorized task'),
	(1, 1, 'Cat 1 task'),
	(1, 2, 'Cat 2 task'),
	(1, 3, 'Cat 3 task');

SELECT * FROM public.task;
	
INSERT INTO public.list(
	user_id, 
	name) 
VALUES
	(1, 'Main list');

SELECT * FROM public.list;
	
INSERT INTO public.list_task_link (
	 list_id
	,task_id
	,task_order)
SELECT
	 1 AS list_id
	,task_id
	,task_id AS task_order -- This is a hack for now, just demo data
FROM public.task
WHERE user_id = 1;

SELECT * FROM public.list_task_link;
