@echo off

java -jar "C:\Program Files (x86)\apgdiff-2.4\apgdiff-2.4.jar" export.sql schema.sql > diff.sql

echo done
