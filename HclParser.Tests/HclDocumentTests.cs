using NUnit.Framework;

namespace Bongobin.HclParser.Tests
{
    [TestFixture]
    public class HclDocumentTests
    {
        [Test]
        public void Construct_HclDocumentNew_ExpectSuccess()
        {
            var document = HclDocument.New();
            Assert.IsNotNull(document);
            Assert.AreEqual(0, document.Children.Count);
            Assert.AreEqual(0, document.Groups.Count);
        }

        [Test]
        public void HclDocumentNew_MergeWith_NewExpectSuccess()
        {
            var document = HclDocument.New();
            var other = HclDocument.New();
            document.Merge(other);

            Assert.IsNotNull(document);
            Assert.AreEqual(0, document.Children.Count);
            Assert.AreEqual(0, document.Groups.Count);
        }
    }
}
