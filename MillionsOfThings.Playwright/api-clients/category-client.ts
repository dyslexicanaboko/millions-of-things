import { ApiClient } from "./api-client";
import { PatchDoc } from "./patch-doc";

export class CategoryClient extends ApiClient {
  private readonly Endpoint: string = "millionsofthings/v1/categories";

  constructor() {
    super();
  }

  async read(categoryId: number) {
    return this.get(`${this.Endpoint}/${categoryId}`);
  }

  async add(name: string) {
    return this.post(this.Endpoint, { name });
  }

  async edit(categoryId: number, name: string) {
    const operations: PatchDoc[] = [];

    if (name !== undefined) {
      operations.push(new PatchDoc("name", name));
    }

    return this.patch(`${this.Endpoint}/${categoryId}`, operations);
  }
}
