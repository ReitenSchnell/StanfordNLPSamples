﻿using System;

namespace MailParsing
{
    public class ParseResult
    {
        public DateTimeInfo DateAndTime { get; set; }
        public string Place { get; set; }
        public string Type { get; set; }
    }

    public class DateTimeInfo
    {
        public string Literal { get; set; }
        public string Expression { get; set; }
    }
}