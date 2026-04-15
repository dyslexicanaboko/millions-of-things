#!/bin/bash

cat \
   public/Tables/user.sql \
   public/Tables/refresh_token.sql \
   public/Tables/category.sql \
   public/Tables/task.sql \
   public/Tables/list.sql \
   public/Tables/list_task_link.sql \
   > schema.sql
