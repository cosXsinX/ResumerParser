using Sharpenter.ResumeParser.Model;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public interface IParser
    {
        void Parse(Section section, Resume resume);
    }
}
