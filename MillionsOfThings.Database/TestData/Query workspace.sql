-- Select all lists and their tasks for user 1
SELECT
     l.list_id
    ,l.name
    ,t.task_id
    ,t.description
    ,lnk.list_task_link_id
		,lnk.task_order
FROM public.list AS l
    INNER JOIN public.list_task_link AS lnk
        ON l.list_id = lnk.list_id
    INNER JOIN public.task AS t
        ON t.task_id = lnk.task_id
WHERE l.user_id = 1

select
 	u.*
	,lnk.security_role_permission_link_id 
	,sr.role
	,sp.permission
from public.user u 
	inner join public.security_role_permission_link lnk
		on u.security_role_id = lnk.security_role_id 
	inner join security_role sr 
			on lnk.security_role_id = sr.security_role_id	
	inner join security_permission sp 
			on lnk.security_permission_id = sp.security_permission_id
			
select
	sp.permission
from public.security_role_permission_link lnk
	inner join security_permission sp 
			on lnk.security_permission_id = sp.security_permission_id
where lnk.security_role_id = '8cf369f9-2da9-44eb-8c27-959ef824406d'::uuid
