import { Credentials } from "./credentials";

export const MaxRefreshAttempts = 5;
export const KeyToken = "token";
export const KeyTokenExpiration = "token-expiration";
export const KeyRefreshToken = "refresh-token";
export const KeyUserId = "user-id";
export const EmptyToken = "";
export const BaseUrl = "https://localhost:44395";
export const StandardUser1 = new Credentials(
  "Default-test-User",
  "emmC2YNvh%9LtNMHWo#T"
);
export const StandardUser2 = new Credentials(
  "Other-test-user",
  "6Uh@16n%jLZKOXZO"
);
export const AdminUser1 = new Credentials(
  "Admin-test-user",
  "tmyX1zySyOSTeLqhLKD2"
);
