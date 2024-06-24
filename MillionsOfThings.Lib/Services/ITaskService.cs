﻿using MillionsOfThings.Lib.Entities;

namespace MillionsOfThings.Lib.Services;

public interface ITaskService
{
  TaskEntity? GetTask(int taskId);

  IList<TaskEntity> GetAllForUser(int userId);

  TaskEntity Add(TaskEntity task);
  
  void Edit(TaskEntity task);

  void Remove(int taskId);
}
