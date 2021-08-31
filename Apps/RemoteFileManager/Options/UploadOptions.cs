using CommandLine;
using System;

namespace RemoteFileManager.Options
{
    [Verb("up", HelpText = "Upload files to Google Drive.")]
    public class UploadOptions : BaseOptions
    {
        [Value(index: 0, Required = true, HelpText = "Local path to upload files from.")]
        public string LocalPath { get; set; }

        [Value(index: 1, Required = true, HelpText = "Remote path to upload files to.")]
        public string RemotePath { get; set; }

        [Value(index: 2, Required = true, HelpText = "Regex used to filter in files.")]
        public string Regex { get; set; }

        public override ActionFlow ActionFlow => ActionFlow.Upload;

        public override void PrintOptions()
        {
            Console.WriteLine($"Action: {ActionFlow}");
            Console.WriteLine($"Local Path: {LocalPath}");
            Console.WriteLine($"Remote Path: {RemotePath}");
            Console.WriteLine($"File Regex: {Regex}");
        }
    }
}
