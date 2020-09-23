using NUnit.Framework;
using System.Text.RegularExpressions;
using Sharpenter.ResumeParser.ResumeProcessor.Parsers;
using Sharpenter.ResumeParser.Model;
using Ganss.Text;
using System.Linq;
using System;
using System.IO;
using Sharpenter.ResumeParser.ResumeProcessor;
using Sharpenter.ResumeParser.OutputFormatter.Json;
using System.Collections.Generic;

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
        public void TestAhoCorasickAlgo()
        {
            var keyWordArray = new string[] { "a", "ab", "bab", "bc", "bca", "c", "caa" }; 

            var ac = new AhoCorasick(keyWordArray);
            var results = ac.Search("abccab").ToList();

            Assert.AreEqual(0, results[0].Index); // index into the searched text
            Assert.AreEqual("a", results[0].Word); // matched word
        }

        [Test]
        public void TestKeyWordSearchEngine()
        {
            var processor = new ResumeProcessor(new JsonOutputFormatter());
            var filePaths = Directory.GetFiles("Resumes").Select(Path.GetFullPath);

            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileName(filePath);
                var rawInput = processor._inputReaders.ReadIntoList(filePath);
                var ac = new AhoCorasick(KeywordSearchEngine.ProgrammingLanguageSkillSet);

                var programmingSkills =  new List<string>();
                foreach (var line in rawInput)
                {
                    programmingSkills.AddRange(ac.Search(line).Select(wm => wm.Word).ToList());
                }
                Assert.IsNotEmpty(programmingSkills);
            }
        }
    }
}