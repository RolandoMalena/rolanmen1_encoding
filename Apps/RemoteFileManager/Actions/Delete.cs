using Google.Apis.Drive.v3;
using RemoteFileManager.Options;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class Delete
    {
        public static async Task Execute(DriveService service, DeleteOptions opts)
        {
            var files = (await service.Files.List().ExecuteAsync())
                .Files
                .Where(f => Regex.IsMatch(f.Name, opts.Regex))
                .ToList();

            Console.WriteLine($"Found {files.Count} file(s). Deleting file(s).");
            foreach(var f in files)
                await service.Files.Delete(f.Id).ExecuteAsync();
        }
    }
}
