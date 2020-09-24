﻿using System.Collections.Generic;

namespace Sharpenter.ResumeParser.Model
{
    public class Resume
    {
        public string KeyWord { get; set; }
        public string Platform { get; set; }
        public string FileName { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumbers { get; set; }
        public List<string> SocialProfiles { get; set; }
        public Skills Skills { get; set; }
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
        }

        public HashSet<string> ProgrammingLanguageSkills { get; set; }
    }
}
