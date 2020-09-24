using NUnit.Framework;
using System.Text.RegularExpressions;
using Sharpenter.ResumeParser.ResumeProcessor.Parsers;
using Sharpenter.ResumeParser.Model;
using System.Linq;
using System.IO;
using Sharpenter.ResumeParser.ResumeProcessor;
using Sharpenter.ResumeParser.OutputFormatter.Json;
using System.Collections.Generic;
using NReco.Text;

namespace Tests
{
    [TestFixture]
    public class KeywordSearchEngineTests
    {
        private Regex EmailRegex;
        private Regex PhoneRegex;
        private KeywordSearchEngine _personalParser;

        [SetUp]
        public void SetUp()
        {
            EmailRegex = KeywordSearchEngine.EmailRegex;
            PhoneRegex = KeywordSearchEngine.PhoneRegex;
            _personalParser = new KeywordSearchEngine();
        }

        [TestCase("eref.fef@gmail.com")]
        [TestCase("andfdf2006@gmail.com")]
        [TestCase("fgfg-g.tran88@gmail.com")]
        [TestCase("5454_sdsd@yahoo.fr")]
        [TestCase("xoxox.ss_ext@eziti.eu")]
        [TestCase("sdsd.tran88@gmail.fr")]
        public void TestEmailRegexMatch(string email)
        {
            Assert.IsTrue(EmailRegex.Match(email).Success);
        }

        [TestCase("0033999888777")]
        [TestCase("+33999888777")]
        [TestCase("0999888777")]
        public void TestPhoneRegexMatch(string phoneNumber)
        {
            Assert.IsTrue(PhoneRegex.Match(phoneNumber).Success);
        }

        [TestCase("APEC_C++_FADAILI_Mos.pdf")]
        [TestCase("BI_Lesjeudis_Yassine-Hdidou (2).pdf")]
        [TestCase("DOTNET_Lesjeudis_Souhaib _Haj Messa")]
        [TestCase("FRONT_APEC_ DASNOIS_Benjamin.pdf")]
        [TestCase("Full Stack_DoYouBuzz_GOSSELIN_THOMA")]
        [TestCase("JAVA J2EE_APEC_Ghassan-JAHMI.doc")]
        [TestCase("MOA_Lesjeudis_OUHAMOU-YOUSSEF.pdf")]
        [TestCase("Python_APEC_Abdellatif-AMANSAG.pdf")]
        public void TestExtractFirstAndLastName(string fileName)
        {
            var resume = new Resume();
            resume.FileName = fileName;
            bool firstNamefound = false;
            firstNamefound = _personalParser.ExtractFirstAndLastName(resume, firstNamefound);
            Assert.True(firstNamefound);
            Assert.IsNotNull(resume.KeyWord);
            Assert.IsNotNull(resume.Platform);
            Assert.IsNotNull(resume.Name1);
        }

        [Test]
        // In order this test to run, you need to create a Resumes folder in test execution directory and put some test resumés.
        public void TestAhoCorasickDoubleArrayTrieForManyResumes()
        {
            var processor = new ResumeProcessor(new JsonOutputFormatter());
            var filePaths = Directory.GetFiles("Resumes").Select(Path.GetFullPath);
            var acdat = new AhoCorasickDoubleArrayTrie<string>();
            var pairs = SkillSetMatcher.ProgrammingLanguageSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString()));
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
            var pairs = SkillSetMatcher.ProgrammingLanguageSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString()));
            acdat.Build(pairs, true);
            var collectedValues = new List<string>();
            acdat.ParseText(line, hit => { collectedValues.Add(hit.Value); return true; });
            Assert.IsNotEmpty(collectedValues);
            var collectedValuesresult = collectedValues.Where(i => SkillSetMatcher.ProgrammingLanguageSkillSet.ElementAtOrDefault(int.Parse(i)) == null);
            Assert.IsEmpty(collectedValuesresult);
            var keyWord = SkillSetMatcher.ProgrammingLanguageSkillSet[int.Parse(collectedValues.FirstOrDefault())].Trim();
            Assert.True(keyWord == "C/C++");
        }
    }
}