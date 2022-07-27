using Bongobin.HclParser;
using Bongobin.HclParser.Exceptions;
using caffee;
using CommandLine;
using CommandLine.Text;

namespace Bongobin.Caffee;

//[Verb("set", HelpText = "Sets a given variable on the document.")]
//public class SetCommand : ICommand
//{
//    [Option('s', "variable", HelpText = "The variable to set the value for.")]
//    public string? Variable { get; set; }
    
//    public string? Value { get; set; }
    
//    public void Execute()
//    {
//        return;
//    }
//}

[Verb("merge", HelpText = "Merges all tfvars files into a single file")]
public class MergeCommand : ICommand
{
    [Option('d', "directory", HelpText = "The working directory in which the merge will take place.")]
    public string? WorkingDirectory { get; set; }
    public string? OutputDirectory { get; set; }
    public string? OutputFilename { get; set; } = "bongod.tfvars";

    [Option('v', "variable", Required = false, HelpText = "Variables to set.")]
    public IEnumerable<string> VariableSettings { get; set; }

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

        Console.WriteLine("Executing merge on files:");

        var result = HclDocument.New();

        foreach (var file in tfVars)
        {
            var hcl = HclDocument.FromFile(file);
            result.Merge(hcl);
        }


        foreach (var variableSetting in VariableSettings)
        {
            var parts = variableSetting.Split('=', 2, StringSplitOptions.TrimEntries);
            var variableName = parts[0];
            var variableValue = string.Empty;

            if (parts.Length == 2)
            {
                variableValue = parts[1];
            }

            Console.WriteLine($"Setting variable {variableName} to {variableValue}...");
            // We break the parts up into variable paths by using dot notation e.g. group.group.variable

        }

        var outputFile = GetOutputFileName();
        Console.WriteLine($"Writing output file to {outputFile}...");

        result.SaveAs(outputFile);
    }

    private string GetOutputFileName()
    {
        return Path.Join(OutputDirectory ?? GetWorkingDirectory(), OutputFilename);
    }

    private string GetWorkingDirectory()
    {
        return WorkingDirectory ?? Directory.GetCurrentDirectory();
    }
}