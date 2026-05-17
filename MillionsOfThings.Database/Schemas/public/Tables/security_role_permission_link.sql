-- Table: public.security_role_permission_link

-- DROP TABLE IF EXISTS public.security_role_permission_link;

-- Enable the extension (only need to do this once per database)
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS public.security_role_permission_link
(
    security_role_permission_link_id UUID PRIMARY KEY DEFAULT uuid_generate_v1() NOT NULL,
    security_role_id UUID NOT NULL,
    security_permission_id UUID NOT NULL,
    created_on timestamp(0) without time zone NOT NULL DEFAULT (now())::timestamp without time zone,
    modified_on timestamp(0) without time zone,
    CONSTRAINT uq_security_link_role_permission 
    	UNIQUE (security_role_id, security_permission_id),
    CONSTRAINT fk_security_link_role 
    	FOREIGN KEY (security_role_id)
    	REFERENCES public.security_role(security_role_id),
	CONSTRAINT fk_security_link_permission 
    	FOREIGN KEY (security_permission_id)
    	REFERENCES public.security_permission(security_permission_id)
) TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.security_role_permission_link
    OWNER to postgres;
