import { ApiClient } from "./api-client";

//TODO: Add the version number to the class name
export class UserClient extends ApiClient {
  private readonly Endpoint: string = "millionsofthings/v1/users";

  constructor() {
    super();
  }

  async read(userId: number) {
    return this.get(`${this.Endpoint}/${userId}`);
  }

  async readAll() {
    return this.get(this.Endpoint);
  }

  async add(name: string) {
    return this.post(this.Endpoint, { name });
  }
}
