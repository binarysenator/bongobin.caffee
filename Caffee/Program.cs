using caffee;
using CommandLine;

Parser.Default.ParseArguments<MergeCommand>(args).WithParsed(t => t.Execute());