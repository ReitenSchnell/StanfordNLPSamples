using System;

namespace MailParsing
{
    public class MailParser
    {
        private const string modelsDir = @"C:\Data\Docs\NLP\stanford-corenlp-3.5.2-models\edu\stanford\nlp\models\";
        
        public ParseResult Parse(string text, DateTime currentDate)
        {
            var parser = new GeneralParser(modelsDir, text);
            return new ParseResult
                {
                    DateAndTime = new DateTimeParser(modelsDir).Parse(text, currentDate),
                    Place = parser.ParsePlace(),
                    Type = parser.ParseType()
                };
        }
    }
}
