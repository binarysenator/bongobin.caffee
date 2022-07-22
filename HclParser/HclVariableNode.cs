using System.Diagnostics;

namespace Bongobin.HclParser;

[DebuggerDisplay("{Name} = {DisplayValue")]
public abstract class HclVariableNode : HclNode
{
    public string Name { get; }

    protected HclVariableNode(string name, string raw) : base(raw)
    {
        Name = name;
    }

    public abstract string DisplayValue { get; }
}