using HttpClientFactoryLite;
using CommandLine;
using System.Threading.Tasks;

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
            var service = new WorkflowRunnerService(new HttpClientFactory(), opts.Token, opts.Ref, opts.Workflow);
            await service.RunWorkflowAsync();

            return 0;
        }
    }
}
