using CommandLine;

namespace WorkflowRunner
{
    public class Options
    {
        [Option('t', "token", Required = true, HelpText = "Github Personal Access Token (PAT).")]
        public string Token { get; set; }

        [Option('r', "ref", Required = true, HelpText = "Github Ref or Branch Name.")]
        public string Ref { get; set; }

        [Option('w', "workflow", Required = true, HelpText = "ID of the Github Workflow to run.")]
        public long Workflow { get; set; }

        [Option('i', "inputs", Required = false, HelpText = "Inputs data to send to Workflow.")]
        public string Inputs { get; set; }
    }
}