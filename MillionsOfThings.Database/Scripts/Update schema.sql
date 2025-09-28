select * from public.user u 

-- Temp table
CREATE TABLE IF NOT EXISTS public.user2
(
    user_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    is_allowed boolean NOT NULL,
    username character varying(20) COLLATE pg_catalog."default" NOT NULL,
    password character varying(100) COLLATE pg_catalog."default" NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone
)

insert into "user2" (
 user_id
,is_allowed
 ,username
,password
,created_on
,modified_on
) OVERRIDING SYSTEM VALUE
select
 user_id
,true as is_allowed
 ,username
,'' as password
,created_on
,modified_on
from public.user u

-- Checking the transfer
select * from public.user2

-- Dropping the original table and dependencies
alter table public.category drop constraint category_user_id_fkey;
alter table public.list drop constraint list_user_id_fkey;
alter table public.task drop constraint task_user_id_fkey;
DROP TABLE public.user;

-- Renaming the temp table to the actual table
ALTER TABLE "user2"	RENAME TO "user";

-- Check the rename
select * from public.user

-- Add constraint back in when finished
alter table public.user add CONSTRAINT user_pkey PRIMARY KEY (user_id);

alter table public.category add constraint category_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.user (user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID;

alter table public.list add constraint list_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.user (user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID;

alter table public.task add constraint task_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.user (user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID;
