import { test, expect } from "@playwright/test";
import { CategoryRepository } from "../repositories/category-repository";
import { CategoryClient } from "../api-clients/category-client";
import {
  SomeCategory,
  DefaultUserId1,
  OtherUserId2,
} from "./common-test-values";

// Helper to delete Category A for user 1
async function deleteCategoryA() {
  const categoryRepo = new CategoryRepository();
  await categoryRepo.delete(SomeCategory, DefaultUserId1);
  await categoryRepo.delete(SomeCategory, OtherUserId2);
}

test.describe("CategoryV1Controller", () => {
  test.beforeAll(async () => {
    await deleteCategoryA();
  });

  test.afterAll(async () => {
    await deleteCategoryA();
  });

  test("User 1 cannot create a duplicate category", async () => {
    const client = new CategoryClient();

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
    const client1 = new CategoryClient();
    const client2 = new CategoryClient();
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
