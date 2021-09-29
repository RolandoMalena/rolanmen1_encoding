using Newtonsoft.Json;
using System.Collections.Generic;

namespace WorkflowRunner.Models
{
  public class ListWorkflowResponse
    {
        [JsonProperty("total_count")]
        public int total_count { get; set; }

        [JsonProperty("workflow_runs")]
        public IList<WorkflowRun> workflow_runs { get; set; }
    }
}
