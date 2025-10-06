import { Credentials } from "./credentials";

export const MaxRefreshAttempts = 5;
export const KeyToken = "token";
export const KeyTokenExpiration = "token-expiration";
export const KeyRefreshToken = "refresh-token";
export const KeyUserId = "user-id";
export const EmptyToken = "";
export const BaseUrl = "https://localhost:44395";
export const DefaultUser1 = new Credentials(
  "Default-test-User",
  "emmC2YNvh%9LtNMHWo#T"
);
export const OtherUser2 = new Credentials(
  "Other-test-user",
  "6Uh@16n%jLZKOXZO"
);
