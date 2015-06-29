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
            result.DateAndTime.Expression.Should().Be("2015-02");
        }

        [Fact]
        public void should_parse_tomorrow()
        {
            const string text = "Organize a remote meeting in Glazgo for us tomorrow.";
            var date = new DateTime(2015, 1, 1);
            var result = new MailParser().Parse(text, date);
            result.DateAndTime.Literal.Should().Be("tomorrow");
            result.DateAndTime.Expression.Should().Be("2015-01-02");
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

        [Fact]
        public void should_recognize_Glazgo_as_place()
        {
            const string text = "Organize a remote meeting in Glazgo for us next month.";
            var date = new DateTime(2015, 1, 1);
            var result = new MailParser().Parse(text, date);
            result.Place.Should().Be("Glazgo");
        }
    }
}
