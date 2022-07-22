using Bongobin.HclParser;
using Bongobin.HclParser.Exceptions;
using CommandLine;

namespace caffee;

[Verb("merge", HelpText = "Merges all tfvars files into a single file")]
public class MergeCommand : ICommand
{
    [Option('d', "directory", HelpText = "The working directory in which the merge will take place.")]
    public string? WorkingDirectory { get; set; }
    public string? OutputDirectory { get; set; }
    public string? OutputFilename { get; set; } = "bongod.tfvars";

    public void Execute()
    {
        // Collect all tf-vars files and then load them in one at a time
        var workingDirectory = GetWorkingDirectory();
        if (!Directory.Exists(workingDirectory))
        {
            throw new ParserException($"The working directory {workingDirectory} doesn't exist, or cannot be accessed.");
        }

        var tfVars = Directory.GetFiles(workingDirectory, "*.tfvars");

        if (!tfVars.Any())
        {
            Console.WriteLine($"No tfvars files located at working directory {workingDirectory}");
            return;
        }

        var files = string.Join(",", tfVars);
        Console.WriteLine($"Executing merge on files:");

        var result = HclDocument.New();

        foreach (var file in tfVars)
        {
            var hcl = HclDocument.FromFile(file);
            result.Merge(hcl);
        }
        
        var outputFile = GetOutputFileName();
        Console.WriteLine($"Writing output file to {outputFile}...");

        result.SaveAs(outputFile);
    }

    private string GetOutputFileName()
    {
        if (OutputDirectory != null)
        {
            return Path.Join(OutputDirectory, OutputFilename);
        }
        else
        {
            return Path.Join(GetWorkingDirectory(), OutputFilename); // Use working directory instead.
        }
    }

    private string GetOutputDirectory()
    {
        if (OutputDirectory != null)
        {
            return OutputDirectory;
        }
        else
        {
            return Directory.GetCurrentDirectory();
        }
    }

    private string GetWorkingDirectory()
    {
        if (WorkingDirectory != null)
        {
            return WorkingDirectory;
        }
        else
        {
            return Directory.GetCurrentDirectory();
        }
    }
}