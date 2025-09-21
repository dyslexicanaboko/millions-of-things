import { test, expect } from "@playwright/test";
import { CategoryRepository } from "../repositories/category-repository";
import { CategoryClient } from "../api-clients/category-client";
import { SomeCategory, SomeUserId } from "./common-test-values";

// Helper to delete Category A for user 1
async function deleteCategoryA() {
  const categoryRepo = new CategoryRepository();
  await categoryRepo.delete(SomeCategory, SomeUserId);
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
});
