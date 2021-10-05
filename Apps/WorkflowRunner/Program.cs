using HttpClientFactoryLite;
using CommandLine;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace WorkflowRunner
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (Options opts) => Run(opts).Result,
                    errors => -1);
        }

        static async Task<int> Run(Options opts)
        {
            var inputs = new string[]
            {
                opts.Input1, opts.Input2, opts.Input3, opts.Input4, opts.Input5
            };
            var inputString = string.Join(", ", inputs.Select(i => i ?? "<none>"));

            Console.WriteLine("Starting Workflow Runner app.");
            Console.WriteLine($"Token: {opts.Token}");
            Console.WriteLine($"Ref: {opts.Ref}");
            Console.WriteLine($"Workflow: {opts.Workflow}");
            Console.WriteLine($"Inputs: {inputString}");

            var service = new WorkflowRunnerService(new HttpClientFactory(), opts.Token, opts.Ref, opts.Workflow, inputs);
            await service.RunWorkflowAsync();

            return 0;
        }
    }
}
