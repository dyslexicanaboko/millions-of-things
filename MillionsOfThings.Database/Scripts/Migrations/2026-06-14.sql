CREATE EXTENSION IF NOT EXISTS citext;

ALTER TABLE public.user 
ALTER COLUMN username TYPE CITEXT,
ALTER COLUMN emailaddress TYPE CITEXT;

ALTER TABLE public.user
ADD CONSTRAINT username_length CHECK (LENGTH(username) <= 20),
ADD CONSTRAINT emailaddress_length CHECK (LENGTH(emailaddress) <= 100);

ALTER TABLE public.security_role  
ALTER COLUMN role TYPE CITEXT;
