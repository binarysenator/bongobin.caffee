using NUnit.Framework;

namespace Bongobin.HclParser.Tests
{
    [TestFixture]
    public class HclVariableGroupTests
    {
        [Test]
        public void CreateEmpty_VariableGroupMergeWithEmpty_ExpectSuccess()
        {
            var group1 = new HclVariableGroup("group1", string.Empty);
            var group2 = new HclVariableGroup("group1", string.Empty);
            group1.MergeWith(group2);
            Assert.AreEqual(0, group1.Children.Count);
        }

        [Test]
        public void CreateSingleVariableInSource_VariableGroupMergeWithEmpty_ExpectSuccess()
        {
            var group1 = new HclVariableGroup("group1", string.Empty);
            group1.Add(new HclStringVariableNode("abc", "pdq", string.Empty));

            var group2 = new HclVariableGroup("group1", string.Empty);
            group1.MergeWith(group2);

            Assert.AreEqual(1, group1.Children.Count);
        }

        [Test]
        public void CreateSingleVariableInDestination_VariableGroupMergeWithEmpty_ExpectSuccess()
        {
            var group1 = new HclVariableGroup("group1", string.Empty);
            var group2 = new HclVariableGroup("group1", string.Empty);
            group2.Add(new HclStringVariableNode("abc", "pdq", string.Empty));
            group1.MergeWith(group2);
            Assert.AreEqual(1, group1.Children.Count);
        }
    }
}
