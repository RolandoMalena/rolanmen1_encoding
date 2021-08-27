﻿using Google.Apis.Drive.v3;
using RemoteFileManager.ExtensionMethods;
using System;
using System.Threading.Tasks;

namespace RemoteFileManager.Actions
{
    public static class List
    {
        public static async Task Execute(DriveService service)
        {
            var fileRequest = service.Files.List();
            fileRequest.Fields = "*";
            var files = (await fileRequest.ExecuteAsync()).Files;

            foreach (var file in files)
                Console.WriteLine($"{file.Name}: {file.Size.Format()}.");
        }
    }
}
