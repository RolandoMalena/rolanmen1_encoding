using Newtonsoft.Json;
using System.Collections.Generic;

namespace WorkflowRunner.Models
{
  public class ListWorkflowResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("workflow_runs")]
        public IList<WorkflowRun> WorkflowRuns { get; set; }
    }
}
