import { test, expect, request } from "@playwright/test";
import { TaskClient } from "../api-clients/task-client";
import { TaskRepository } from "../repositories/task-repository";
import {
  SomeTask,
  SomeCategoryId,
  SomeUserId,
  DefaultDateTime,
} from "./common-test-values";

test.describe("TaskV1Controller", () => {
  test.afterAll(async () => {
    const repo = new TaskRepository();
    await repo.delete(SomeTask, SomeUserId);
  });

  test("Creating a task should have expected default audit values.", async () => {
    const client = new TaskClient();

    const response = await client.add(SomeTask, SomeCategoryId);

    expect(response.status()).toBe(201);

    const responseBody = await response.json();

    expect(responseBody).toHaveProperty("isFinished", false);
    expect(responseBody).toHaveProperty("finishedOn", null);
    expect(responseBody).toHaveProperty("modifiedOn", null);
    expect(responseBody).toHaveProperty("createdOn");
    expect(responseBody.createdOn).not.toBe(DefaultDateTime);
    expect(responseBody).toHaveProperty("description", SomeTask);
    expect(responseBody).toHaveProperty("categoryId", SomeCategoryId);
    expect(responseBody).toHaveProperty("userId", 1);

    await client.dispose();
  });
});
