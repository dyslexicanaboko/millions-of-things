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
