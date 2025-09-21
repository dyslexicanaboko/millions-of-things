import { ApiClient } from "./api-client";

export class CategoryClient extends ApiClient {
  private readonly Endpoint: string = "api/v1/category";

  constructor() {
    super();
  }

  async add(name: string) {
    return this.post(this.Endpoint, { name });
  }
}
