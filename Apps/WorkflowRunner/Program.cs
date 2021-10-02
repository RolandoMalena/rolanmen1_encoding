using HttpClientFactoryLite;
using CommandLine;
using System.Threading.Tasks;
using System;

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
            Console.WriteLine("Starting Workflow Runner app.");
            Console.WriteLine($"Token: {opts.Token}");
            Console.WriteLine($"Ref: {opts.Ref}");
            Console.WriteLine($"Workflow: {opts.Workflow}");

            var service = new WorkflowRunnerService(new HttpClientFactory(), opts.Token, opts.Ref, opts.Workflow);
            await service.RunWorkflowAsync();

            return 0;
        }
    }
}
