using Google.Apis.Drive.v3;
using RemoteFileManager.ExtensionMethods;
using RemoteFileManager.Options;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class List
    {
        public static async Task Execute(DriveService service, ListOptions opts)
        {
            var fileRequest = service.Files.List();
            fileRequest.Fields = "*";
            
            var files = (await fileRequest.ExecuteAsync()).Files;
            files = files
                .Where(f => f.MimeType != Constants.MimeTypes.Folder && Regex.IsMatch(f.Name, opts.Regex))
                .OrderBy(f => f.Name)
                .ToList();
            
            foreach (var file in files)
                Console.WriteLine($"{file.Name}: {file.Size.Format()}.");
        }
    }
}
