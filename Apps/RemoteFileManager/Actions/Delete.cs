using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using RemoteFileManager.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class Delete
    {
        public static async Task Execute(DriveService service, DeleteOptions opts)
        {
            string nextPageToken = null;
            var fileList = new List<File>();

            while (true)
            {
                var fileRequest = service.Files.List();
                fileRequest.PageToken = nextPageToken;

                var response = await fileRequest.ExecuteAsync();
                fileList.AddRange(response.Files
                    .Where(f => Regex.IsMatch(f.Name, opts.Regex)));

                nextPageToken = response.NextPageToken;
                if (nextPageToken == null)
                    break;
            }

            Console.WriteLine($"Found {fileList.Count} file(s). Deleting file(s).");
            foreach (var f in fileList)
                await service.Files.Delete(f.Id).ExecuteAsync();
        }
    }
}
