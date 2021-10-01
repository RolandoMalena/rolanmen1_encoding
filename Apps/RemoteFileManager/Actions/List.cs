using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using RemoteFileManager.ExtensionMethods;
using RemoteFileManager.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class List
    {
        public static async Task Execute(DriveService service, ListOptions opts)
        {
            string nextPageToken = null;
            var fileList = new List<File>();

            while (true)
            {
                var fileRequest = service.Files.List();
                fileRequest.Fields = "*";
                fileRequest.PageToken = nextPageToken;

                var response = await fileRequest.ExecuteAsync();
                fileList.AddRange(response.Files
                    .Where(f => f.MimeType != Constants.MimeTypes.Folder && Regex.IsMatch(f.Name, opts.Regex))
                    .OrderBy(f => f.Name));

                nextPageToken = response.NextPageToken;
                if (nextPageToken == null)
                    break;
            }
            
            foreach (var file in fileList)
                Console.WriteLine($"{file.Name}: {file.Size.Format()}.");
        }
    }
}
