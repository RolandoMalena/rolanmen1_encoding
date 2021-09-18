using System;
using CommandLine;

namespace WorkflowRunner
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (Options opts) => Run(opts),
                    errors => -1);
        }

        static int Run(Options opts)
        {
            Console.WriteLine(opts.Token);
            Console.WriteLine(opts.Ref);

            return 0;
        }
    }
}
