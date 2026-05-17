-- Table: public.security_role

-- DROP TABLE IF EXISTS public.security_role;

-- Enable the extension (only need to do this once per database)
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS public.security_permission
(
    security_permission_id UUID PRIMARY KEY DEFAULT uuid_generate_v1() NOT NULL,
    permission text UNIQUE COLLATE pg_catalog."default" NOT NULL,
    description text COLLATE pg_catalog."default" NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone
) TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.security_permission
    OWNER to postgres;
