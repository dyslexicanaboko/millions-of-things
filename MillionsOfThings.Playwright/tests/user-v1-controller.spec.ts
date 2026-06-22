import { test, expect } from "@playwright/test";
import { UserV1Client } from "../api-clients/user-v1-client";
import {
  SomeTask,
  SomeCategoryId,
  DefaultDateTime,
  StandardUserId1,
} from "./common-test-values";
import { UserRepository } from "../repositories/user-repository";

//Unit Test User can be created, updated, and ultimately deleted.
const unitTestUser = {
  username: "unittestuser",
  firstName: "Unit",
  lastName: "Test",
  emailAddress: "unittest@user.com",
  role: "standard"
};

const getExistingTestUser = () => {
  return {
    username: "Default-test-User",
    firstName: "does not matter",
    lastName: "does not matter",
    emailAddress: "Default@testuser.com",
    role: "standard"
  };
}

const userRepo = new UserRepository();

test.describe("UserV1Controller", () => {
  test.afterAll(async () => {
    //console.log("Tasks deleted");
    //await userRepo.delete(unitTestUser.username);
  });

  /*
    For additional tests go look at Obsidian
  */

  test("Standard user attempting to access get-all-users endpoint should return 403.", async () => {
    const client = new UserV1Client();

    // Standard user attempts to read all users.
    const getResponse = await client.readAll();
    expect(getResponse.status()).toBe(403);

    await client.dispose();
  });

  test("Standard user attempting to create users should return 403.", async () => {
    const client = new UserV1Client();

    // Standard user attempts to create a user.
    const response = await client.add(unitTestUser);

    expect(response.status()).toBe(403);

    await client.dispose();
  });

  test("Administrator user creating user should return 201.", async () => {
    const client = new UserV1Client();
    //TODO: 2026-06-21 change the client to Admin for the next three tests to pass.
    // Should there just be a method that starts the ApiClient with the Administrator?

    // Administrator user creates a user.
    const response = await client.add(unitTestUser);
    expect(response.status()).toBe(201);

    const createdUser = await response.json();

    userRepo.read(createdUser.userId).then((userRecord) => {
      expect(userRecord.isAllowed).toBe(false);
      expect(userRecord.username).toBe(unitTestUser.username);
      expect(userRecord.firstName).toBe(unitTestUser.firstName);
      expect(userRecord.lastName).toBe(unitTestUser.lastName);
      expect(userRecord.emailAddress).toBe(unitTestUser.emailAddress);
    });

    await client.dispose();
  });

  test("Administrator user creating user with existing username should return 400.", async () => {
    const client = new UserV1Client();

    var user = getExistingTestUser();
    //Testing duplicated username.
    user.emailAddress = "123456789@user.com"; // Assuming this doesn't exist

    const response = await client.add(user);
    
    expect(response.status()).toBe(400);

    await client.dispose();
  });

  test("Administrator user creating user with existing email should return 400.", async () => {
    const client = new UserV1Client();

    var user = getExistingTestUser();
    //Testing duplicated email.
    user.username = "unique-username"; // Assuming this doesn't exist

    const response = await client.add(user);
    
    expect(response.status()).toBe(400);

    await client.dispose();
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
