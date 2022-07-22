namespace Bongobin.HclParser
{
    public class HclStringVariableNode : HclVariableNode
    {
        public HclStringVariableNode(string name, string value, string raw) : base(name, raw)
        {
            Value = value;
        }

        public string Value { get; private set; }
        public override string DisplayValue => Value;
    }
}