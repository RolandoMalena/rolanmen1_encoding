using Google.Apis.Download;
using Google.Apis.Drive.v3;
using RemoteFileManager.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class Download
    {
        public static async Task Execute(DriveService service, DownloadOptions opts)
        {
            string downloadPath = opts.LocalPath;
            string remotePath = opts.RemotePath;
            string regex = opts.Regex;

            if (remotePath == null || remotePath == ".")
                remotePath = string.Empty;

            var files = await GetFileList(service);

            if (remotePath == string.Empty)
                files = files.Where(f => !f.Name.Contains("/")).ToList();
            else
                files = files.Where(f => f.Name.StartsWith(remotePath)).ToList();

            if (!string.IsNullOrWhiteSpace(regex))
                files = files.Where(f => Regex.IsMatch(f.Name, regex)).ToList();

            Console.WriteLine($"Found {files.Count} matching files.");

            if (!Directory.Exists(downloadPath))
                Directory.CreateDirectory(downloadPath);

            foreach (var file in files)
            {
                Console.WriteLine($"Downloading file: {file.Name}");
                
                using (var fileStream = File.Create(Path.Combine(downloadPath, Path.GetFileName(file.Name))))
                {
                    var result = service.Files.Get(file.Id).DownloadWithStatus(fileStream);
                    
                    if (result.Status == DownloadStatus.Completed)
                        Console.WriteLine("File downloaded successfully.");
                    else
                        Console.WriteLine($"File download failed with message: {result.Exception.Message}");
                }
            }
        }

        private static async Task<IList<Google.Apis.Drive.v3.Data.File>> GetFileList(DriveService service)
        {
            string nextPageToken = null;
            var fileList = new List<Google.Apis.Drive.v3.Data.File>();

            while (true)
            {
                var fileRequest = service.Files.List();
                fileRequest.PageToken = nextPageToken;

                var response = await fileRequest.ExecuteAsync();
                fileList.AddRange(response.Files
                    .Where(f => f.MimeType != Constants.MimeTypes.Folder));

                nextPageToken = response.NextPageToken;
                if (nextPageToken == null)
                    break;
            }

            return fileList;
        }
    }
}
