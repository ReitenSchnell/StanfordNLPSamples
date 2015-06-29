using System;
using MailParsing;
using Xunit;
using FluentAssertions;

namespace Tests
{
    public class MailParserTests
    {
        [Fact]
        public void should_parse_next_month()
        {
            const string text = "Organize a remote meeting in Glazgo for us next month.";
            var date = new DateTime(2015, 1, 1);
            var result = new MailParser().Parse(text, date);
            result.DateAndTime.Literal.Should().Be("next month");
            Console.Out.WriteLine(result.DateAndTime.Expression);
        }

        [Fact]
        public void should_parse_tomorrow()
        {
            const string text = "Organize a remote meeting in Glazgo for us tomorrow.";
            var date = new DateTime(2015, 1, 1);
            var result = new MailParser().Parse(text, date);
            result.DateAndTime.Literal.Should().Be("tomorrow");
            Console.Out.WriteLine(result.DateAndTime.Expression);
        }

        [Fact]
        public void should_return_empty_DateAndTime()
        {
            const string text = "Organize a remote meeting in Glazgo.";
            var date = new DateTime(2015, 1, 1);
            var result = new MailParser().Parse(text, date);
            result.DateAndTime.Literal.Should().BeNullOrEmpty();
            result.DateAndTime.Expression.Should().BeNullOrEmpty();
        }
    }
}
