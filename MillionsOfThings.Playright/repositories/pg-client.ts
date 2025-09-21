import { Client } from "pg";

// Export a class
export default class PgClient {
  public constructor() {}

  private async getClient() {
    const client = new Client({
      host: "localhost",
      port: 5432,
      user: "postgres",
      password: "postgres",
      database: "millions_of_things",
    });

    await client.connect();

    return client;
  }

  public async executeQuery(query: string, params: any[]) {
    const client = await this.getClient();
    await client.query(query, params);
    await client.end();
  }
}
