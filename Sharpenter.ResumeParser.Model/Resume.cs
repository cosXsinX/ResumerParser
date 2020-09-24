using System.Collections.Generic;

namespace Sharpenter.ResumeParser.Model
{
    public class Resume
    {
        public Skills Skills { get; set; }
        public string KeyWord { get; set; }
        public string Platform { get; set; }
        public string FileName { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumbers { get; set; }
        public List<string> SocialProfiles { get; set; }
        public List<string> Languages { get; set; }

        public Resume()
        {
            Skills = new Skills();
            SocialProfiles = new List<string>();
            Languages = new List<string>();
        }
    }

    public class Skills
    {
        public Skills()
        {
            ProgrammingLanguageSkills = new HashSet<string>();
            ToolSkillSet = new HashSet<string>();
            MethodologySkillSet = new HashSet<string>();
            OperationSyttemAndServerSkillSet = new HashSet<string>();
            DatabaseSkillSet = new HashSet<string>();
        }
        public HashSet<string> ProgrammingLanguageSkills { get; set; }
        public HashSet<string> ToolSkillSet { get; set; }
        public HashSet<string> MethodologySkillSet { get; set; }
        public HashSet<string> OperationSyttemAndServerSkillSet { get; set; }
        public HashSet<string> DatabaseSkillSet { get; set; }
    }
}
