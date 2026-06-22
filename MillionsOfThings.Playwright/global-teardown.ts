import { CategoryRepository } from "./repositories/category-repository";
import { TaskRepository } from "./repositories/task-repository";
import {
  SomeTask,
  StandardUserId1,
  StandardUserId2,
  SomeCategory,
} from "./tests/common-test-values";

async function globalTeardown() {
  console.log("Running global teardown...");

  //This only works well if you are not using the UI. These will only run
  //after you close the UI which is inconvenient.
  //await taskV1ControllerTeardown();
  //await categoryV1ControllerTeardown();

  console.log("Global cleanup completed - Tasks deleted");
}

async function taskV1ControllerTeardown() {
  const repo = new TaskRepository();
  await repo.delete(SomeTask, StandardUserId1);
  console.log("Tasks deleted");
}

async function categoryV1ControllerTeardown() {
  const repo = new CategoryRepository();
  await repo.delete(SomeCategory, StandardUserId1);
  await repo.delete(SomeCategory, StandardUserId2);
  console.log("Categories deleted");
}

export default globalTeardown;
