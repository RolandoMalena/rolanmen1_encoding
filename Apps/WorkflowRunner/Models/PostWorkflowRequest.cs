using System.Collections.Generic;

namespace WorkflowRunner.Models
{
    public class PostWorkflowRequest
    {
        public string @ref { get; set; }
        public IDictionary<string, string> inputs { get; set; }
    }
}
