-- Run `Default-test-users.sql` first

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

SELECT gen_random_uuid();

-- 2026-05-17 This is future seed data, going to leave it here for now
MERGE INTO public.security_role AS t
USING (VALUES 
    ('8cf369f9-2da9-44eb-8c27-959ef824406c'::uuid, 'Standard', 'Standard user who would just be concerned with their own account.'),
    ('8cf369f9-2da9-44eb-8c27-959ef824406d'::uuid, 'Administrator', 'User that can manage everything, including other user accounts.')
) AS s(security_role_id, role, description)
ON t.security_role_id = s.security_role_id
WHEN matched 
	and s.description <> t.description THEN
    UPDATE SET 
        description = s.description,
        modified_on = now()
WHEN NOT MATCHED THEN
    INSERT (security_role_id, role, description)
    VALUES (s.security_role_id, s.role, s.description);

select * from public.security_role

MERGE INTO public.security_permission AS t
USING (VALUES 
    ('6e880f03-bbcf-4dd4-b529-312074159e00'::uuid, 'user.manage.full', 'User can manage all users including self.'),
    ('6e880f03-bbcf-4dd4-b529-312074159e01'::uuid, 'user.manage.self', 'User can only manage their own account.')
) AS s(security_permission_id, permission, description)
ON t.security_permission_id = s.security_permission_id
WHEN matched 
	and s.description <> t.description THEN
    UPDATE SET 
        description = s.description,
        modified_on = now()
WHEN NOT MATCHED THEN
    INSERT (security_permission_id, permission, description)
    VALUES (s.security_permission_id, s.permission, s.description);

select * from public.security_permission

MERGE INTO public.security_role_permission_link AS t
USING (VALUES 
    ('58c2688c-a6bc-4cff-9681-f4d37d10494f'::uuid, '8cf369f9-2da9-44eb-8c27-959ef824406c'::uuid, '6e880f03-bbcf-4dd4-b529-312074159e00'::uuid),
    ('1dda98fa-4688-4371-9e63-21f20882c930'::uuid, '8cf369f9-2da9-44eb-8c27-959ef824406d'::uuid, '6e880f03-bbcf-4dd4-b529-312074159e01'::uuid)
) AS s(security_role_permission_link_id, security_role_id, security_permission_id)
ON t.security_role_permission_link_id = s.security_role_permission_link_id
WHEN matched 
	and (s.security_role_id <> t.security_role_id 
	or s.security_permission_id <> t.security_permission_id) 
	THEN
    UPDATE SET 
        security_role_id = s.security_role_id,
        security_permission_id = s.security_permission_id,
        modified_on = now()
WHEN NOT MATCHED THEN
    INSERT (security_role_permission_link_id, security_role_id, security_permission_id)
    VALUES (s.security_role_permission_link_id, s.security_role_id, s.security_permission_id);

select * from public.security_role_permission_link

select
 	 lnk.security_role_permission_link_id 
	,sr.role
	,sp.permission
from public.security_role_permission_link lnk
	inner join security_role sr 
			on lnk.security_role_id = sr.security_role_id	
	inner join security_permission sp 
			on lnk.security_permission_id = sp.security_permission_id




