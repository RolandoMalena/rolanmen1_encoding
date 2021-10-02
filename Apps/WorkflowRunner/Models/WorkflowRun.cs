namespace WorkflowRunner.Models
{
    public class WorkflowRun
    {
        public long id { get; set; }
        public string status { get; set; }
        public string conclusion { get; set; }
        public string html_url { get; set; }
    }
}