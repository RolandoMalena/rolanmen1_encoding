using Google.Apis.Drive.v3;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class Delete
    {
        public static async Task Execute(DriveService service)
        {
            var files = (await service.Files.List().ExecuteAsync())
                .Files
                .ToList();

            Console.WriteLine($"Found {files.Count} file(s). Deleting file(s).");
            foreach(var f in files)
                await service.Files.Delete(f.Id).ExecuteAsync();
        }
    }
}
