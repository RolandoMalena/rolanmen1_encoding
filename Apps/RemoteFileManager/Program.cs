using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoteFileManager
{
  class Program
    {
        static ActionFlow? action;
        static string localPath;
        static string fileRegex;

        static string ApplicationName = "Remote File Manager";
        static DriveService service;

        /*
         * Arguments:
         * Action: What action to make, options are 'up', 'down' or 'del'. If not provided will just check used storage.
         * LocalPath: Local path to download files to or upload files from. (Only used on 'up' or 'down')
         * 
         * Examples:
         * .exe
         * .exe down . file*.txt
         * .exe up C:/Encoding Encoding/Output
         * .exe del
         */
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Remote File Manager.");
            SetVariables(args);
            Console.WriteLine();

            Console.WriteLine("Authenticating to Google Drive.");
            AuthenticateToGoogleDrive();
            Console.WriteLine();


            Console.WriteLine("Checking used storage.");
            await CheckDriveUsedStorage();
            Console.WriteLine();

            if (action == ActionFlow.Upload)
            {
                Console.WriteLine("Uploading files to Google Drive.");
                await UploadFiles();
                Console.WriteLine();
            }
            else if (action == ActionFlow.Download)
            {
                Console.WriteLine("Downloading files from Google Drive.");
                await DownloadFiles();
                Console.WriteLine();
            }
            else if (action == ActionFlow.Delete)
            {
                Console.WriteLine("Deleting files from Google Drive.");
                await DeleteAllFiles();
                Console.WriteLine();
            }

            if (action != null)
            {
                Console.WriteLine("Checking used storage.");
                await CheckDriveUsedStorage();
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

        static void AuthenticateToGoogleDrive()
        {
            string[] Scopes = { DriveService.Scope.DriveFile };
            GoogleCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
                Console.WriteLine("Authenticated successfully.");
            }

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            service.HttpClient.Timeout = TimeSpan.FromMinutes(10);
        }

        static async Task CheckDriveUsedStorage()
        {
            var fileRequest = service.Files.List();
            fileRequest.Fields = "*";
            var files = (await fileRequest.ExecuteAsync()).Files;

            var totalKnownSize = files.Sum(f => f.Size.Value);
            Console.WriteLine($"Found {files.Count} files. Total size used: {GetFormattedFileSize(totalKnownSize)}.");
        }

        static string GetFormattedFileSize(long bytes)
        {
            var kbs = bytes / 1024;
            var mbs = bytes / 1048576;
            var gbs = bytes / 1073741824;

            return $"{bytes} Bytes / {kbs} KBs / {mbs} MBs / {gbs} GBs";
        }

        static async Task UploadFiles()
        {
            var files = Directory.GetFiles(localPath);
            foreach (string filePath in files)
            {
                var fileName = Path.GetFileName(filePath);
                var fileSize = new FileInfo(filePath).Length;

                Console.WriteLine($"Uploading file: {fileName} ({GetFormattedFileSize(fileSize)})");

                await DeleteFileIfExists(fileName);

                var file = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName
                };

                byte[] byteArray = File.ReadAllBytes(filePath);  
                var stream = new MemoryStream(byteArray);

                var result = await service.Files.Create(file, stream, "").UploadAsync();

                if (result.Status == UploadStatus.Completed)
                    Console.WriteLine($"File uploaded successfully.");
                else
                    Console.WriteLine($"File upload failed with message: {result.Exception.Message}");
            }
        }

        static async Task DeleteFileIfExists(string fileName)
        {
            var files = (await service.Files.List().ExecuteAsync())
                .Files
                .Where(f => f.Name == fileName)
                .ToList();

            if (!files.Any())
                return;
            
            Console.WriteLine($"Found {files.Count} file(s). Deleting file(s).");
            foreach(var f in files)
                await service.Files.Delete(f.Id).ExecuteAsync();
        }

        static async Task DeleteAllFiles()
        {
            var files = (await service.Files.List().ExecuteAsync())
                .Files
                .ToList();

            Console.WriteLine($"Found {files.Count} file(s). Deleting file(s).");
            foreach(var f in files)
                await service.Files.Delete(f.Id).ExecuteAsync();
        }

        static async Task DownloadFiles()
        {
            var files = (await service.Files.List().ExecuteAsync()).Files;

            if (!string.IsNullOrWhiteSpace(fileRegex))
                files = files.Where(f => Regex.IsMatch(f.Name, fileRegex)).ToList();

            Console.WriteLine($"Found {files.Count} matching files.");

            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            foreach (var file in files)
            {
                Console.WriteLine($"Downloading file: {file.Name}");
                
                using (var fileStream = File.Create(Path.Combine(localPath, file.Name)))
                {
                    var result = service.Files.Get(file.Id).DownloadWithStatus(fileStream);
                    
                    if (result.Status == DownloadStatus.Completed)
                        Console.WriteLine("File downloaded successfully.");
                    else
                        Console.WriteLine($"File download failed with message: {result.Exception.Message}");
                }
            }
        }
    }
}
