# Getting started

Installing Playwright locally and running tests.

## Consider using NVM

Recommended to install `nvm` if you haven't already.

- If you already have `node` installed, you will have to uninstall it before installing `nvm`.
- Windows version: <https://github.com/coreybutler/nvm-windows/releases> - just download the `nvm-setup.exe` at the bottom where it says "Assets". These instructions were written on 2025-11-29, therefore at this time this version of NVM is being succeeded by something called "Author" and it's related to "Runtime (rt)". This version is stale and will be replaced in the future.
- Linux version: <https://github.com/nvm-sh/nvm>
    - This is the original version. The above Windows version is based on this version.

## Node verion

- The required Node Version is in the `package-lock.json`.
- If you are using `nvm` you should switch to the version shown in the `package-lock.json` or greater.

### Using this repository's version

For example if the current version is (subject to change) `24.5.2` then you would do the following:

```sh
nvm install 24.5.2
nvm use 24.5.2
node -v #If you want to verify
```

### Using the latest version

If you want to use the most recent stable version then you would do the following:

```sh
nvm install latest
nvm list #Shows versions you have
nvm use M.m.p #Enter the version you want to use such as 25.2.1
node -v #If you want to verify
```

## Installation

These instructions assume you have the correct node version installed and set/selected if using `nvm`.

```sh
npm i #installing the NPM dependencies
npx playwright install #installing playwright browsers
```

## Running the tests

To just get started do the following:

1. Make sure Postgres is up & running and that the `millions_of_things` database has been setup. Those [instructions are located here](../MillionsOfThings.Database/ReadMe.md).
2. Make sure the API is running.
   1. In `VisualStudio` run the project as `IIS Express`.
   2. If this is your first time running it, allow the SSL certificates to be created and installed.
   3. Notate the port being used once Swagger pops up.
   4. Make sure that the `constants.ts::BaseUrl` variable is using that port. Alternatively make the API change its port instead (preferable).
3. Run `npx playwright test` to run all tests.
   1. If you get a bunch of failures, check your port.
   2. If you still get failures, make sure Postgres is running, and the `millions_of_things` database exists with its test data.

If you want to run the tests via UI or only run a specific tests go to the next section.

### Different ways to run the tests

These are different commands that can be used to run the tests.

```sh
npx playwright test
    #Runs the end-to-end tests.

npx playwright test --ui
	#Starts the interactive UI mode.

npx playwright test --project=chromium
	#Runs the tests only on Desktop Chrome.

npx playwright test example.spec.ts
	#Runs the tests in a specific file.
	#Example: npx playwright test api-client.spec.ts
    #Every test that is run will open a results window. You have to close the window and re-run the command. Not idea for repeated re-runs.

npx playwright test --debug
	#Runs the tests in debug mode.
```

This is for creating tests apparently, I have not used it yet.

```sh
npx playwright codegen
	#Auto generate tests with Codegen.
```
