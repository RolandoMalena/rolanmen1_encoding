﻿using Google.Apis.Drive.v3;
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
            var fileRequest = service.Files.List();
            fileRequest.Fields = "*";
            var files = (await fileRequest.ExecuteAsync())
                .Files
                .Where(f => f.MimeType != Constants.MimeTypes.Folder)
                .ToArray();

            var totalKnownSize = files.Sum(f => f.Size.Value);
            Console.WriteLine($"Found {files.Length} files. Total size used: {totalKnownSize.Format()}.");
        }
    }
}