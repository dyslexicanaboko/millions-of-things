-- This was not created using a diff. Manual updates.

-- Adding the column as NULL first
ALTER TABLE public.USER ADD security_role_id UUID NULL

-- Backfilling data
update public.user set 
	 security_role_id='8cf369f9-2da9-44eb-8c27-959ef824406c'::uuid
	,modified_on=now()
where user_id = 1;

update public.user set 
	 security_role_id='8cf369f9-2da9-44eb-8c27-959ef824406c'::uuid
	,modified_on=now()
where user_id = 2;

-- Adding unique constraints on username and emailaddress individually
ALTER TABLE public.USER alter column security_role_id SET NOT null;

ALTER TABLE public.user
ADD CONSTRAINT fk_user_security_role
    	FOREIGN KEY (security_role_id)
    	REFERENCES public.security_role(security_role_id);
