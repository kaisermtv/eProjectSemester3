using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.General
{
    public class LogEntry
    {
        public DateTime Date { get; set; }
        public string Module { get; set; }
        public string Method { get; set; }
        public string DeclaringType { get; set; }
        public string LineNumber { get; set; }
        public string ErrorMessage { get; set; }
    }
}