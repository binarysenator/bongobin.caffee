using System;
using System.IO;
using System.Reflection;

namespace Bongobin.HclParser.Tests;

public class ResourceReader
{
    private readonly Assembly _assembly;

    public ResourceReader()
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    public static string Read(string name)
    {
        var reader = new ResourceReader();
        return reader.ReadString(name);
    }

    public string ReadString (string name)
    {
        var stream = _assembly.GetManifestResourceStream(name);
        if (stream == null) throw new ArgumentNullException(nameof(name), "Unable to read assembly stream.");
        var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}