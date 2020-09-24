using System;
using System.IO;
using Sharpenter.ResumeParser.Model;
using Sharpenter.ResumeParser.Model.Exceptions;
using Sharpenter.ResumeParser.ResumeProcessor.Parsers;

namespace Sharpenter.ResumeParser.ResumeProcessor
{
    public class ResumeProcessor
    {
        public readonly IOutputFormatter _outputFormatter;
        public readonly IInputReader _inputReaders; 

        public ResumeProcessor(IOutputFormatter outputFormatter)
        {
            if (outputFormatter == null)
            {
                throw new ArgumentNullException("outputFormatter");    
            }

            _outputFormatter = outputFormatter;
            IInputReaderFactory inputReaderFactory = new InputReaderFactory(new ConfigFileApplicationSettingsAdapter());
            _inputReaders = inputReaderFactory.LoadInputReaders();
        }
        public string Process(string location)
        {
            try
            {
                var fileName = Path.GetFileName(location);
                var rawInput = _inputReaders.ReadIntoList(location);
                var parsingManager = new KeywordSearchEngine();
                var resume = parsingManager.Parse(rawInput, fileName);
                return _outputFormatter.Format(resume);
            }
            catch (IOException ex)
            {
                throw new ResumeParserException("There's a problem accessing the file, it might still being opened by other application", ex);
            }            
        }
    }
}
