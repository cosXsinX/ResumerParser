using System.IO;
using System.Linq;
using NUnit.Framework;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.OutputFormatter.Json;
using Sharpenter.ResumeParser.ResumeProcessor;

namespace Tests
{
    [TestFixture]
    public class SkillSetMatcherTests
    {
        private SkillSetMatcher _matcher;

        [SetUp]
        public void SetUp()
        {
            _matcher = new SkillSetMatcher();
        }

        [Test]
        public void TestProgrammingSkillMatcher()
        {
            var resume = new Resume();
            var processor = new ResumeProcessor(new JsonOutputFormatter());
            var filePaths = Directory.GetFiles("Resumes").Select(Path.GetFullPath);

            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileName(filePath);
                var rawInput = processor._inputReaders.ReadIntoList(filePath);

                foreach (var line in rawInput)
                {
                    _matcher.GetProgrammingLanguageSkillSet(resume, line);
                }
                Assert.IsNotEmpty(resume.Skills.ProgrammingLanguageSkills);
            }
        }
    }
}
