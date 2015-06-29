using System;

namespace MailParsing
{
    public class MailParser
    {
        private const string modelsDir = @".\models\";
        
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
