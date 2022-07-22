using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Bongobin.HclParser;

[DebuggerDisplay("Group {Name}")]
public class HclVariableGroup : HclNode
{
    public string Name { get; }

    public HclVariableGroup(string name, string raw) : base(raw)
    {
        Name = name;
    }

    public IReadOnlyCollection<HclVariableNode> Variables => new ReadOnlyCollection<HclVariableNode>(Children.Where(c => c is HclVariableNode).Cast<HclVariableNode>().ToList());

    public HclVariableGroup MergeWith(HclVariableGroup group)
    {
        // Add current level variables
        foreach (var item in group.Variables)
        {
            // Do we merge variables with the same name or keep the later or the former or explode?
            Add(item);
        }

        foreach (var groups in group.Groups)
        {

        }

        return this;
    }

    public HclVariableGroup Clone()
    {
        return new HclVariableGroup(Name, Raw);
    }
}