namespace WorkflowRunner
{
    public class Constants
    {
        public class WorkflowRunStatus
        {
            public const string Queued = "queued";
            public const string InProgress = "in_progress";
            public const string Completed = "completed";
        }

        public class WorkflowRunConclusion
        {
            public const string Failure = "failure";
            public const string Success = "success";
        }
    }
}
