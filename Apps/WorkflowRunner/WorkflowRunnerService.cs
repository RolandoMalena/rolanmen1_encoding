using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WorkflowRunner.ExtensionMethods;
using WorkflowRunner.Models;

namespace WorkflowRunner
{
    public interface IWorkflowRunnerService
    {
        Task RunWorkflowAsync(CancellationToken cancellationToken = default);
    }

    public class WorkflowRunnerService : IWorkflowRunnerService
    {
        private readonly HttpClientFactoryLite.IHttpClientFactory _httpClientFactory;
        private readonly string _token;
        private readonly string _ref;
        private readonly long _workflow;

        private const string ListBaseUrl = "https://api.github.com/repos/RolandoMalena/rolanmen1_encoding/actions/workflows/{0}/runs";
        private const int ListPageSize = 100;

        private const string GetBaseUrl = "https://api.github.com/repos/RolandoMalena/rolanmen1_encoding/actions/runs/{0}";
        private const string PostBaseUrl = "https://api.github.com/repos/RolandoMalena/rolanmen1_encoding/actions/workflows/{0}/dispatches";

        public WorkflowRunnerService(HttpClientFactoryLite.IHttpClientFactory httpClientFactory, string token, string @ref, long workflow)
        {
            _httpClientFactory = httpClientFactory;
            _token = token;
            _ref = @ref;
            _workflow = workflow;
        }

        public async Task RunWorkflowAsync(CancellationToken cancellationToken = default)
        {
            long runId;
            int retryCount = 0;
            int maxRetryCount = 20;

            var oldList = await GetWorkflowList(cancellationToken);
            await CreateWorkflowRun(cancellationToken);

            while (true)
            {
                WaitSeconds(5);
                var newList = await GetWorkflowList(cancellationToken);

                if (TryFindNewRun(oldList, newList, out runId))
                    break;

                oldList = newList;

                retryCount++;
                if (retryCount >= maxRetryCount)
                    throw new ApplicationException($"Could not retrieve Workflow run after {maxRetryCount} attempts.");
            }

            while (true)
            {
                WaitSeconds(10);
                var run = await GetWorkflowRun(runId, cancellationToken);

                if (run.status != Constants.WorkflowRunStatus.InProgress)
                {
                    Console.WriteLine($"Workflow run is no longer running. Conclusion = {run.conclusion}");

                    if (run.conclusion != Constants.WorkflowRunConclusion.Success)
                        throw new ApplicationException($"Workflow run completed with Conclusion: {run.conclusion}");

                    break;
                }
            }
        }

        #region Private Helpers
        private void WaitSeconds(int seconds)
        {
            Console.WriteLine($"Waiting {seconds} seconds.");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }

        private async Task<IList<WorkflowRun>> GetWorkflowList(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Retrieving list of Runs.");
            var runs = new List<WorkflowRun>();
            int currentPage = 0;

            var baseUrl = string.Format(ListBaseUrl, _workflow);
            var parameters =
                $"branch={_ref.Split('/').Last()}" +
                "&event=workflow_dispatch" +
                $"&status={Constants.WorkflowRunStatus.InProgress}" +
                $"&per_page={ListPageSize}" +
                "&page=";

            while (true)
            {
                currentPage++;
                string url = $"{baseUrl}?{parameters}{currentPage}";

                var responseBody = await _httpClientFactory.MakeRequest<ListWorkflowResponse>(HttpMethod.Get, url, _token, cancellationToken);
                
                if (responseBody.workflow_runs != null)
                    runs.AddRange(responseBody.workflow_runs);

                if (responseBody.total_count < ListPageSize)
                    break;
            }

            Console.WriteLine($"Retrieved {runs.Count} runs.");
            return runs;
        }

        private async Task CreateWorkflowRun(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Creating Workflow run.");

            var url = string.Format(PostBaseUrl, _workflow);
            var body = new PostWorkflowRequest()
            {
                @ref = _ref
            };

            await _httpClientFactory.MakeRequest(HttpMethod.Post, url, _token, body, cancellationToken);

            Console.WriteLine("Workflow run created successfully.");
        }

        private bool TryFindNewRun(IList<WorkflowRun> oldList, IList<WorkflowRun> newList, out long runId)
        {
            Console.WriteLine("Comparing List of Runs.");
            runId = 0;

            var oldIds = oldList.Select(i => i.id).OrderByDescending(i => i).ToArray();
            var newIds = newList.Select(i => i.id).OrderByDescending(i => i).ToArray();

            foreach (var id in newIds)
            {
                if (!oldIds.Contains(id))
                {
                    runId = id;
                    break;
                }
            }

            if (runId == 0)
            {
                Console.WriteLine("Could not find new Workflow run.");
                return false;
            }

            Console.WriteLine($"Found new Workflow run. Id = {runId}");
            return true;
        }

        private async Task<WorkflowRun> GetWorkflowRun(long runId, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Retrieving Workflow Run.");

            var url = string.Format(GetBaseUrl, runId);
            var responseBody = await _httpClientFactory.MakeRequest<WorkflowRun>(HttpMethod.Get, url, _token, cancellationToken);

            Console.WriteLine($"Retrieved Workflow run. Status = {responseBody.status}");
            return responseBody;
        }
        #endregion
    }
}
