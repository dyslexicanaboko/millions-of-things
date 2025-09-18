select * from public.category
select * from public.task
select * from "public"."category"
select * from millions_of_things.public.category
select * from "millions_of_things"."public"."category"
select now()
select * from public.task t 


SELECT
    Task_id,
    User_id,
    Category_id,
    Description,
    Is_finished,
    Finished_on,
    Created_on,
    Modified_on
FROM public.task
-- delete stuff after id 4 to reset the testing

delete from public.task where task_id > 4
delete from public.category where category_id > 3

SELECT EXISTS (
  SELECT 1
  FROM public.category
  WHERE user_id = 1 AND name = 'string2'
);

do $$
declare
	Blah int := 0;
begin
perform Blah;
end $$;

ALTER TABLE public.category
ADD CONSTRAINT category_user_id_name_unique UNIQUE (user_id, name);

select * from public.category

UPDATE public.category SET 
	name = 'string3',
	modified_on = null
WHERE category_id = 10
