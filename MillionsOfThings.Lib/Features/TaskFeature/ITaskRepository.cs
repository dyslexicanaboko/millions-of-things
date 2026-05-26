using MillionsOfThings.Lib.Utility;

namespace MillionsOfThings.Lib.Features.TaskFeature;

public interface ITaskRepository : IRepository
{
  Task<List<TaskRecord>> ReadAll(int userId);

  Task<TaskRecord?> Read(int taskId, int userId);

  Task<List<TaskRecord>> ReadAll();

  Task<int> Create(TaskRecord entity);
  Task Update(TaskRecord entity);

  Task UpdatePartial(int userId, int taskId, List<UpdateInstruction> instructions);

  Task Delete(int userId, int taskId);
}
