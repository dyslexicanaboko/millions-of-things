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
