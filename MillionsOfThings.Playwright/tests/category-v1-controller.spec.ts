import { test, expect } from "@playwright/test";
import { CategoryV1Client } from "../api-clients/category-v1-client";
import {
  DefaultUserId1,
  OtherUserId2,
  SomeCategory,
} from "./common-test-values";
import { CategoryRepository } from "../repositories/category-repository";

test.describe("CategoryV1Controller", () => {
  test.afterAll(async () => {
    const repo = new CategoryRepository();
    await repo.delete(SomeCategory, DefaultUserId1);
    await repo.delete(SomeCategory, OtherUserId2);
    console.log("Categories deleted");
  });

  test("User 1 cannot create a duplicate category", async () => {
    const client = new CategoryV1Client();

    // 1. User 1 creates Category A
    const createRes1 = await client.add(SomeCategory);

    expect(createRes1.status()).toBe(201);

    // 2. User 1 attempts to create Category A again
    const createRes2 = await client.add(SomeCategory);

    // 3. Should get 400 with error code 40014
    expect(createRes2.status()).toBe(400);
    const error = await createRes2.json();
    expect(error.Code).toBe(40014);

    await client.dispose();
  });

  test("User 2 cannot update user 1's category", async () => {
    const client1 = new CategoryV1Client();
    const client2 = new CategoryV1Client();
    client2.changeToOtherUser();

    // 1. User 2 creates Category A
    const response2 = await client2.add(SomeCategory);
    const cat2 = await response2.json();

    // 2. User 1 attempts to modify User 2's category
    const response1 = await client1.edit(
      cat2.categoryId,
      "This should not work"
    );

    // 3. Should get 400 with error code 40014
    expect(response1.status()).toBe(404);

    await client1.dispose();
    await client2.dispose();
  });
});
