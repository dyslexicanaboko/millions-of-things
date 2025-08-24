select * from public.category
select * from "public"."category"
select * from millions_of_things.public.category
select * from "millions_of_things"."public"."category"

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
			
do $$
declare
	Blah int := 0;
begin
perform Blah;
end $$;