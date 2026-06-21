import { test, expect } from "@playwright/test";
import { UserV1Client } from "../api-clients/user-v1-client";
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
    const clientUser1 = new UserV1Client();

    // User 1 attempts to read all users.
    const getResponse = await clientUser1.readAll();
    expect(getResponse.status()).toBe(403);

    await clientUser1.dispose();
  });

  test("Standard user attempting to create users should return 403.", async () => {
    const clientUser1 = new UserV1Client();

    // Standard User 1 attempts to create a user.
    const response = await clientUser1.add({
      username: "testuser",
      firstName: "Test",
      lastName: "User",
      emailAddress: "testuser@example.com",
      role: "standard"
    });
    expect(response.status()).toBe(403);

    await clientUser1.dispose();
  });

  /* 2026-06-20 tests to do
   * Creating a user as an Admin should succeed and return the created user's Id.
   * Follow up with a get to verify all inputted data is correct.
   * 
   * After a user is created, it is not enabled. Test for this.
   * Either check the user's record or attempt to log in as the user and expect failure.
   * However, the user does not have a password at this point either.
   * 
   * Usernames are unique, so we can attempt to create the same user twice and expect the second attempt to fail with a 400.
   * Emails are unique, so we can attempt to create the same user twice and expect the second attempt to fail with a 400.
   * 
   */
});
