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

        [Option('1', "input1", Required = false, HelpText = "Input #1 to send to Workflow.")]
        public string Input1 { get; set; }

        [Option('2', "input2", Required = false, HelpText = "Input #2 to send to Workflow.")]
        public string Input2 { get; set; }

        [Option('3', "input3", Required = false, HelpText = "Input #3 to send to Workflow.")]
        public string Input3 { get; set; }

        [Option('4', "input4", Required = false, HelpText = "Input #4 to send to Workflow.")]
        public string Input4 { get; set; }

        [Option('5', "input5", Required = false, HelpText = "Input #5 to send to Workflow.")]
        public string Input5 { get; set; }
    }
}