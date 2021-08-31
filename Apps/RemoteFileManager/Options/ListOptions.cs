using CommandLine;
using System;

namespace RemoteFileManager.Options
{
    [Verb("ls", HelpText = "List all files from Google Drive.")]
    public class ListOptions : BaseOptions
    {
        public override ActionFlow ActionFlow => ActionFlow.List;

        public override void PrintOptions()
        {
            Console.WriteLine($"Action: {ActionFlow}");
        }
    }
}
