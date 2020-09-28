using System.Collections.Generic;

namespace Sharpenter.ResumeParser.Model
{
    public class Resume
    {
        public HashSet<string> Skills { get; set; }
        public string KeyWord { get; set; }
        public string Platform { get; set; }
        public string FileName { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumbers { get; set; }
        public List<string> SocialProfiles { get; set; }
        public List<string> Languages { get; set; } // A implementer

        public Resume()
        {
            Skills = new HashSet<string>();
            SocialProfiles = new List<string>();
            Languages = new List<string>();
        }
    }
}
