using CommandLine;
using System;

namespace RemoteFileManager.Options
{
    [Verb("del", HelpText = "Delete files from Google Drive.")]
    public class DeleteOptions : BaseOptions
    {
        [Value(index: 0, Required = false, HelpText = "Regex used to filter in files.", Default = "")]
        public string Regex { get; set; }
        
        public override ActionFlow ActionFlow => ActionFlow.Delete;

        public override void PrintOptions()
        {
            Console.WriteLine($"Action: {ActionFlow}");
            Console.WriteLine($"File Regex: {Regex}");
        }
    }
}
