import { test, expect } from "@playwright/test";
import { ApiClient } from "../api-clients/api-client";
import { DefaultUser1 } from "../constants";
import jwt from "jsonwebtoken";

test.describe("ApiClient", () => {
  let apiClient: ApiClient;

  test.beforeEach(() => {
    apiClient = new ApiClient();
  });

  test.afterEach(async () => {
    await apiClient.dispose();
  });

  test("should fetch a non-empty token", async () => {
    // @ts-expect-error: Accessing private method for testing
    const token = await apiClient.fetchToken(DefaultUser1);
    expect(token).toBeDefined();
    expect(typeof token).toBe("string");
    expect(token).not.toBe("");
    expect(token).not.toBeNull();
    expect(token.split(".")).toHaveLength(3); // Make sure it's a valid JWT

    //Finally output the token for debugging purposes
    console.log("\r\nFetched token:", token);

    //Decode the JWT to visually inspect its contents for debugging purposes
    console.log("\r\nDecoded JWT payload:", jwt.decode(token));
  });
});
