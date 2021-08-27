using Google.Apis.Drive.v3;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RemoteFileManager
{
    class Program
    {
        static ActionFlow? action;
        static string localPath;
        static string fileRegex;

        const string ApplicationName = "Remote File Manager";
        static DriveService service;

        /*
         * Arguments:
         * Action: What action to make, options are 'ls', 'up', 'down' or 'del'. If not provided will just check used storage.
         * LocalPath: Local path to download files to or upload files from. (Only used on 'up' or 'down')
         * RemotePath: Folder path in Google Drive to upload files to or download/delete files from. (Only used on 'up', 'down' or 'del')
         * Regex: Regex used to determine which files to upload/download/delete.
         * 
         * Examples:
         * .exe
         * .exe ls
         * .exe down . file*.txt
         * .exe up C:/Encoding/Output clip1/output *.mkv
         * .exe del
         */
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Remote File Manager.");
            SetVariables(args);
            Console.WriteLine();

            Console.WriteLine("Authenticating to Google Drive.");
            service = Actions.Authenticate.Execute(ApplicationName);
            Console.WriteLine();


            Console.WriteLine("Checking used storage.");
            await Actions.CheckStorage.Execute(service);
            Console.WriteLine();

            if (action == ActionFlow.List)
            {
                Console.WriteLine("Listing files from Google Drive.");
                await Actions.List.Execute(service);
                Console.WriteLine();
            }
            else if (action == ActionFlow.Upload)
            {
                Console.WriteLine("Uploading files to Google Drive.");
                await Actions.Upload.Execute(service, localPath, fileRegex);
                Console.WriteLine();
            }
            else if (action == ActionFlow.Download)
            {
                Console.WriteLine("Downloading files from Google Drive.");
                await Actions.Download.Execute(service, localPath, fileRegex);
                Console.WriteLine();
            }
            else if (action == ActionFlow.Delete)
            {
                Console.WriteLine("Deleting files from Google Drive.");
                await Actions.Delete.Execute(service);
                Console.WriteLine();
            }

            if (action != null)
            {
                Console.WriteLine("Checking used storage.");
                await Actions.CheckStorage.Execute(service);
                Console.WriteLine();
            }

            Console.WriteLine("Finished execution. Shutting down application.");
        }

        static void SetVariables(string[] args)
        {
            if (args.Length >= 1)
            {
                var act = args[0].ToLower().Trim();
                switch (act)
                {
                    case "ls":
                        action = ActionFlow.List;
                        break;
                        
                    case "up":
                        action = ActionFlow.Upload;
                        break;

                    case "down":
                        action = ActionFlow.Download;
                        break;

                    case "del":
                        action = ActionFlow.Delete;
                        break;
                }
            }

            if (args.Length >= 2)
            {
                if (!Path.IsPathRooted(args[1]))
                    localPath = Path.Combine(Environment.CurrentDirectory, args[1]);
                else
                    localPath = args[1];
            }

            if (args.Length >= 3)
                fileRegex = args[2];

            Console.WriteLine($"Action: {action}");
            Console.WriteLine($"Local Path: {localPath}");
            Console.WriteLine($"File Regex: {fileRegex}");
        }
    }
}
