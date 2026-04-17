-- Not sure why the original sequence has the number 2 in it.
-- Removed the schema `public.` for this to run, not sure why it wouldn't work with it.
ALTER SEQUENCE user2_user_id_seq RENAME TO user_user_id_seq;

-- Adding three columns to the user table, these are not nullable, so have to add as null, fill, then change to not null
ALTER TABLE public.user
	add	firstname character varying(50) COLLATE pg_catalog."default" null,
    add lastname character varying(50) COLLATE pg_catalog."default" NULL,
	add emailaddress character varying(100) COLLATE pg_catalog."default" null;

-- Fill data
update public.user set firstname = '', lastname = '', emailaddress = ''

-- Change to not null
ALTER TABLE public.USER 
	alter firstname set not null,
    alter lastname set not null,
	alter emailaddress set not null;
