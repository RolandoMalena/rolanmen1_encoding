using Google.Apis.Drive.v3;
using Google.Apis.Upload;
using RemoteFileManager.ExtensionMethods;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class Upload
    {
        public static async Task Execute(DriveService service, string sourcePath, string remotePath, string regex)
        {
            var files = Directory
                .GetFiles(sourcePath)
                .Where(f => Regex.IsMatch(Path.GetFileName(f), regex));

            if (remotePath == null || remotePath == ".")
                remotePath = string.Empty;

            if(!string.IsNullOrEmpty(remotePath))
                await CreateFolder(service, remotePath);

            foreach (string filePath in files)
            {
                var fileName = Path.GetFileName(filePath);
                var fileSize = new FileInfo(filePath).Length;

                Console.WriteLine($"Uploading file: {fileName} ({fileSize.Format()})");

                await DeleteFileIfExists(service, fileName);

                var file = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = $"{remotePath}/{fileName}"
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

        private static async Task CreateFolder(DriveService service, string remotePath)
        {
            var file = new Google.Apis.Drive.v3.Data.File()
            {
                Name = remotePath,
                MimeType = Constants.MimeTypes.Folder
            };

            try
            {
                await service.Files.Create(file).ExecuteAsync();
                Console.WriteLine($"Folder created successfully.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Folder failed to be created with message: {ex.Message}");
            }
        }

        private static async Task DeleteFileIfExists(DriveService service, string fileName)
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
    }
}
