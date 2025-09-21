import PgClient from "./pg-client";

export class CategoryRepository extends PgClient {
  constructor() {
    super();
  }

  public async deleteCategory(name: string, userId: number) {
    const query =
      "DELETE FROM public.category WHERE user_id = $1 AND name = $2";
    const params = [userId, name];
    await this.executeQuery(query, params);
  }
}
