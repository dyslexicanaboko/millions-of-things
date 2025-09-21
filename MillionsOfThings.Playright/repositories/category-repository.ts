import PostgresSqlClient from "./postgres-sql-client";

export class CategoryRepository extends PostgresSqlClient {
  constructor() {
    super();
  }

  public async delete(name: string, userId: number) {
    const query =
      "DELETE FROM public.category WHERE user_id = $1 AND name = $2";
    const params = [userId, name];
    await this.executeQuery(query, params);
  }
}
