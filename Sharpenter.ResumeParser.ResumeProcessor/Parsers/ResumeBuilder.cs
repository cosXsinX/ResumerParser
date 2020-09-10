using System.Collections.Generic;
using System.Linq;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Models;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public class ResumeBuilder
    {
        private readonly Dictionary<SectionType, dynamic> _parserRegistry;
        public ResumeBuilder()
        {
            _parserRegistry = new Dictionary<SectionType, dynamic>
            {
                {SectionType.Personal, new PersonalParser()},
                {SectionType.Skills, new SkillsParser()},
            };
        }
        
        public Resume Build(IList<Section> sections, string fileName)
        {
            var resume = new Resume { FileName = fileName };
            foreach (var section in sections.Where(section => _parserRegistry.ContainsKey(section.Type)))
            {
                _parserRegistry[section.Type].Parse(section, resume);
            }
            return resume;
        }
    }
}
