import { ApiClient } from "./api-client";
import { UserV1CreateModel } from "./models/UserV1CreateModel";

export class UserV1Client extends ApiClient {
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

  async add(user: UserV1CreateModel) {
    return this.post(this.Endpoint, user);
  }
}
