using Google.Apis.Drive.v3;
using RemoteFileManager.ExtensionMethods;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class CheckStorage
    {
        public static async Task Execute(DriveService service)
        {
            int fileCount = 0;
            long totalKnownSize = 0;
            string nextPageToken = null;

            while (true)
            {
                var fileRequest = service.Files.List();
                fileRequest.Fields = "*";
                fileRequest.PageToken = nextPageToken;

                var response = await fileRequest.ExecuteAsync();
                var files = response.Files
                    .Where(f => f.MimeType != Constants.MimeTypes.Folder)
                    .ToArray();

                fileCount += files.Length;
                totalKnownSize += files.Sum(f => f.Size.Value);

                nextPageToken = response.NextPageToken;
                if (nextPageToken == null)
                    break;
            }

            Console.WriteLine($"Found {fileCount} files. Total size used: {totalKnownSize.Format()}.");
        }
    }
}
