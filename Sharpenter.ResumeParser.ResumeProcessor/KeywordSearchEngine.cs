using Sharpenter.ResumeParser.Model;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Text;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class KeywordSearchEngine
    {
        public static readonly Regex EmailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex PhoneRegex = new Regex(@"(0|\+33|0033|\+33\(0\)|0033\(0\))[1-9][0-9]{8}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex SocialProfileRegex = new Regex(@"(http(s)?:\/\/)?([\w]+\.)?(linkedin\.com|facebook\.com|github\.com|stackoverflow\.com|bitbucket\.org|sourceforge\.net|(\w+\.)?codeplex\.com|code\.google\.com).*?(?=\s)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex SplitByWhiteSpaceRegex = new Regex(@"\s+|,", RegexOptions.Compiled);
       
        public static readonly string[] KeyWordSet = new string[] { "BI", "C++", "DOTNET", "JAVA-J2EE", "MOA", "ARCHITECTURE", "BIG DATA", "BUSINESS DEVELOPMENT", "CLOUD", "COBOL", "CRM", "DATA SCIENTIST", "DBA", "DELPHI", "DEVOPS", "EMBARQUE", "ERP", "FRONT JAVA", "FRONT", "FULLSTACK", "FULL STACK", "IAM", "INFRA", "MOBILE", "ORACLE", "PEGA ARCHITECT", "PHP", "PYTHON", "QA", "QA TEST", "RESPONSABLE DOMAINE", "SAP", "SCRUM", "SIEBEL", "SYSTÈMES RÉSEAUX", "TECHNICIENS", "TELECOM", "UX-UI", "JAVA J2EE" };
        public static readonly string[] ProgrammingLanguageSkillSet = new string[] { "VISUAL BASIC", "VBA", "EXCEL-DNA","EXCELDNA"," JAVA"," .NET"," ASP.NET"," C++"," C/C++"," C#"," HTML"," CSS"," LINUX"," JAVA SCRIPT"," PHP"," BOOTSTRAP"," JSON","XML"," J2EE"," SPRING"," JAVASCRIPT"," NODE.JS"," NODEJS"," PHP"," NUXTS"," VUEJS"," RXJS"," REACTIVE"," QUANTLIB"," QNET"," SWIFT"," REACT  ","PYTHON"," ANGULAR"," WEBAPI"," RESTFFULL"," RESTFFULLAPI"," REST"," API"," WEBFRAMEWORK"," SOAP"," W3C"," HTTP" };
        //public static readonly string[] methodologySkillSet = new string[] { };     
        //public static readonly string[] toolSkillSet = new string[] { };

        public Resume Parse(IList<string> content, string fileName)
        {
            var resume = new Resume { FileName = fileName };
            var firstNameFound = false;
            var emailFound = false;
            var phoneFound = false;

            ExtractFirstAndLastName(resume, firstNameFound);

            foreach (var line in content)
            {                
                emailFound = ExtractEmail(resume, emailFound, line);
                phoneFound = ExtractPhone(resume, phoneFound, line);
                ExtractSocialProfiles(resume, line);
            }
            return resume;
        }

        private void ExtractSocialProfiles(Resume resume, string line)
        {
            var socialProfileMatches = SocialProfileRegex.Matches(line);
            foreach (Match socialProfileMatch in socialProfileMatches)
            {
                resume.SocialProfiles.Add(socialProfileMatch.Value);
            }
        }

        private bool ExtractPhone(Resume resume, bool phoneFound, string line)
        {
            if (phoneFound) return phoneFound;

            var cleanLine = RemoveUnwantedCharacters(line);
            var phoneMatch = PhoneRegex.Match(cleanLine);

            if (!phoneMatch.Success) return phoneFound;

            resume.PhoneNumbers = phoneMatch.Value;
            phoneFound = true;
            return phoneFound;
        }

        private string RemoveUnwantedCharacters(string line)
        {
            var lineWithNoWhiteSpace = Regex.Replace(line, @"\s+", "");
            var lineWithNoWhiteSpaceNoDots = Regex.Replace(lineWithNoWhiteSpace, @"\.+", "");
            var lineWithNoWhiteSpaceNoDotsNoDash = Regex.Replace(lineWithNoWhiteSpaceNoDots, @"\-+", "");
            var lineWithNoWhiteSpaceNoDotsNoDashNoParenthesis = Regex.Replace(Regex.Replace(lineWithNoWhiteSpaceNoDotsNoDash, @"\(+", ""), @"\)+", "");
            return lineWithNoWhiteSpaceNoDotsNoDashNoParenthesis;
        }

        public bool ExtractFirstAndLastName(Resume resume, bool firstNameFound)
        {
            if (firstNameFound) return firstNameFound;

            try
            {
                var wordArray = Path.GetFileNameWithoutExtension(resume.FileName).Split('_');
                
                // Lookup for keyword.
                foreach (var word in wordArray)
                {
                    var upperCaseWord = word.ToUpper(); 
                    if (KeyWordSet.Contains(upperCaseWord))
                    {
                        resume.KeyWord = upperCaseWord;
                    }
                }

                // Check if keyword and platform are inversed.
                if (!KeyWordSet.Contains(wordArray[1]))
                {
                    resume.Platform = wordArray[1];
                }
                else
                {
                    resume.Platform = wordArray[0];
                }

                // Isolate name and surname
                var sb = new StringBuilder();
                for (int i = 2; i < wordArray.Length; i++)
                {
                    sb.Append(wordArray[i]);
                    sb.Append(" ");
                }
                var name = sb.ToString().Trim();
                var cleanName = name.Replace('_', ' ').Replace('-', ' ').Split(' ')
                                    .Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (cleanName.Length >= 3)
                {
                    resume.Name1 = cleanName[0];
                    resume.Name2 = cleanName[1];
                    resume.Name3 = cleanName[2];
                }
                if (cleanName.Length == 2)
                {
                    resume.Name1 = cleanName[0];
                    resume.Name2 = cleanName[1];
                }
                if (cleanName.Length == 1)
                {
                    resume.Name1 = cleanName[0];
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return firstNameFound;
            }

            firstNameFound = true;
            return firstNameFound;
        }

        private bool ExtractEmail(Resume resume, bool emailFound, string line)
        {
            if (emailFound) return emailFound;
            
            var emailMatch = EmailRegex.Match(Regex.Replace(line, @"\s+", ""));
            if (!emailMatch.Success) return emailFound;

            resume.EmailAddress = emailMatch.Value;
            emailFound = true;

            return emailFound;
        }        
    }
}
 