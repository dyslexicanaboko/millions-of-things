// category-duplicate.spec.ts
import { test, expect, request } from "@playwright/test";
import { CategoryRepository } from "../repositories/category-repository";

//TODO - Move API stuff to its own folder
const API_BASE = "https://localhost:44395/api/v1/category";

// Helper to delete Category A for user 1
async function deleteCategoryA() {
  const categoryRepo = new CategoryRepository();
  await categoryRepo.deleteCategory("Category A", 1);
}

test.describe("Category duplicate creation", () => {
  test.beforeAll(async () => {
    await deleteCategoryA();
  });

  test.afterAll(async () => {
    await deleteCategoryA();
  });

  test("User 1 cannot create duplicate category", async ({ request }) => {
    // 1. User 1 creates Category A
    const createRes1 = await request.post(API_BASE, {
      data: { name: "Category A" },
      headers: {
        // Add authentication headers if needed, e.g. Authorization: 'Bearer ...'
        "x-user-id": "1", // Example: adjust to your API's auth scheme
      },
      ignoreHTTPSErrors: true, // Ignore HTTPS errors for localhost
    });
    expect(createRes1.status()).toBe(201);

    // 2. User 1 attempts to create Category A again
    const createRes2 = await request.post(API_BASE, {
      data: { name: "Category A" },
      headers: {
        "x-user-id": "1",
      },
      ignoreHTTPSErrors: true, // Ignore HTTPS errors for localhost
    });

    // 3. Should get 400 with error code 40014
    expect(createRes2.status()).toBe(400);
    const error = await createRes2.json();
    expect(error.Code).toBe(40014);
  });
});
