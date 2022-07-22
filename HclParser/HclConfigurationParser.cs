namespace Bongobin.HclParser;

public class HclConfigurationParser
{
    public static HclParseResult Parse(string input)
    {
        var document = HclDocument.New();
        return document.Parse(input);
    }
}