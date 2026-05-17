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
) TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.refresh_token
    OWNER to postgres;