using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Bongobin.HclParser;

public abstract class HclNode
{
    protected readonly List<HclNode> _children = new List<HclNode>();

    public const char OpenBrace = '{';
    public const char Assignment = '=';
    public const char CloseBrace = '}';
    public const string CloseBraceComma = "},";
    public const string EmptyGroup = "{}";
    protected string RawOpen;
    protected string? RawClose = null;

    protected HclNode(string raw)
    {
        RawOpen = raw;
    }

    protected virtual string? RawText => RawOpen;

    public virtual string Raw
    {
        get
        {
            var builder = new StringBuilder();
            
            if ( RawText != null )
                builder.AppendLine(RawText);

            _children.ForEach(c => builder.Append(c.Raw));

            if (RawClose != null)
            {
                builder.Append(RawClose);
            }

            return builder.ToString();
        }
        protected set => RawOpen = value;
    }

    public IReadOnlyCollection<HclNode> Children => _children;

    public IReadOnlyCollection<HclVariableGroup> Groups => new ReadOnlyCollection<HclVariableGroup>(_children.OfType<HclVariableGroup>().ToList());

    public HclVariableGroup? this[string name]
    {
        get { return Groups?.FirstOrDefault(g => g.Name.Equals(name)); }
    }

    protected virtual void ParseNode(HclNode parent, StringReader reader)
    {
        var currentLine = reader.ReadLine();
        if (currentLine == null) return;

        // Not whitespace so there is some content.
        var trimmed = currentLine.Trim();
        if (trimmed == string.Empty || trimmed.StartsWith('#'))
        {
            var ws = new WhiteSpaceHclNode(currentLine);
            parent.Add(ws);
        }

        if ( !trimmed.EndsWith(EmptyGroup) && (trimmed.EndsWith(CloseBrace) || trimmed.EndsWith((CloseBraceComma))) )
        {
            // Recurse back up as we have closed a section.
            if (trimmed == "}")
            {
                if (reader.Peek() < 0)
                {
                    parent.SetCloseText(currentLine); // If this is the last line then we don't add a linefeed.
                }
                else
                {
                    parent.SetCloseText(currentLine + Environment.NewLine); // If this is the last line then we don't add a linefeed.
                }
                return;
            }
        }

        // Try splitting up on equalities
        if (!trimmed.StartsWith('#'))
        {
            var assignment = trimmed.Split(Assignment);
            if (assignment.Length == 2)
            {
                var prefix = assignment[0].Trim();
                var postfix = assignment[1];

                // If the end of this line ends in a open brace then it's the start of a group
                if (postfix.EndsWith(OpenBrace))
                {
                    var group = new HclVariableGroup(prefix, currentLine);
                    group.ParseNode(group, reader);
                    parent.Add(group);
                }
                else if (postfix.EndsWith(EmptyGroup))
                {
                    // Likely to be a group that is empty
                    var group = new HclVariableGroup(prefix, currentLine);
                    parent.Add(group);
                }
                else
                {
                    // Just a variable.
                    var variable = new HclStringVariableNode(prefix, postfix, currentLine);
                    parent.Add(variable);
                }
            }
        }
        else
        {
            Debug.WriteLine("Dodged a bullet.");
        }

        ParseNode(parent, reader);
    }

    public void SetCloseText(string raw)
    {
        Debug.WriteLine($"Setting close text to {raw}");
        RawClose = raw;
    }

    public void Add(HclNode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));
        _children.Add(node);
    }
}