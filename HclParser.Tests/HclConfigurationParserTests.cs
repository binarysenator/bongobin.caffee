using System.Linq;
using NUnit.Framework;

namespace Bongobin.HclParser.Tests
{
    public class HclConfigurationParserTests
    {
        [Test]
        public void Parse_GlobalOnly_ExpectDocumentSingleGroup_NestedVariables()
        {
            var hcl = ResourceReader.Read("Bongobin.HclParser.Tests.TestFiles.Caf_GlobalOnly.txt");
            var result = HclConfigurationParser.Parse(hcl);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Document);
            Assert.IsNotNull(result.Document.Children);
            Assert.AreEqual(1, result.Document.Children.Count);
            Assert.AreEqual(1, result.Document.Groups.Count);
            Assert.AreEqual(5, result.Document.Children.First().Children.Count);
            Assert.AreEqual(1, result.Document.Children.First().Groups.Count); // 1 nested group.
        }

        [Test]
        public void Parse_GlobalOnly_ExpectDocumentSingleGroupSimple_NoNestedVariables()
        {
            var hcl = ResourceReader.Read("Bongobin.HclParser.Tests.TestFiles.Caf_GlobalSimple.txt");
            var result = HclConfigurationParser.Parse(hcl);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Document);
            Assert.IsNotNull(result.Document.Children);
            Assert.AreEqual(1, result.Document.Children.Count);
            Assert.AreEqual(1, result.Document.Groups.Count);
            Assert.AreEqual(1, result.Document.Children.First().Children.Count);
        }

        [Test]
        public void Parse_GlobalOnly_ExpectDocumentDualGroupSimple_FlatVariables()
        {
            var hcl = ResourceReader.Read("Bongobin.HclParser.Tests.TestFiles.Caf_2GroupsSimple.txt");
            var result = HclConfigurationParser.Parse(hcl);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Document);
            Assert.IsNotNull(result.Document.Children);
            Assert.AreEqual(2, result.Document.Groups.Count);
        }

        [Test]
        public void Parse_GlobalOnly_ExpectDocumentDualGroup_NestedVariables()
        {
            var hcl = ResourceReader.Read("Bongobin.HclParser.Tests.TestFiles.Caf_2Groups.txt");
            var result = HclConfigurationParser.Parse(hcl);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Document);
            Assert.IsNotNull(result.Document.Children);
            Assert.AreEqual(2, result.Document.Groups.Count);
        }

        [Test]
        public void Parse_Full_ExpectSuccess()
        {
            var hcl = ResourceReader.Read("Bongobin.HclParser.Tests.TestFiles.Caf1.txt");
            var result = HclConfigurationParser.Parse(hcl);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Document);
            Assert.IsNotNull(result.Document.Children);


            var namespacesGroup = result.Document["api_management_namespaces"];
            Assert.IsNotNull(namespacesGroup);

            var clusterGroup = result.Document["aks_clusters"];
            Assert.IsNotNull(clusterGroup);
            Assert.AreEqual("aks_clusters", clusterGroup?.Name);
            Assert.AreEqual(1, clusterGroup?.Groups.Count);
            var clusterOne = clusterGroup?["cluster_1"];
            Assert.AreEqual("cluster_1", clusterOne?.Name);


            Assert.AreEqual(16, result.Document.Groups.Count);
        }

        [Test]
        public void Parse_UnParseGlobalSimple_NoChanges()
        {
            var hcl = ResourceReader.Read("Bongobin.HclParser.Tests.TestFiles.Caf_GlobalSimple.txt");
            var result = HclConfigurationParser.Parse(hcl);
            var raw = result.Document.Raw;
            Assert.AreEqual(hcl, raw);
        }

        [Test]
        public void Parse_UnParseGlobalOnly_NoChanges()
        {
            var hcl = ResourceReader.Read("Bongobin.HclParser.Tests.TestFiles.Caf_GlobalOnly.txt");
            var result = HclConfigurationParser.Parse(hcl);
            var raw = result.Document.Raw;
            Assert.AreEqual(hcl, raw);
        }

        [Test]
        public void Parse_UnParseFullCaf1_NoChanges()
        {
            var hcl = ResourceReader.Read("Bongobin.HclParser.Tests.TestFiles.Caf1.txt");
            var result = HclConfigurationParser.Parse(hcl);
            var raw = result.Document.Raw;
            Assert.AreEqual(hcl, raw);
        }
    }
}