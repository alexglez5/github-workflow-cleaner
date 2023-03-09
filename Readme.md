# Github Workflow Cleaner

Allows you to delete workflows from Github actions from Windows.

## How to use?

- download repo
- ensure `gh` and `jq` CLI tools are installed
  - `choco install gh`
  - `choco install jq`
  - `refreshenv`
  - `gh auth login`
    - follow instructions
- navigate to folder from command prompt
- run `clean "<org>" "<repo>" <optional-minRunsToKeepPerWorkflow>`
  - for example: `clean jothsmith123 mywebapp 2` (for every workflow delete all runs except last 2)
