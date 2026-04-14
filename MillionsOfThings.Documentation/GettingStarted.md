# How to get started with Redocly

All operations described happen in the same folder where the `OAS-export.json` is located.

- If not installed already [install the redocly CLI](https://redocly.com/docs/cli/installation#install-locally) using `npm i @redocly/cli@latest`
- In the same directory where the OAS file is run `redocly preview` and a preview engine will run. Wait for it to load.
- After it loads use the URL it provides that will look like this <http://127.0.0.1:4000>.

## Preview

- Navigate to the aforementioned URL and this is the live preview of what redocly is interpreting from your documentation.
- Either add external Markdown files to `description` properties as `"description": { "$ref": "./docs/info.md" }` for example, or perform in-line descriptions as regular text, or alternatively in-line markdown.

## Build docs

- Once you are satisfied with your documentation, then you can build your docs as [described here](https://redocly.com/docs/cli/commands/build-docs).
- Example: `redocly build-docs <api> --output=custom.html`

