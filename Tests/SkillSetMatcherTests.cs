using System.Collections.Generic;
using System.IO;
using System.Linq;
using NReco.Text;
using NUnit.Framework;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.OutputFormatter.Json;
using Sharpenter.ResumeParser.ResumeProcessor;

namespace Tests
{
    [TestFixture]
    public class SkillSetMatcherTests
    {
        private SkillSetMapper _matcher;

        [SetUp]
        public void SetUp()
        {
            _matcher = new SkillSetMapper();
        }

        [Test]
        // In order this test to run, you need to create a Resumes folder in test execution directory and put some test resumés.
        public void TestAhoCorasickDoubleArrayTrieForManyResumes()
        {
            var processor = new ResumeProcessor(new JsonOutputFormatter());
            var filePaths = Directory.GetFiles("Resumes").Select(Path.GetFullPath);
            var acdat = new AhoCorasickDoubleArrayTrie<string>();
            var pairs = SkillSetMapper.ProgrammingLanguageSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString()));
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
                Assert.IsNotEmpty(collectedValues);
            }
        }

        [TestCase("MATLAB et C/C++ sous contraintes temps-réel. ")]
        public void TestAhoCorasickDoubleArrayTrieForSingleLine(string line)
        {
            var acdat = new AhoCorasickDoubleArrayTrie<string>();
            var pairs = SkillSetMapper.ProgrammingLanguageSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString()));
            acdat.Build(pairs, true);
            var collectedValues = new List<string>();
            acdat.ParseText(line, hit => { collectedValues.Add(hit.Value); return true; });
            Assert.IsNotEmpty(collectedValues);
            var collectedValuesresult = collectedValues.Where(i => SkillSetMapper.ProgrammingLanguageSkillSet.ElementAtOrDefault(int.Parse(i)) == null);
            Assert.IsEmpty(collectedValuesresult);
            var keyWord = SkillSetMapper.ProgrammingLanguageSkillSet[int.Parse(collectedValues.FirstOrDefault())].Trim();
            Assert.True(keyWord == "C/C++");
        }

        [Test]
        public void TestProgrammingAndToolSetSkillMatcher()
        {
            var processor = new ResumeProcessor(new JsonOutputFormatter());
            var filePaths = Directory.GetFiles("Resumes").Select(Path.GetFullPath);

            foreach (var filePath in filePaths)
            {
                var resume = new Resume();
                var fileName = Path.GetFileName(filePath);
                var rawInput = processor._inputReaders.ReadIntoList(filePath);

                foreach (var line in rawInput)
                {
                    _matcher.GetProgrammingLanguageSkillSet(resume, line);
                    _matcher.GetToolSkillSet(resume, line);
                }
                Assert.IsNotEmpty(resume.Skills.ProgrammingLanguageSkills);
                Assert.IsNotEmpty(resume.Skills.ToolSkillSet);
            }
        }
    }
}
