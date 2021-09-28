using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
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

        public WorkflowRunnerService(HttpClientFactoryLite.IHttpClientFactory httpClientFactory, string token, string @ref, long workflow)
        {
            _httpClientFactory = httpClientFactory;
            _token = token;
            _ref = @ref;
            _workflow = workflow;
        }

        public async Task RunWorkflowAsync(CancellationToken cancellationToken = default)
        {
            var list = await GetWorkflowList(cancellationToken);
        }
    
        #region Private Helpers
        private async Task<IList<WorkflowRun>> GetWorkflowList(CancellationToken cancellationToken = default)
        {
            var runs = new List<WorkflowRun>();
            int currentPage = 0;

            var baseUrl = string.Format(ListBaseUrl, _workflow);
            var parameters =
                $"branch={_ref}" +
                "&event=workflow_dispatch" +
                "&status=in_progress" +
                $"&per_page={ListPageSize}" +
                "&page=";

            while (true)
            {
                currentPage++;
                var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}?{parameters}{currentPage}");
                request.Headers.Add("Authorization", $"Bearer {_token}");

                var client = _httpClientFactory.CreateClient("List");
                var response = await client.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                  throw new HttpResponseException(response);

                var body = await response.Content.ReadAsStringAsync();
                var deserializedBody = JsonSerializer.Deserialize<ListWorkflowResponse>(body);

                runs.AddRange(deserializedBody.WorkflowRuns);
                if (deserializedBody.TotalCount < ListPageSize)
                    break;
            }

            return runs;
        }
        #endregion
    }
}
