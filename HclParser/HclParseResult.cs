namespace Bongobin.HclParser;

public class HclParseResult
{
    public HclParseResult (HclDocument document)
    {
        Document = document;
    }

    public HclDocument Document { get; }
}