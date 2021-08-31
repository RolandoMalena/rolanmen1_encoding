using CommandLine;
using System;

namespace RemoteFileManager.Options
{
    [Verb("del", HelpText = "Delete files from Google Drive.")]
    public class DeleteOptions : BaseOptions
    {
        public override ActionFlow ActionFlow => ActionFlow.Delete;

        public override void PrintOptions()
        {
            Console.WriteLine($"Action: {ActionFlow}");
        }
    }
}
