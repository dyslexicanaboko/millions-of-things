import { test, expect } from "@playwright/test";
import { UserClient } from "../api-clients/user-client";
import {
  SomeTask,
  SomeCategoryId,
  DefaultDateTime,
  DefaultUserId1,
} from "./common-test-values";
import { TaskRepository } from "../repositories/task-repository";

test.describe("UserV1Controller", () => {
  test.afterAll(async () => {
    //console.log("Tasks deleted");
  });

  /*
    For additional tests go look at Obsidian
  */

  test("Standard user attempting to access get-all-users endpoint should return 403.", async () => {
    const clientUser1 = new UserClient();

    // User 1 attempts to read all users.
    const getResponse = await clientUser1.readAll();
    expect(getResponse.status()).toBe(403);

    await clientUser1.dispose();
  });

  test("Standard user attempting to create users endpoint should return 403.", async () => {
    const clientUser1 = new UserClient();

    // User 1 attempts to create a user.
    const getResponse = await clientUser1.readAll();
    expect(getResponse.status()).toBe(403);

    await clientUser1.dispose();
  });
});
