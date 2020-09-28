using System.Collections.Generic;
using System.IO;
using System.Linq;
using NReco.Text;
using NUnit.Framework;
using Sharpenter.ResumeParser.OutputFormatter.Json;
using Sharpenter.ResumeParser.ResumeProcessor;

namespace Tests
{
    [TestFixture]
    public class SkillSetMatcherTests
    {
        [Test]
        // In order this test to run, you need to create a Resumes folder in test execution directory and put some test resumés.
        // Only technical skill matching is set up, to test with technical profiles.
        public void TestAhoCorasickDoubleArrayTrieForManyResumes()
        {
            var processor = new ResumeProcessor(new JsonOutputFormatter());
            var filePaths = Directory.GetFiles("Resumes").Select(Path.GetFullPath);
            var acdat = new AhoCorasickDoubleArrayTrie<string>();
            var pairs = SkillSetMapper.SkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString()));
            acdat.Build(pairs, true);

            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileName(filePath);
                var rawInput = processor._inputReaders.ReadIntoList(filePath);

                var collectedValues = new List<string>();
                foreach (var line in rawInput)
                {
                    acdat.ParseText(line, hit => { collectedValues.Add(hit.Value); return true; });
                }
                Assert.IsNotEmpty(collectedValues, $"No match found in file: {filePath}");
            }
        }

        [TestCase("MATLAB et C/C++ sous contraintes temps-réel. ")]
        public void TestAhoCorasickDoubleArrayTrieForSingleLine(string line)
        {
            var acdat = new AhoCorasickDoubleArrayTrie<string>();
            var pairs = SkillSetMapper.SkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString()));
            acdat.Build(pairs, true);
            var collectedValues = new List<string>();
            acdat.ParseText(line, hit => { collectedValues.Add(hit.Value); return true; });
            Assert.IsNotEmpty(collectedValues);
            var collectedValuesresult = collectedValues.Where(i => SkillSetMapper.SkillSet.ElementAtOrDefault(int.Parse(i)) == null);
            Assert.IsEmpty(collectedValuesresult);
            var keyWord = SkillSetMapper.SkillSet[int.Parse(collectedValues.FirstOrDefault())].Trim();
            Assert.True(keyWord == "MATLAB"); 
        }
    }
}
