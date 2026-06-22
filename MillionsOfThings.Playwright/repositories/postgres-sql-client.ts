import { Client } from "pg";

// Export a class
export default class PostgresSqlClient {
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

  public async executeNonQuery(query: string, params: any[]) {
    const client = await this.getClient();
    await client.query(query, params);
    await client.end();
  }

  public async executeQuery<T>(query: string, params: any[]): Promise<T[]> {
    const client = await this.getClient();
    const result = await client.query(query, params);
    await client.end();
    return result.rows as unknown as T[];
  }
}
