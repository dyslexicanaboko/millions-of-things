import { ApiClient } from "./api-client";
import { PatchDoc } from "./patch-doc";

export class TaskClient extends ApiClient {
  private readonly Endpoint: string = "api/v1/tasks";

  constructor() {
    super();
  }

  async read(taskId: number) {
    return this.get(`${this.Endpoint}/${taskId}`);
  }

  async add(description: string, categoryId?: number) {
    return this.post(this.Endpoint, {
      description: description,
      categoryId: categoryId,
    });
  }

  async update(
    taskId: number,
    categoryId?: number,
    description?: string,
    isFinished?: boolean
  ) {
    const operations: PatchDoc[] = [];

    if (description !== undefined) {
      operations.push(new PatchDoc("description", description));
    }

    if (categoryId !== undefined) {
      operations.push(new PatchDoc("categoryId", categoryId));
    }

    if (isFinished !== undefined) {
      operations.push(new PatchDoc("isFinished", isFinished));
    }

    return this.patch(this.Endpoint + `/${taskId}`, operations);
  }

  async remove(taskId: number) {
    return this.delete(this.Endpoint + `/${taskId}`);
  }
}
