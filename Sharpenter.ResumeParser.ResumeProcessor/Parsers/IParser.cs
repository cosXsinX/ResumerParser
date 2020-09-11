using Sharpenter.ResumeParser.Model;
using System.Collections.Generic;

namespace Sharpenter.ResumeParser.ResumeProcessor.Parsers
{
    public interface IParser
    {
        Resume Parse(IList<string> content, string fineName);
    }
}
