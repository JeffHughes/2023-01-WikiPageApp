using System;
using System.Collections.Generic;
using System.Text;

namespace WikiPageApp.Model
{
    public class Healthcheck
    {
        public string ApplicationName { get; set; } = "WikiPageApp";
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public DateTime StatusDateTimeUtc { get; set; }
    }
}