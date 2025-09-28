import { test, expect } from "@playwright/test";
import { ApiClient } from "../api-clients/api-client";

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
    const token = await apiClient.fetchToken();
    expect(token).toBeDefined();
    expect(typeof token).toBe("string");
    expect(token).not.toBe("");
    expect(token).not.toBeNull();
    expect(token.split(".")).toHaveLength(3); // Make sure it's a valid JWT
  });
});
