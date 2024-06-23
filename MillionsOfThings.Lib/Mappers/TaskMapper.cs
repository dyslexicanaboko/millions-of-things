﻿using MillionsOfThings.Lib.Entities;
using MillionsOfThings.Lib.Models;

namespace MillionsOfThings.Lib.Mappers
{
  public class TaskMapper
    : MapperBase, ITaskMapper
  {
    public TaskEntity ToEntity(TaskModel model)
    {
      var entity = new TaskEntity();
      entity.TaskId = model.TaskId;
      entity.UserId = model.UserId;
      entity.CategoryId = model.CategoryId;
      entity.Description = model.Description;
      entity.IsFinished = model.IsFinished;
      entity.FinishedOn = model.FinishedOn;
      entity.CreatedOn = model.CreatedOn;
      entity.ModifiedOn = model.ModifiedOn;

      return entity;
    }

    public TaskModel ToModel(TaskEntity entity)
    {
      var model = new TaskModel();
      model.TaskId = entity.TaskId;
      model.UserId = entity.UserId;
      model.CategoryId = entity.CategoryId;
      model.Description = entity.Description;
      model.IsFinished = entity.IsFinished;
      model.FinishedOn = entity.FinishedOn;
      model.CreatedOn = entity.CreatedOn;
      model.ModifiedOn = entity.ModifiedOn;

      return model;
    }

    public TaskEntity ToEntity(ITask target)
    {
      var entity = new TaskEntity();
      entity.TaskId = target.TaskId;
      entity.UserId = target.UserId;
      entity.CategoryId = target.CategoryId;
      entity.Description = target.Description;
      entity.IsFinished = target.IsFinished;
      entity.FinishedOn = target.FinishedOn;
      entity.CreatedOn = target.CreatedOn;
      entity.ModifiedOn = target.ModifiedOn;

      return entity;
    }

    public TaskModel ToModel(ITask target)
    {
      var model = new TaskModel();
      model.TaskId = target.TaskId;
      model.UserId = target.UserId;
      model.CategoryId = target.CategoryId;
      model.Description = target.Description;
      model.IsFinished = target.IsFinished;
      model.FinishedOn = target.FinishedOn;
      model.CreatedOn = target.CreatedOn;
      model.ModifiedOn = target.ModifiedOn;

      return model;
    }
  }
}
