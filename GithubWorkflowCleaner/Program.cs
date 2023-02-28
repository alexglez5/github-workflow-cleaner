using System.Diagnostics;
using GithubWorkflowCleaner.Extensions;

namespace GithubWorkflowCleaner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start Github workflows cleanup.");

            try
            {
                var defaultMinRunsToKeep = 1;
                var minRunsToKeep = args.Length <= 2 ? defaultMinRunsToKeep : int.Parse(args[2]);
                CleanUpWorkflows(org: args[0], repo: args[1], minRunsToKeep);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CleanUpWorkflows(string org, string repo, int minRunsToKeep)
        {
            Console.WriteLine($"Cleaning up workflows at {org}/{repo}.");
            
            var actions = $"{org}/{repo}/actions";
            var workflowIdsString = Run($"gh api -X GET /repos/{actions}/workflows | jq \".workflows[] | .id\"");

            var workflowIds = workflowIdsString.SplitByNewLine();

            foreach (var workflowId in workflowIds)
            {
                var workflowRunIdsString = Run($"gh api -X GET /repos/{actions}/workflows/{workflowId}/runs | jq \".workflow_runs[] | .id\"");

                var workflowRunIds = workflowRunIdsString
                    .SplitByNewLine()
                    .Skip(minRunsToKeep)
                    .ToList();

                var entriesToSkip = Math.Min(workflowRunIds.Count, minRunsToKeep);
                
                Console.WriteLine($"Keeping {entriesToSkip} out of the {workflowRunIds.Count} available workflow runs.");

                var workflowRunIdsToDelete = workflowRunIds
                    .Skip(entriesToSkip)
                    .ToList();

                foreach (var workflowRunId in workflowRunIdsToDelete)
                {
                    Console.WriteLine($"Removing workflow run with id {workflowRunId}.");
                    
                    Run($"gh api -X DELETE /repos/{actions}/runs/{workflowRunId}");
                }
            }

            Console.WriteLine("Workflows cleanup completed successfully.");
        }

        private static string Run(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", $"/c {command}")
            {
                RedirectStandardOutput = true
            };

            var process = Process.Start(processInfo) ?? new Process();
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            process.Close();

            return output;
        }
    }
}