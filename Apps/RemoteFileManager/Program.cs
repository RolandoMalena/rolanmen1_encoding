using CommandLine;
using Google.Apis.Drive.v3;
using RemoteFileManager.Options;
using System;
using System.Threading.Tasks;

namespace RemoteFileManager
{
    class Program
    {
        const string ApplicationName = "Remote File Manager";

        static DriveService service;
        static BaseOptions options;

        static async Task<int> Main(string[] args)
        {
            Console.WriteLine($"Starting {ApplicationName}.");

            if(!ParseArguments(args))
                return -1;

            options.PrintOptions();
            Console.WriteLine();

            Console.WriteLine("Authenticating to Google Drive.");
            service = Actions.Authenticate.Execute(ApplicationName);
            Console.WriteLine();

            Console.WriteLine("Checking used storage.");
            await Actions.CheckStorage.Execute(service);
            Console.WriteLine();

            switch (options.ActionFlow)
            {
              case ActionFlow.List:
                Console.WriteLine("Listing files from Google Drive.");
                await Actions.List.Execute(service, options as ListOptions);
                break;

              case ActionFlow.Download:
                Console.WriteLine("Downloading files from Google Drive.");
                await Actions.Download.Execute(service, options as DownloadOptions);
                break;

              case ActionFlow.Upload:
                Console.WriteLine("Uploading files to Google Drive.");
                await Actions.Upload.Execute(service, options as UploadOptions);
                break;

        
              case ActionFlow.Delete:
                Console.WriteLine("Deleting files from Google Drive.");
                await Actions.Delete.Execute(service);
                break;
            }
            Console.WriteLine();

            Console.WriteLine("Checking used storage.");
            await Actions.CheckStorage.Execute(service);
            Console.WriteLine();

            Console.WriteLine("Finished execution. Shutting down application.");
            return 0;
        }

        private static bool ParseArguments(string[] args)
        {
            Func<BaseOptions, int> HandleOptions = new Func<BaseOptions, int>(opts =>
            {
                options = opts;
                return 0;
            });

            var result = Parser.Default.ParseArguments<ListOptions, DownloadOptions, UploadOptions, DeleteOptions>(args)
                .MapResult(
                    (ListOptions opts) => HandleOptions(opts),
                    (DownloadOptions opts) => HandleOptions(opts),
                    (UploadOptions opts) => HandleOptions(opts),
                    (DeleteOptions opts) => HandleOptions(opts),
                    errors => -1);

            return result != -1;
        }
    }
}
