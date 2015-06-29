using MailParsing;
using Xunit;
using FluentAssertions;

namespace Tests
{
    public class MailParserTests
    {
        [Fact]
        public void FactMethodName()
        {
            const string text = "Organize a meeting in Glazgo for us today. This should be remote meeting";
            var result = new MailParser().Parse(text);
//            result.Time.Should().Be("next week");
        }
    }
}
