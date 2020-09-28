using System.Collections.Generic;
using System.Linq;
using NReco.Text;
using Sharpenter.ResumeParser.Model;

namespace Sharpenter.ResumeParser.ResumeProcessor
{
    public class SkillSetMapper
    {
        public static readonly string[] ProgrammingLanguageSkillSet = new string[] { "VISUAL BASIC", "VBA", "EXCEL-DNA", "EXCELDNA", " JAVA", " .NET", " ASP.NET", " C++", " C/C++", " C#", " HTML", " CSS", " LINUX", " JAVA SCRIPT", " PHP", " BOOTSTRAP", " JSON", "XML", " J2EE", " SPRING", " JAVASCRIPT", " NODE.JS", " NODEJS", " PHP", " NUXTS", " VUEJS", " RXJS", " REACTIVE", " QUANTLIB", " QNET", " SWIFT", " REACT  ", "PYTHON", " ANGULAR", " WEBAPI", " RESTFFULL", " RESTFFULLAPI", " REST", " API", " WEBFRAMEWORK", " SOAP", " W3C", " HTTP" };
        public static readonly string[] ToolSkillSet = new string[] { "MATLAB","GIT","JENKINS","DEVOPS","JIRA","PACK OFFICE","OFFICE","ORIGIN","UML","ANDROID STUDIO","VISUAL STUDIO","VS CODE","VISUAL STUDIO CODE","SAS","WORD","EXCEL","POWERPOINT","POWER POINT","VISIO","MS PROJECT","MSPROJECT","SONAR","SELINIUM","AIX","GOOGLE ANALYTICS","WORDPRESS","SYMPHONY","ELASTICSEARCH","NUXTJS","VUSAX","BULMA","ELEMENT","KIBANA","SONARCUBE","DOCKER" };
        public static readonly string[] MethodologySkillSet = new string[] { "SCRUM", "ITIL", "TDD", "BDD", "DDD", "DESING PATTERN", "DESING PATTERNS", "SOLID", "AGILE" };
        public static readonly string[] MessageProtocolSkillSet = new string[] { "FIX", "TIBCO", "NATS" };
        public static readonly string[] OperationSyttemAndServerSkillSet = new string[] { "SHELL LINUX", "LINUX", "WINDOWS SERVER", "WINDOWS SERVEUR", "UBUNTU", "IIS" };
        public static readonly string[] DatabaseSkillSet = new string[] { "SQL SERVER","ORACLE","MONGODB","MONGO DB","JBOSS","MYSQL","SQL","NOSQL","POSTGRESQL","TRANSACT-SQL","TRANSACT SQL","POSTGRE SQL","DAPPER","ENTITY FRAMEWORK" };

        private AhoCorasickDoubleArrayTrie<string> _programmingLanguageSkillMatcher;
        private AhoCorasickDoubleArrayTrie<string> _toolSkillMatcher;
        private AhoCorasickDoubleArrayTrie<string> _methodologySkillSet;
        private AhoCorasickDoubleArrayTrie<string> _messageProtocolSkillSet;
        private AhoCorasickDoubleArrayTrie<string> _operationSyttemAndServerSkillSet;
        private AhoCorasickDoubleArrayTrie<string> _databaseSkillSet;

        public SkillSetMapper()
        {
            _programmingLanguageSkillMatcher = new AhoCorasickDoubleArrayTrie<string>();
            _toolSkillMatcher = new AhoCorasickDoubleArrayTrie<string>();
            _methodologySkillSet = new AhoCorasickDoubleArrayTrie<string>();
            _messageProtocolSkillSet = new AhoCorasickDoubleArrayTrie<string>();
            _operationSyttemAndServerSkillSet = new AhoCorasickDoubleArrayTrie<string>();
            _databaseSkillSet = new AhoCorasickDoubleArrayTrie<string>();

            _programmingLanguageSkillMatcher.Build(ProgrammingLanguageSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString())), true);
            _toolSkillMatcher.Build(ToolSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString())), true);
            _methodologySkillSet.Build(MethodologySkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString())), true);
            _messageProtocolSkillSet.Build(MessageProtocolSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString())), true);          
            _operationSyttemAndServerSkillSet.Build(OperationSyttemAndServerSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString())), true);           
            _databaseSkillSet.Build(DatabaseSkillSet.Select((k, i) => new KeyValuePair<string, string>(k, i.ToString())), true);
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
                    resume.Skills.ProgrammingLanguageSkills.Add(element.Trim());
                }
            }
        }

        public void GetToolSkillSet(Resume resume, string line)
        {
            var collectedIndexes = new HashSet<int>();
            _toolSkillMatcher.ParseText(line, hit => { collectedIndexes.Add(int.Parse(hit.Value)); return true; });
            foreach (var index in collectedIndexes)
            {
                var element = ToolSkillSet.ElementAtOrDefault(index);
                if (element != null)
                {
                    resume.Skills.ToolSkillSet.Add(element.Trim());
                }
            }
        }

        public void GetMethodologySkillSet(Resume resume, string line)
        {
            var collectedIndexes = new HashSet<int>();
            _methodologySkillSet.ParseText(line, hit => { collectedIndexes.Add(int.Parse(hit.Value)); return true; });
            foreach (var index in collectedIndexes)
            {
                var element = MethodologySkillSet.ElementAtOrDefault(index);
                if (element != null)
                {
                    resume.Skills.MethodologySkillSet.Add(element.Trim());
                }
            }
        }

        public void GetOperationSyttemAndServerSkillSet(Resume resume, string line)
        {
            var collectedIndexes = new HashSet<int>();
            _operationSyttemAndServerSkillSet.ParseText(line, hit => { collectedIndexes.Add(int.Parse(hit.Value)); return true; });
            foreach (var index in collectedIndexes)
            {
                var element = OperationSyttemAndServerSkillSet.ElementAtOrDefault(index);
                if (element != null)
                {
                    resume.Skills.OperationSyttemAndServerSkillSet.Add(element.Trim());
                }
            }
        }

        public void GetDatabaseSkillSet(Resume resume, string line)
        {
            var collectedIndexes = new HashSet<int>();
            _toolSkillMatcher.ParseText(line, hit => { collectedIndexes.Add(int.Parse(hit.Value)); return true; });
            foreach (var index in collectedIndexes)
            {
                var element = DatabaseSkillSet.ElementAtOrDefault(index);
                if (element != null)
                {
                    resume.Skills.DatabaseSkillSet.Add(element.Trim());
                }
            }
        }
    }
}
