using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptSegmentation
{
  class Program
  {
    static int current = 1;
    static int segments;
    static string scriptPath;
    static string outputPath;

    /*
     * Arguments:
     * Segments: Number of segments to split the script into.
     * ScriptPath: Input script file path (relative or absolute) to split.
     * OutputPath: Output directory path (relative or absolute) to dump splitted files to.
     * 
     * Examples:
     * .exe 20 final.avs Segments
     * .exe 15 C:/Encoding/final.avs C:/Encoding/Segments
     */
    static async Task Main(string[] args)
    {
      Console.WriteLine("Starting Script Segmentation.");
      SetVariables(args);
      Console.WriteLine();

      Console.WriteLine("Deleting Script Files.");
      DeleteScripts();
      Console.WriteLine();

      Console.WriteLine("Creating Script Files.");
      await CreateScripts();
      Console.WriteLine();

      Console.WriteLine("Shutting Down application.");
    }

    static void SetVariables(string[] args)
    {
      segments = int.Parse(args[0]);

      if (!Path.IsPathRooted(args[1]))
        scriptPath = Path.Combine(Environment.CurrentDirectory, args[1]);
      else
        scriptPath = args[1];

      if (!Path.IsPathRooted(args[2]))
        outputPath = Path.Combine(Environment.CurrentDirectory, args[2]);
      else
        outputPath = args[2];

      Console.WriteLine($"Segments: {segments}");
      Console.WriteLine($"Script Path: {scriptPath}");
      Console.WriteLine($"Output Path: {outputPath}");
    }

    static void DeleteScripts()
    {
      if (!Directory.Exists(outputPath))
      {
        Directory.CreateDirectory(outputPath);
        Console.WriteLine("0 script(s) detected.");
        return;
      }

      var files = Directory.GetFiles(outputPath).Where(f => f.EndsWith(".avs"));
      var scriptCount = files.Count();

      Console.WriteLine($"{scriptCount} script(s) detected.");

      foreach (var file in files)
      {
        Console.WriteLine($"Deleting file: {Path.GetFileName(file)}");
        File.Delete(file);
      }
    }

    static async Task CreateScripts()
    {
      string[] trimlines = GenerateTrimLines();
      
      while (current <= segments)
      {
        string fileName = Path.Combine(outputPath, $"{current}.avs");
        string scriptStart = $"Import(\"{scriptPath.Replace('\\', '/')}\")";

        Console.WriteLine($"Creating Script File: {Path.GetFileName(fileName)}");

        using (var write = new StreamWriter(fileName, false))
        {
          await write.WriteLineAsync(scriptStart);
          await write.WriteLineAsync();
          await write.WriteLineAsync("#Trimming");

          for (int i = 0; i < trimlines.Length; i++)
          {
            if (current - 1 == i)
              await write.WriteLineAsync(trimlines[i].Replace("#", ""));
            else
              await write.WriteLineAsync(trimlines[i]);
          }
        };

        current++;
      }
    }

    static string[] GenerateTrimLines()
    {
      var trimlines = new string[segments];
      string currentLine;

      for (int i = 0; i < segments; i++)
      {
        if (i == 0)
          currentLine = $"#Trim(0, Floor(FrameCount/{segments})*{i+1})";
        else if (i != segments - 1)
          currentLine = $"#Trim(Floor(FrameCount/{segments})*{i}+1, Floor(FrameCount/{segments})*{i+1})";
        else
          currentLine = $"#Trim(Floor(FrameCount/{segments})*{i}+1, 0)";
        
        trimlines[i] = currentLine;
      }

      return trimlines;
    }
  }
}
