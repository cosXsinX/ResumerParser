using System.Collections.Generic;
using System.Linq;
using NReco.Text;
using Sharpenter.ResumeParser.Model;

namespace Sharpenter.ResumeParser.ResumeProcessor
{
    public class SkillSetMapper
    {
        public static readonly string[] SkillSet = new string[] { "VISUAL BASIC", "VBA", "EXCEL-DNA", "EXCELDNA", " JAVA", " .NET", " ASP.NET", " C++", " C/C++", " C#", " HTML", " CSS", " LINUX", " JAVA SCRIPT", " PHP", " BOOTSTRAP", " JSON", "XML", " J2EE", " SPRING", " JAVASCRIPT", " NODE.JS", " NODEJS", " PHP", " NUXTS", " VUEJS", " RXJS", " REACTIVE", " QUANTLIB", " QNET", " SWIFT", " REACT  ", "PYTHON", " ANGULAR", " WEBAPI", " RESTFFULL", " RESTFFULLAPI", " REST", " API", " WEBFRAMEWORK", " SOAP", " W3C", " HTTP", "MATLAB","GIT","JENKINS","DEVOPS","JIRA","PACK OFFICE","OFFICE","ORIGIN","UML","ANDROID STUDIO","VISUAL STUDIO","VS CODE","VISUAL STUDIO CODE","SAS","WORD","EXCEL","POWERPOINT","POWER POINT","VISIO","MS PROJECT","MSPROJECT","SONAR","SELINIUM","AIX","GOOGLE ANALYTICS","WORDPRESS","SYMPHONY","ELASTICSEARCH","NUXTJS","VUSAX","BULMA","ELEMENT","KIBANA","SONARCUBE","DOCKER", "SCRUM", "ITIL", "TDD", "BDD", "DDD", "DESING PATTERN", "DESING PATTERNS", "SOLID", "AGILE", "FIX", "TIBCO", "NATS", "SHELL LINUX", "LINUX", "WINDOWS SERVER", "WINDOWS SERVEUR", "UBUNTU", "IIS", "SQL SERVER","ORACLE","MONGODB","MONGO DB","JBOSS","MYSQL","SQL","NOSQL","POSTGRESQL","TRANSACT-SQL","TRANSACT SQL","POSTGRE SQL","DAPPER","ENTITY FRAMEWORK" };

        private AhoCorasickDoubleArrayTrie<string> __skillSetMatcher;

        public SkillSetMapper()
        {
            __skillSetMatcher = new AhoCorasickDoubleArrayTrie<string>();
            __skillSetMatcher.Build(SkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString())), true);
        }

        public void GetSkillSet(Resume resume, string line)
        {
            var collectedIndexes = new HashSet<int>();
            __skillSetMatcher.ParseText(line, hit => { collectedIndexes.Add(int.Parse(hit.Value)); return true; });
            foreach (var index in collectedIndexes)
            {
                var element = SkillSet.ElementAtOrDefault(index);
                if(element != null)
                {
                    resume.Skills.Add(element.Trim());
                }
            }
        }
    }
}
