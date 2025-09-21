import { test, expect } from "@playwright/test";
import { CategoryRepository } from "../repositories/category-repository";
import { CategoryClient } from "../api-clients/category-client";

const CategoryA = "Category A";
const User1Id = 1;

// Helper to delete Category A for user 1
async function deleteCategoryA() {
  const categoryRepo = new CategoryRepository();
  await categoryRepo.deleteCategory(CategoryA, User1Id);
}

test.describe("Category duplicate creation", () => {
  test.beforeAll(async () => {
    await deleteCategoryA();
  });

  test.afterAll(async () => {
    await deleteCategoryA();
  });

  test("User 1 cannot create duplicate category", async () => {
    const client = new CategoryClient();

    // 1. User 1 creates Category A
    const createRes1 = await client.add(CategoryA);

    expect(createRes1.status()).toBe(201);

    // 2. User 1 attempts to create Category A again
    const createRes2 = await client.add(CategoryA);

    // 3. Should get 400 with error code 40014
    expect(createRes2.status()).toBe(400);
    const error = await createRes2.json();
    expect(error.Code).toBe(40014);

    await client.dispose();
  });
});
