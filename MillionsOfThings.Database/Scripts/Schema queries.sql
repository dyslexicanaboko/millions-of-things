--ALTER TABLE public.types_table ADD CONSTRAINT types_table_pk PRIMARY KEY (primary_key_int);

-- This provides the full schema info on JUST the table
SELECT 
    column_name, 
    data_type, 
    is_nullable, 
    column_default, 
    character_maximum_length, 
    numeric_precision, 
    numeric_scale
FROM 
    information_schema.columns
WHERE 
    table_name = 'user'
    AND table_schema = 'public';

SELECT
    kcu.column_name
FROM
    information_schema.table_constraints tc
    JOIN information_schema.key_column_usage kcu
      ON tc.constraint_name = kcu.constraint_name
      AND tc.table_schema = kcu.table_schema
WHERE
    tc.constraint_type = 'PRIMARY KEY'
    AND tc.table_name = 'types_table'
    AND tc.table_schema = 'public';

-- public.types_table

SELECT * FROM public.user LIMIT 0
