using CommandLine;
using System;

namespace RemoteFileManager.Options
{
    [Verb("down", HelpText = "Download files from Google Drive.")]
    public class DownloadOptions : BaseOptions
    {
        [Value(index: 0, Required = true, HelpText = "Local path to download files to.")]
        public string LocalPath { get; set; }

        [Value(index: 1, Required = true, HelpText = "Remote path to download files from.")]
        public string RemotePath { get; set; }

        [Value(index: 2, Required = true, HelpText = "Regex used to filter in files.")]
        public string Regex { get; set; }

        public override ActionFlow ActionFlow => ActionFlow.Download;

        public override void PrintOptions()
        {
            Console.WriteLine($"Action: {ActionFlow}");
            Console.WriteLine($"Local Path: {LocalPath}");
            Console.WriteLine($"Remote Path: {RemotePath}");
            Console.WriteLine($"File Regex: {Regex}");
        }
    }
}
