@echo off

echo Exporting database schema to export.sql from docker container 'postgres'...

docker exec -i postgres pg_dump -U postgres --schema-only --no-owner --no-privileges millions_of_things > export.sql

echo done
