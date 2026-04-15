-- Table: public.user

-- DROP TABLE IF EXISTS public.user;

CREATE TABLE IF NOT EXISTS public.user
(
    user_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    is_allowed boolean NOT NULL,
    username character varying(20) COLLATE pg_catalog."default" NOT NULL,
    password character varying(100) COLLATE pg_catalog."default" NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT user_pkey PRIMARY KEY (user_id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.user
    OWNER to postgres;
-- Table: public.refresh_token

-- DROP TABLE IF EXISTS public.refresh_token;

CREATE TABLE IF NOT EXISTS public.refresh_token
(
    refresh_token_id uuid NOT NULL DEFAULT gen_random_uuid(),
    user_id integer NOT NULL,
    token character varying(255) COLLATE pg_catalog."default" NOT NULL,
    created_by_ip character varying(39) COLLATE pg_catalog."default" NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT refresh_token_pkey PRIMARY KEY (refresh_token_id),
    CONSTRAINT refresh_token_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.user (user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.refresh_token
    OWNER to postgres;-- Table: public.category

-- DROP TABLE IF EXISTS public.category;

CREATE TABLE IF NOT EXISTS public.category
(
    category_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    user_id integer NOT NULL,
    name character varying(20) COLLATE pg_catalog."default" NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT category_pkey PRIMARY KEY (category_id),
    CONSTRAINT category_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.user (user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT category_user_id_name_unique UNIQUE (user_id, name)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.category
    OWNER to postgres;
-- Table: public.task

-- DROP TABLE IF EXISTS public.task;

CREATE TABLE IF NOT EXISTS public.task
(
    task_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    user_id integer NOT NULL,
    category_id integer,
    description character varying(255) COLLATE pg_catalog."default" NOT NULL,
    is_finished boolean NOT NULL DEFAULT false,
    finished_on timestamp(0) without time zone,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT task_pkey PRIMARY KEY (task_id),
    CONSTRAINT task_category_id_fkey FOREIGN KEY (category_id)
        REFERENCES public.category (category_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT task_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.user (user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.task
    OWNER to postgres;
-- Table: public.list

-- DROP TABLE IF EXISTS public.list;

CREATE TABLE IF NOT EXISTS public.list
(
    list_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    user_id integer NOT NULL,
    name character varying(20) COLLATE pg_catalog."default" NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT list_pkey PRIMARY KEY (list_id),
    CONSTRAINT list_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.user (user_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.list
    OWNER to postgres;
-- Table: public.list_task_link

-- DROP TABLE IF EXISTS public.list_task_link;

CREATE TABLE IF NOT EXISTS public.list_task_link
(
    list_task_link_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    list_id integer NOT NULL,
    task_id integer NOT NULL,
    task_order integer NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT list_task_link_pkey PRIMARY KEY (list_task_link_id),
    CONSTRAINT list_task_link_list_id_fkey FOREIGN KEY (list_id)
        REFERENCES public.list (list_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT list_task_link_task_id_fkey FOREIGN KEY (task_id)
        REFERENCES public.task (task_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID    
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.list_task_link
    OWNER to postgres;
