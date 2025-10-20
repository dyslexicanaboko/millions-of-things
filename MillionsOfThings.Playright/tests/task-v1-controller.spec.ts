import { test, expect } from "@playwright/test";
import { TaskClient } from "../api-clients/task-client";
import {
  SomeTask,
  SomeCategoryId,
  DefaultDateTime,
  DefaultUserId1,
} from "./common-test-values";
import { TaskRepository } from "../repositories/task-repository";

test.describe("TaskV1Controller", () => {
  test.afterAll(async () => {
    const repo = new TaskRepository();
    await repo.delete(SomeTask, DefaultUserId1);
    console.log("Tasks deleted");
  });

  test("Creating a task should have expected default audit values.", async () => {
    const client = new TaskClient();

    const response = await client.add(SomeTask, SomeCategoryId);

    expect(response.status()).toBe(201);

    const responseBody = await response.json();

    console.log(responseBody.taskId);

    expect(responseBody).toHaveProperty("isFinished", false);
    expect(responseBody).toHaveProperty("finishedOn", null);
    expect(responseBody).toHaveProperty("modifiedOn", null);
    expect(responseBody).toHaveProperty("createdOn");
    expect(responseBody.createdOn).not.toBe(DefaultDateTime);
    expect(responseBody).toHaveProperty("description", SomeTask);
    expect(responseBody).toHaveProperty("categoryId", SomeCategoryId);
    expect(responseBody).toHaveProperty("userId", 1);

    await client.dispose();
    console.log("Test completed");
  });

  test("Updating a task's isFinished property should populate finishedOn and modifiedOn.", async () => {
    const client = new TaskClient();

    // Create the task
    const createResponse = await client.add(SomeTask, SomeCategoryId);
    expect(createResponse.status()).toBe(201);
    const createdTask = await createResponse.json();
    const taskId = createdTask.taskId;

    console.log(taskId);

    // Perform partial patch: update isFinished to true
    const updateResponse = await client.edit(
      taskId,
      undefined,
      undefined,
      true
    );
    expect(updateResponse.status()).toBe(204);

    // Retrieve the updated task
    const getResponse = await client.read(taskId);
    const updatedTask = await getResponse.json();

    // Assert finishedOn and modifiedOn are populated (not null)
    expect(updatedTask.isFinished).toBe(true);
    expect(updatedTask.finishedOn).not.toBeNull();
    expect(updatedTask.modifiedOn).not.toBeNull();

    await client.dispose();
    console.log("Test completed");
  });

  test("User 2 attempting to edit User 1's task should return 404.", async () => {
    const clientUser1 = new TaskClient();
    const clientUser2 = new TaskClient();
    clientUser2.changeToOtherUser();

    // User 1 creates the task
    const createResponse = await clientUser1.add(SomeTask, SomeCategoryId);
    const createdTask = await createResponse.json();
    const taskId = createdTask.taskId;

    // User 2 attempts to edit User 1's task
    const editResponse = await clientUser2.edit(
      taskId,
      undefined,
      "This should not work",
      true
    );
    expect(editResponse.status()).toBe(404);

    await clientUser1.dispose();
    await clientUser2.dispose();
  });
});
