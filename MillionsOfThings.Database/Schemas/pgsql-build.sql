-- docker ps, to find the container name
-- docker exec -i postgres psql -U postgres -d millions_of_things < build.sql
-- This will execute each file in the order shown.
-- This is good for initial load only
\i public/Tables/user.sql
\i public/Tables/refresh_token.sql
\i public/Tables/category.sql
\i public/Tables/task.sql
\i public/Tables/list.sql
\i public/Tables/list_task_link.sql
