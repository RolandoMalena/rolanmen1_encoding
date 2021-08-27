using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.IO;

namespace RemoteFileManager.Actions
{
    public static class Authenticate
    {
        public static DriveService Execute(string applicationName)
        {
            string[] Scopes = { DriveService.Scope.DriveFile };
            GoogleCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
                Console.WriteLine("Authenticated successfully.");
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });

            service.HttpClient.Timeout = TimeSpan.FromMinutes(10);
            return service;
        }
    }
}
