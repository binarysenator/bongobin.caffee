using System.Text;

namespace Bongobin.HclParser;

public class HclDocument : HclNode
{
    static readonly HclDocument Empty = new HclDocument();
    
    public HclParseResult Parse(string input)
    {
        if ( string.IsNullOrWhiteSpace(input) ) return new HclParseResult(HclDocument.Empty);
        var buffer = new StringBuilder();
        var reader = new StringReader(input);
        ParseNode(this, reader);
        return new HclParseResult(this);
    }

    private HclDocument() : base(string.Empty) { }

    protected override string? RawText => null;

    public static HclDocument New()
    {
        return new HclDocument();
    }

    public HclDocument Merge(HclDocument document)
    {
        // Go through each node in the provided document and then merge this back into this existing document.
        foreach (var group in document.Groups)
        {
            // First try and find a match in the node structure in the parent
            var localGroup = Groups.FirstOrDefault(grp => grp.Name == group.Name);
            if (localGroup != null)
            {
                localGroup.MergeWith(group);
            }
            else
            {
                Add(group.Clone());
            }
        }

        return this;
    }

    public void SaveAs(string outputFile)
    {
        using var writer = File.CreateText(outputFile);
        writer.Write(Raw);
        writer.Flush();
        writer.Close();
        
    }

    public static HclDocument FromFile(string file)
    {
        using var reader = File.OpenText(file);
        var document = new HclDocument();
        document.Parse(reader.ReadToEnd());
        return document;
    }
}