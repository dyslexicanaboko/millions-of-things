import { ApiClient } from "./api-client";

export class TaskClient extends ApiClient {
  private readonly Endpoint: string = "api/v1/tasks";

  constructor() {
    super();
  }

  async add(description: string, categoryId?: number) {
    return this.post(this.Endpoint, {
      description: description,
      categoryId: categoryId,
    });
  }
}
