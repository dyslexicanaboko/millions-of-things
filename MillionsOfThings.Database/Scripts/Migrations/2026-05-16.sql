-- This was not created using a diff. Manual updates.

-- Backfilling data
update public.user set 
	 firstname='Default test user'
	,lastname='Default test user'
	,emailaddress='Default@testuser.com'
where user_id = 1

update public.user set 
	 firstname='Other test user'
	,lastname='Other test user'
	,emailaddress='Other@testuser.com'
where user_id = 2

-- Adding unique constraints on username and emailaddress individually
ALTER TABLE public.user
ADD CONSTRAINT uq_user_username UNIQUE (username);

ALTER TABLE public.user
ADD CONSTRAINT uq_user_emailaddress UNIQUE (emailaddress);
