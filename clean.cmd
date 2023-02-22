@echo off

call dotnet build && cd GithubWorkflowCleaner && dotnet run %1 %2 %3