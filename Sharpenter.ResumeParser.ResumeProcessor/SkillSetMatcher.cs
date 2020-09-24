using System.Collections.Generic;
using System.Linq;
using NReco.Text;
using Sharpenter.ResumeParser.Model;

namespace Sharpenter.ResumeParser.ResumeProcessor
{
    public class SkillSetMatcher
    {
        public static readonly string[] ProgrammingLanguageSkillSet = new string[] { "VISUAL BASIC", "VBA", "EXCEL-DNA", "EXCELDNA", " JAVA", " .NET", " ASP.NET", " C++", " C/C++", " C#", " HTML", " CSS", " LINUX", " JAVA SCRIPT", " PHP", " BOOTSTRAP", " JSON", "XML", " J2EE", " SPRING", " JAVASCRIPT", " NODE.JS", " NODEJS", " PHP", " NUXTS", " VUEJS", " RXJS", " REACTIVE", " QUANTLIB", " QNET", " SWIFT", " REACT  ", "PYTHON", " ANGULAR", " WEBAPI", " RESTFFULL", " RESTFFULLAPI", " REST", " API", " WEBFRAMEWORK", " SOAP", " W3C", " HTTP" };
        //public static readonly string[] methodologySkillSet = new string[] { };     
        //public static readonly string[] toolSkillSet = new string[] { };

        private AhoCorasickDoubleArrayTrie<string> _programmingLanguageSkillMatcher;

        public SkillSetMatcher()
        {
            _programmingLanguageSkillMatcher = new AhoCorasickDoubleArrayTrie<string>();
            _programmingLanguageSkillMatcher.Build(ProgrammingLanguageSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString())), true);
        }

        public void GetProgrammingLanguageSkillSet(Resume resume, string line)
        {
            var collectedIndexes = new HashSet<int>();
            _programmingLanguageSkillMatcher.ParseText(line, hit => { collectedIndexes.Add(int.Parse(hit.Value)); return true; });
            foreach (var index in collectedIndexes)
            {
                var element = ProgrammingLanguageSkillSet.ElementAtOrDefault(index);
                if(element != null)
                {
                    resume.Skills.ProgrammingLanguageSkills.Add(element);
                }
            }
        }
    }
}
