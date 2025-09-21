import PostgresSqlClient from "./postgres-sql-client";

export class TaskRepository extends PostgresSqlClient {
  constructor() {
    super();
  }

  public async delete(description: string, userId: number) {
    const query =
      "DELETE FROM public.task WHERE user_id = $1 AND description = $2";
    const params = [userId, description];
    await this.executeQuery(query, params);
  }
}
