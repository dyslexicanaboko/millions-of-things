import { ApiClient } from "./api-client";

export class UserClient extends ApiClient {
  private readonly Endpoint: string = "api/v1/users";

  constructor() {
    super();
  }

  async read(userId: number) {
    return this.get(`${this.Endpoint}/${userId}`);
  }

  async readAll() {
    return this.get(this.Endpoint);
  }
}
