-- Table: public.user

-- DROP TABLE IF EXISTS public.user;

CREATE TABLE IF NOT EXISTS public.user
(
    user_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    is_allowed boolean NOT NULL,
    username character varying(20) UNIQUE COLLATE pg_catalog."default" NOT NULL,
    password character varying(100) COLLATE pg_catalog."default" NOT NULL,
    firstname character varying(50) COLLATE pg_catalog."default" NOT NULL,
    lastname character varying(50) COLLATE pg_catalog."default" NOT NULL,
    emailaddress character varying(100) UNIQUE COLLATE pg_catalog."default" NOT NULL,
    security_role_id UUID NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT user_pkey PRIMARY KEY (user_id),
    CONSTRAINT fk_user_security_role
    	FOREIGN KEY (security_role_id)
    	REFERENCES public.security_role(security_role_id)
) TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.user
    OWNER to postgres;
