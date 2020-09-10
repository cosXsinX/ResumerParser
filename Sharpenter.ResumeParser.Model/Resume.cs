using System.Collections.Generic;

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
        public List<string> Skills { get; set; }
        /*
        public string Languages { get; set; }
        public string SummaryDescription { get; set; }
        public string Gender { get; set; }
        public string Location { get; set; }
        public List<Position> Positions { get; set; }
        public List<Project> Projects { get; set; }    
        public List<Education> Educations { get; set; }
        public List<string> Courses { get; set; }
        public List<string> Awards { get; set; }
        */

        public Resume()
        {
            SocialProfiles = new List<string>();
            Skills = new List<string>();
            /* 
            Positions = new List<Position>();
            Projects = new List<Project>();
            Educations = new List<Education>();
            Courses = new List<string>();
            Awards = new List<string>();
            */
        }
    }
}
