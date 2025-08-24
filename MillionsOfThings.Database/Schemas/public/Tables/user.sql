-- Table: public.user

-- DROP TABLE IF EXISTS public.user;

CREATE TABLE IF NOT EXISTS public.user
(
    user_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    username character varying(20) COLLATE pg_catalog."default" NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT user_pkey PRIMARY KEY (user_id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.user
    OWNER to postgres;
