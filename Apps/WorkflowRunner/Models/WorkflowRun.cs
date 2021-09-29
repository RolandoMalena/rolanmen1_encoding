namespace WorkflowRunner.Models
{
    public class WorkflowRun
    {
        public long id { get; set; }
        public string status { get; set; }
        public string conclusion { get; set; }
    }
}