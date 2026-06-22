import PostgresSqlClient from "./postgres-sql-client";
import { UserRecord } from "./records/UserRecord";

export class UserRepository extends PostgresSqlClient {
  constructor() {
    super();
  }

  public async delete(username: string) {
    const query =
      "DELETE FROM public.user WHERE username = $1::citext";
    const params = [username];
    await this.executeNonQuery(query, params);
  }

  public async read(userId: number): Promise<UserRecord> {
    const query =
      `SELECT
         user_id
        ,is_allowed
        ,username
        ,firstname
        ,lastname
        ,emailaddress
        ,created_on
      FROM public.user WHERE user_id = $1`;
    const params = [userId];
    const result = await this.executeQuery<UserRecord>(query, params);
    
    return result[0];
  }
}
